using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using HotelBooking.Models;
using HotelBooking.Data;

namespace HotelBooking.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly SignInManager<CustomUser> _signInManager;
        private readonly HotelBookingContext _context;

        public AccountController(
            UserManager<CustomUser> userManager,
            SignInManager<CustomUser> signInManager,
            HotelBookingContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    model.Email!, model.Password!, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email!);
                    TempData["Message"] = $"Welcome back, {user?.UserName}!";

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid email or password.");
                }
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult DemoLogin(string role)
        {
            // Demo accounts for testing
            var accounts = new Dictionary<string, (string email, string password)>
            {
                { "admin", ("admin@hotelpro.com", "Admin123!") },
                { "manager", ("manager@hotelpro.com", "Manager123!") },
                { "guest", ("guest@hotelpro.com", "Guest123!") }
            };

            if (accounts.ContainsKey(role.ToLower()))
            {
                var account = accounts[role.ToLower()];
                TempData["Message"] = $"Demo login as {role} - Email: {account.email}";
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RegisterSimple()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new CustomUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    CreatedAt = DateTime.Now,
                    IsActive = true,
                    CreatedBy = "System",
                    CreatedDate = DateTime.Now,
                    RoleID = 2 // Default to Customer role
                };

                var result = await _userManager.CreateAsync(user, model.Password!);

                if (result.Succeeded)
                {
                    // Add user to Customer role
                    await _userManager.AddToRoleAsync(user, "Customer");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    TempData["Message"] = "Registration successful! Welcome to Hotel Booking System.";
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData["Message"] = "You have been logged out successfully.";
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
