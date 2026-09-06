using CommunitySportsBooking.Data;
using CommunitySportsBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommunitySportsBooking.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Bookings/Create?facilityId=1
        [HttpGet]
        public async Task<IActionResult> Create(int? facilityId)
        {
            var memberId = HttpContext.Session.GetInt32("MemberId");
            if (memberId == null)
            {
                TempData["InfoMessage"] = "Please sign in to book a facility.";
                return RedirectToAction("Login", "Account");
            }

            int targetFacilityId = facilityId ?? 1;
            var facility = await _context.Facilities.FindAsync(targetFacilityId) ?? await _context.Facilities.FirstOrDefaultAsync();
            if (facility == null)
            {
                return NotFound();
            }

            ViewBag.Facility = facility;
            ViewBag.AvailableTimeSlots = GetTimeSlots();

            var booking = new Booking
            {
                FacilityId = facility.FacilityId,
                MemberId = memberId.Value,
                BookingDate = DateTime.Today.AddDays(1),
                TotalPrice = facility.HourlyRate
            };

            return View(booking);
        }

        // POST: /Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            var memberId = HttpContext.Session.GetInt32("MemberId");
            if (memberId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var facility = await _context.Facilities.FindAsync(booking.FacilityId);
            if (facility == null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(booking.TimeSlot))
            {
                ModelState.AddModelError("TimeSlot", "Please select a time slot.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Facility = facility;
                ViewBag.AvailableTimeSlots = GetTimeSlots();
                return View(booking);
            }

            // Availability Check: Check if slot is already booked for this facility on the chosen date
            var isAlreadyBooked = await _context.Bookings.AnyAsync(b =>
                b.FacilityId == booking.FacilityId &&
                b.BookingDate.Date == booking.BookingDate.Date &&
                b.TimeSlot == booking.TimeSlot &&
                b.Status == "Confirmed");

            if (isAlreadyBooked)
            {
                ModelState.AddModelError("TimeSlot", $"The time slot '{booking.TimeSlot}' is already booked for this date. Please select another time slot.");
                ViewBag.Facility = facility;
                ViewBag.AvailableTimeSlots = GetTimeSlots();
                return View(booking);
            }

            booking.MemberId = memberId.Value;
            booking.Status = "Confirmed";
            booking.TotalPrice = facility.HourlyRate;
            booking.CreatedAt = DateTime.Now;

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Facility booking confirmed for {facility.Name} on {booking.BookingDate:yyyy-MM-dd} at {booking.TimeSlot}!";
            return RedirectToAction("MyBookings");
        }

        // GET: /Bookings/MyBookings
        public async Task<IActionResult> MyBookings()
        {
            var memberId = HttpContext.Session.GetInt32("MemberId");
            if (memberId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var bookings = await _context.Bookings
                .Include(b => b.Facility)
                .Where(b => b.MemberId == memberId.Value)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

            return View(bookings);
        }

        // POST: /Bookings/Cancel/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var memberId = HttpContext.Session.GetInt32("MemberId");
            if (memberId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.BookingId == id && b.MemberId == memberId.Value);
            if (booking != null)
            {
                booking.Status = "Cancelled";
                await _context.SaveChangesAsync();
                TempData["InfoMessage"] = "Booking has been cancelled.";
            }

            return RedirectToAction("MyBookings");
        }

        private List<string> GetTimeSlots()
        {
            return new List<string>
            {
                "08:00 - 09:00",
                "09:00 - 10:00",
                "10:00 - 11:00",
                "11:00 - 12:00",
                "13:00 - 14:00",
                "14:00 - 15:00",
                "15:00 - 16:00",
                "16:00 - 17:00",
                "17:00 - 18:00",
                "18:00 - 19:00",
                "19:00 - 20:00"
            };
        }
    }
}
