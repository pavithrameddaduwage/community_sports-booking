using CommunitySportsBooking.Data;
using CommunitySportsBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CommunitySportsBooking.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var featuredFacilities = await _context.Facilities
                .Include(f => f.Reviews)
                .Take(4)
                .ToListAsync();

            ViewBag.TotalFacilities = await _context.Facilities.CountAsync();
            ViewBag.TotalMembers = await _context.Members.CountAsync();
            ViewBag.TotalBookings = await _context.Bookings.CountAsync();

            return View(featuredFacilities);
        }

        public IActionResult Programs()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
