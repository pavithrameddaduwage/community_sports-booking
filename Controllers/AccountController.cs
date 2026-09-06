using CommunitySportsBooking.Data;
using CommunitySportsBooking.Models;
using CommunitySportsBooking.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommunitySportsBooking.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingMember = await _context.Members
                    .FirstOrDefaultAsync(m => m.Email.ToLower() == model.Email.ToLower());

                if (existingMember != null)
                {
                    ModelState.AddModelError("Email", "An account with this email address already exists.");
                    return View(model);
                }

                var newMember = new Member
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    Password = model.Password,
                    ContactNumber = model.ContactNumber,
                    Address = model.Address,
                    PreferredSports = model.PreferredSports ?? string.Empty,
                    Role = "Member",
                    CreatedAt = DateTime.Now
                };

                _context.Members.Add(newMember);
                await _context.SaveChangesAsync();

                // Automatically log in member after registration
                HttpContext.Session.SetInt32("MemberId", newMember.MemberId);
                HttpContext.Session.SetString("MemberName", newMember.FullName);
                HttpContext.Session.SetString("MemberEmail", newMember.Email);

                TempData["SuccessMessage"] = "Welcome to Community Sports! Your member account has been registered successfully.";
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var member = await _context.Members
                    .FirstOrDefaultAsync(m => m.Email.ToLower() == model.Email.ToLower() && m.Password == model.Password);

                if (member == null)
                {
                    ModelState.AddModelError("", "Invalid email or password credentials.");
                    return View(model);
                }

                HttpContext.Session.SetInt32("MemberId", member.MemberId);
                HttpContext.Session.SetString("MemberName", member.FullName);
                HttpContext.Session.SetString("MemberEmail", member.Email);

                TempData["SuccessMessage"] = $"Welcome back, {member.FullName}!";
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        // GET: /Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["InfoMessage"] = "You have been logged out safely.";
            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Profile
        public async Task<IActionResult> Profile()
        {
            var memberId = HttpContext.Session.GetInt32("MemberId");
            if (memberId == null)
            {
                return RedirectToAction("Login");
            }

            var member = await _context.Members
                .Include(m => m.Bookings)
                    .ThenInclude(b => b.Facility)
                .Include(m => m.Reviews)
                    .ThenInclude(r => r.Facility)
                .FirstOrDefaultAsync(m => m.MemberId == memberId);

            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }
    }
}
