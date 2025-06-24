using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using HotelBooking.Models;
using HotelBooking.Models.ViewModels;
using HotelBooking.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomerController : Controller
    {
        private readonly HotelBookingContext _context;
        private readonly UserManager<CustomUser> _userManager;

        public CustomerController(HotelBookingContext context, UserManager<CustomUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Customer/Profile
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var guest = await _context.Guests
                .Include(g => g.Country)
                .Include(g => g.State)
                .FirstOrDefaultAsync(g => g.UserID == user.Id);

            if (guest == null)
            {
                // Redirect to CreateProfile action instead of returning view directly
                return RedirectToAction("CreateProfile");
            }

            return View(guest);
        }

        // GET: Customer/EditProfile
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var guest = await _context.Guests
                .Include(g => g.Country)
                .Include(g => g.State)
                .FirstOrDefaultAsync(g => g.UserID == user.Id);

            if (guest == null)
            {
                return RedirectToAction("CreateProfile");
            }

            var model = new GuestProfileViewModel
            {
                FirstName = guest.FirstName,
                LastName = guest.LastName,
                Email = guest.Email,
                Phone = guest.Phone,
                AgeGroup = guest.AgeGroup,
                Address = guest.Address,
                CountryID = guest.CountryID,
                StateID = guest.StateID
            };

            ViewBag.Countries = await _context.Countries.Where(c => c.IsActive).ToListAsync();
            ViewBag.States = await _context.States.Where(s => s.IsActive).ToListAsync();

            return View(model);
        }

        // POST: Customer/EditProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(GuestProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound();
                }

                var guest = await _context.Guests.FirstOrDefaultAsync(g => g.UserID == user.Id);
                if (guest == null)
                {
                    return NotFound();
                }

                // Update guest information
                guest.FirstName = model.FirstName;
                guest.LastName = model.LastName;
                guest.Email = model.Email;
                guest.Phone = model.Phone;
                guest.AgeGroup = model.AgeGroup;
                guest.Address = model.Address;
                guest.CountryID = model.CountryID;
                guest.StateID = model.StateID;
                guest.ModifiedBy = user.UserName;
                guest.ModifiedDate = DateTime.Now;

                _context.Guests.Update(guest);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Profile updated successfully!";
                return RedirectToAction("Profile");
            }

            ViewBag.Countries = await _context.Countries.Where(c => c.IsActive).ToListAsync();
            ViewBag.States = await _context.States.Where(s => s.IsActive).ToListAsync();

            return View(model);
        }

        // GET: Customer/CreateProfile
        public async Task<IActionResult> CreateProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new GuestProfileViewModel
            {
                UserID = user.Id,
                Email = user.Email,
                Countries = await _context.Countries.Where(c => c.IsActive).ToListAsync(),
                States = await _context.States.Where(s => s.IsActive).ToListAsync()
            };

            ViewBag.Message = "Please complete your profile information.";
            return View(viewModel);
        }

        // POST: Customer/CreateProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProfile(GuestProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound();
                }

                var guest = new Guest
                {
                    UserID = user.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Phone = model.Phone,
                    AgeGroup = model.AgeGroup,
                    Address = model.Address,
                    CountryID = model.CountryID,
                    StateID = model.StateID,
                    CreatedBy = user.UserName,
                    CreatedDate = DateTime.Now
                };

                _context.Guests.Add(guest);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Profile created successfully!";
                return RedirectToAction("Profile");
            }

            // If we got this far, something failed, redisplay form
            model.Countries = await _context.Countries.Where(c => c.IsActive).ToListAsync();
            model.States = await _context.States.Where(s => s.IsActive).ToListAsync();
            return View(model);
        }

        // GET: Customer/Feedback
        public IActionResult Feedback()
        {
            ViewBag.Message = "Share your experience with us!";
            return View();
        }

        // POST: Customer/Feedback
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Feedback(string comment, int rating)
        {
            if (string.IsNullOrEmpty(comment) || rating < 1 || rating > 5)
            {
                ModelState.AddModelError("", "Please provide a valid comment and rating (1-5 stars).");
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            var guest = await _context.Guests.FirstOrDefaultAsync(g => g.UserID == user!.Id);

            if (guest != null)
            {
                var feedback = new Feedback
                {
                    GuestID = guest.GuestID,
                    Comment = comment,
                    Rating = rating,
                    FeedbackDate = DateTime.Now
                };

                _context.Feedbacks.Add(feedback);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Thank you for your feedback!";
                return RedirectToAction("Feedback");
            }

            ModelState.AddModelError("", "Unable to submit feedback. Please try again.");
            return View();
        }

        // GET: Customer/Notifications
        public IActionResult Notifications()
        {
            // For now, show a simple notifications page
            ViewBag.Message = "Stay updated with your booking notifications!";
            return View();
        }

        // AJAX: Get states by country
        [HttpGet]
        public async Task<JsonResult> GetStatesByCountry(int countryId)
        {
            var states = await _context.States
                .Where(s => s.CountryID == countryId && s.IsActive)
                .Select(s => new { stateID = s.StateID, stateName = s.StateName })
                .ToListAsync();

            return Json(states);
        }
    }
}
