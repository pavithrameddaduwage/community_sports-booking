using CommunitySportsBooking.Data;
using CommunitySportsBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommunitySportsBooking.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Reviews/Create?facilityId=1
        [HttpGet]
        public async Task<IActionResult> Create(int? facilityId)
        {
            var memberId = HttpContext.Session.GetInt32("MemberId");
            if (memberId == null)
            {
                TempData["InfoMessage"] = "Please sign in to submit a review.";
                return RedirectToAction("Login", "Account");
            }

            int targetFacilityId = facilityId ?? 1;
            var facility = await _context.Facilities.FindAsync(targetFacilityId) ?? await _context.Facilities.FirstOrDefaultAsync();
            if (facility == null)
            {
                return NotFound();
            }

            ViewBag.Facility = facility;

            var review = new Review
            {
                FacilityId = facility.FacilityId,
                MemberId = memberId.Value,
                Rating = 5
            };

            return View(review);
        }

        // POST: /Reviews/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Review review)
        {
            var memberId = HttpContext.Session.GetInt32("MemberId");
            if (memberId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                review.MemberId = memberId.Value;
                review.ReviewDate = DateTime.Now;

                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Thank you for submitting your review!";
                return RedirectToAction("Details", "Facilities", new { id = review.FacilityId });
            }

            var facility = await _context.Facilities.FindAsync(review.FacilityId);
            ViewBag.Facility = facility;
            return View(review);
        }
    }
}
