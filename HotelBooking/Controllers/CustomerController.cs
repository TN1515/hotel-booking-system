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
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(comment))
                {
                    ModelState.AddModelError("comment", "Please provide your feedback comment.");
                    return View();
                }

                if (rating < 1 || rating > 5)
                {
                    ModelState.AddModelError("rating", "Please select a rating from 1 to 5 stars.");
                    return View();
                }

                // Get current user
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    ModelState.AddModelError("", "User not found. Please login again.");
                    return View();
                }

                // Find or create guest profile
                var guest = await _context.Guests.FirstOrDefaultAsync(g => g.UserID == user.Id);

                if (guest == null)
                {
                    // Create guest profile if not exists
                    guest = new Guest
                    {
                        UserID = user.Id,
                        FirstName = user.UserName ?? "Guest",
                        LastName = "",
                        Email = user.Email ?? "",
                        Phone = user.PhoneNumber ?? "",
                        CreatedBy = user.UserName ?? "System",
                        CreatedDate = DateTime.Now
                    };

                    _context.Guests.Add(guest);
                    await _context.SaveChangesAsync();
                }

                // Create feedback record
                var feedback = new Feedback
                {
                    GuestID = guest.GuestID,
                    Comment = comment.Trim(),
                    Rating = rating,
                    FeedbackDate = DateTime.Now,
                    Category = "General" // Default category
                };

                // Add to database
                _context.Feedbacks.Add(feedback);
                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    TempData["SuccessMessage"] = "Thank you for your feedback! Your review has been submitted successfully.";

                    // Log the feedback submission
                    Console.WriteLine($"Feedback saved successfully - ID: {feedback.FeedbackID}, User: {user.UserName}, Rating: {rating}");

                    return RedirectToAction("Feedback");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to save feedback. Please try again.");
                    return View();
                }
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error saving feedback: {ex.Message}");
                ModelState.AddModelError("", "An error occurred while submitting your feedback. Please try again.");
                return View();
            }
        }

        // GET: Customer/Notifications
        public IActionResult Notifications()
        {
            // For now, show a simple notifications page
            ViewBag.Message = "Stay updated with your booking notifications!";
            return View();
        }

        // GET: Customer/MyFeedback - View submitted feedback
        public async Task<IActionResult> MyFeedback()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var guest = await _context.Guests.FirstOrDefaultAsync(g => g.UserID == user.Id);
            if (guest == null)
            {
                ViewBag.Message = "No feedback submitted yet.";
                return View(new List<Feedback>());
            }

            var feedbacks = await _context.Feedbacks
                .Where(f => f.GuestID == guest.GuestID)
                .OrderByDescending(f => f.FeedbackDate)
                .ToListAsync();

            return View(feedbacks);
        }

        // AJAX: Test database connection
        [HttpGet]
        public async Task<JsonResult> TestDatabaseConnection()
        {
            try
            {
                var feedbackCount = await _context.Feedbacks.CountAsync();
                var guestCount = await _context.Guests.CountAsync();

                return Json(new {
                    success = true,
                    message = "Database connection successful",
                    feedbackCount = feedbackCount,
                    guestCount = guestCount,
                    connectionString = _context.Database.GetConnectionString()?.Substring(0, 50) + "..."
                });
            }
            catch (Exception ex)
            {
                return Json(new {
                    success = false,
                    message = "Database connection failed: " + ex.Message
                });
            }
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
