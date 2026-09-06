using CommunitySportsBooking.Data;
using CommunitySportsBooking.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommunitySportsBooking.Controllers
{
    public class FacilitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FacilitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Facilities
        public async Task<IActionResult> Index(string? facilityType, string? location, string? searchTerm, string? searchDate, string? searchTime)
        {
            var query = _context.Facilities.Include(f => f.Reviews).AsQueryable();

            if (!string.IsNullOrWhiteSpace(facilityType))
            {
                query = query.Where(f => f.FacilityType.ToLower() == facilityType.ToLower());
            }

            if (!string.IsNullOrWhiteSpace(location))
            {
                query = query.Where(f => f.Location.ToLower().Contains(location.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(f => f.Name.ToLower().Contains(searchTerm.ToLower()) || 
                                         f.Description.ToLower().Contains(searchTerm.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(searchDate) && DateTime.TryParse(searchDate, out var parsedDate) && !string.IsNullOrWhiteSpace(searchTime))
            {
                var bookedFacilityIds = await _context.Bookings
                    .Where(b => b.BookingDate.Date == parsedDate.Date && b.TimeSlot == searchTime && b.Status == "Confirmed")
                    .Select(b => b.FacilityId)
                    .ToListAsync();

                query = query.Where(f => !bookedFacilityIds.Contains(f.FacilityId));
            }

            var facilities = await query.ToListAsync();

            ViewBag.AvailableTimeSlots = new List<string>
            {
                "08:00 - 09:00", "09:00 - 10:00", "10:00 - 11:00", "11:00 - 12:00",
                "13:00 - 14:00", "14:00 - 15:00", "15:00 - 16:00", "16:00 - 17:00",
                "17:00 - 18:00", "18:00 - 19:00", "19:00 - 20:00"
            };

            var viewModel = new FacilitySearchViewModel
            {
                FacilityType = facilityType,
                Location = location,
                SearchTerm = searchTerm,
                SearchDate = searchDate,
                SearchTime = searchTime,
                Facilities = facilities,
                AvailableTypes = await _context.Facilities.Select(f => f.FacilityType).Distinct().ToListAsync(),
                AvailableLocations = await _context.Facilities.Select(f => f.Location).Distinct().ToListAsync()
            };

            return View(viewModel);
        }

        // GET: /Facilities/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var facility = await _context.Facilities
                .Include(f => f.Reviews)
                    .ThenInclude(r => r.Member)
                .Include(f => f.Bookings)
                .FirstOrDefaultAsync(f => f.FacilityId == id);

            if (facility == null)
            {
                return NotFound();
            }

            return View(facility);
        }
    }
}
