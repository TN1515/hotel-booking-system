using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using HotelBooking.Models;
using HotelBooking.Data;

namespace HotelBooking.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly SignInManager<CustomUser> _signInManager;
        private readonly RoleManager<CustomRole> _roleManager;
        private readonly HotelBookingContext _context;

        public AccountController(
            UserManager<CustomUser> userManager,
            SignInManager<CustomUser> signInManager,
            RoleManager<CustomRole> roleManager,
            HotelBookingContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
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
                Microsoft.AspNetCore.Identity.SignInResult result;
                CustomUser? user = null;

                // Thử đăng nhập bằng email trước
                user = await _userManager.FindByEmailAsync(model.Email!);
                if (user != null)
                {
                    result = await _signInManager.PasswordSignInAsync(user.UserName!, model.Password!, model.RememberMe, lockoutOnFailure: false);
                }
                else
                {
                    // Nếu không tìm thấy bằng email, thử bằng username
                    user = await _userManager.FindByNameAsync(model.Email!);
                    if (user != null)
                    {
                        result = await _signInManager.PasswordSignInAsync(user.UserName!, model.Password!, model.RememberMe, lockoutOnFailure: false);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "User not found with email or username: " + model.Email);
                        return View(model);
                    }
                }

                if (result.Succeeded)
                {
                    TempData["Message"] = $"Welcome back, {user?.UserName}!";

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    // Redirect based on user role
                    if (user != null)
                    {
                        var roles = await _userManager.GetRolesAsync(user);
                        if (roles.Contains("Admin"))
                        {
                            return RedirectToAction("Dashboard", "Admin");
                        }
                        else if (roles.Contains("Staff"))
                        {
                            return RedirectToAction("Index", "Dashboard");
                        }
                    }

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var errorMsg = "Invalid password.";
                    if (result.IsNotAllowed)
                        errorMsg = "Account not allowed to sign in.";
                    else if (result.IsLockedOut)
                        errorMsg = "Account is locked out.";
                    else if (result.RequiresTwoFactor)
                        errorMsg = "Two-factor authentication required.";

                    ModelState.AddModelError(string.Empty, errorMsg);
                }
            }

            return View(model);
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
                // Use role from model or default to Customer
                var selectedRole = string.IsNullOrEmpty(model.Role) ? "Customer" : model.Role;

                // Ensure selected role exists
                var roleExists = await _roleManager.RoleExistsAsync(selectedRole);
                if (!roleExists)
                {
                    var newRole = new CustomRole
                    {
                        Name = selectedRole,
                        NormalizedName = selectedRole.ToUpper(),
                        RoleName = selectedRole,
                        Description = $"{selectedRole} role",
                        IsActive = true,
                        CreatedBy = "System",
                        CreatedDate = DateTime.Now
                    };
                    await _roleManager.CreateAsync(newRole);
                }

                // Get selected role ID
                var roleEntity = await _roleManager.FindByNameAsync(selectedRole);

                var user = new CustomUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    CreatedAt = DateTime.Now,
                    IsActive = true,
                    CreatedBy = "System",
                    CreatedDate = DateTime.Now,
                    CustomRoleId = roleEntity?.Id ?? 2 // Use actual role ID
                };

                var result = await _userManager.CreateAsync(user, model.Password!);

                if (result.Succeeded)
                {
                    // Add user to selected role
                    await _userManager.AddToRoleAsync(user, selectedRole);

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    TempData["Message"] = "Registration successful! Welcome to Hotel Booking System.";

                    // Redirect based on user role
                    if (selectedRole == "Admin")
                    {
                        return RedirectToAction("Dashboard", "Admin");
                    }
                    else if (selectedRole == "Staff")
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
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

        // Debug action to check user roles
        public async Task<IActionResult> CheckRole()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Json(new { message = "User not authenticated" });
            }

            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user!);
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();

            var result = new
            {
                IsAuthenticated = User.Identity.IsAuthenticated,
                UserName = User.Identity.Name,
                UserId = user?.Id,
                Email = user?.Email,
                Roles = roles,
                Claims = claims,
                IsAdmin = User.IsInRole("Admin"),
                IsStaff = User.IsInRole("Staff"),
                IsCustomer = User.IsInRole("Customer")
            };

            return Json(result);
        }

        // Action to recreate demo users with correct roles
        public async Task<IActionResult> RecreateDemo()
        {
            try
            {
                // Delete existing demo users
                var demoEmails = new[] { "admin@hotel.com", "staff@hotel.com", "customer@hotel.com", "admin@demo.com", "staff@demo.com", "customer@demo.com" };
                foreach (var email in demoEmails)
                {
                    var existingUser = await _userManager.FindByEmailAsync(email);
                    if (existingUser != null)
                    {
                        await _userManager.DeleteAsync(existingUser);
                    }
                }

                // Create roles if they don't exist
                var roleManager = HttpContext.RequestServices.GetRequiredService<RoleManager<CustomRole>>();
                string[] roleNames = { "Admin", "Customer", "Staff" };
                foreach (var roleName in roleNames)
                {
                    var roleExist = await roleManager.RoleExistsAsync(roleName);
                    if (!roleExist)
                    {
                        var role = new CustomRole
                        {
                            Name = roleName,
                            NormalizedName = roleName.ToUpper(),
                            RoleName = roleName,
                            Description = $"{roleName} role",
                            IsActive = true,
                            CreatedBy = "System",
                            CreatedDate = DateTime.Now
                        };
                        await roleManager.CreateAsync(role);
                    }
                }

                // Create demo users with correct emails and strong passwords
                var testUsers = new[]
                {
                    new { Email = "admin@hotel.com", UserName = "admin", Role = "Admin", Password = "Admin123!" },
                    new { Email = "staff@hotel.com", UserName = "staff", Role = "Staff", Password = "Staff123!" },
                    new { Email = "customer@hotel.com", UserName = "customer", Role = "Customer", Password = "Customer123!" }
                };

                var results = new List<object>();
                foreach (var userData in testUsers)
                {
                    var role = await roleManager.FindByNameAsync(userData.Role);
                    var user = new CustomUser
                    {
                        UserName = userData.UserName,
                        Email = userData.Email,
                        EmailConfirmed = true,
                        PhoneNumber = "0123456789",
                        IsActive = true,
                        CreatedBy = "System",
                        CreatedDate = DateTime.Now,
                        CustomRoleId = role?.Id ?? 1
                    };

                    var result = await _userManager.CreateAsync(user, userData.Password);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, userData.Role);
                        results.Add(new { User = userData.UserName, Role = userData.Role, Status = "Created" });
                    }
                    else
                    {
                        results.Add(new { User = userData.UserName, Role = userData.Role, Status = "Failed", Errors = result.Errors.Select(e => e.Description) });
                    }
                }

                return Json(new { Success = true, Results = results });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Error = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult DemoAccounts()
        {
            var demoAccounts = new List<object>
            {
                new {
                    Role = "Admin",
                    Username = "admin",
                    Email = "admin@hotel.com",
                    Password = "Admin123",
                    Description = "Quản trị viên hệ thống - Có quyền truy cập tất cả chức năng",
                    Features = new[] { "Quản lý người dùng", "Quản lý phòng", "Báo cáo", "Cài đặt hệ thống" }
                },
                new {
                    Role = "Super Admin",
                    Username = "superadmin",
                    Email = "superadmin@hotel.com",
                    Password = "123456",
                    Description = "Quản trị viên cấp cao - Tài khoản đơn giản với password 123456",
                    Features = new[] { "Quản lý người dùng", "Quản lý phòng", "Báo cáo", "Cài đặt hệ thống", "Toàn quyền" }
                },
                new {
                    Role = "Staff",
                    Username = "staff",
                    Email = "staff@hotel.com",
                    Password = "Staff123",
                    Description = "Nhân viên khách sạn - Quản lý đặt phòng và khách hàng",
                    Features = new[] { "Quản lý đặt phòng", "Xử lý thanh toán", "Quản lý feedback", "Gửi thông báo" }
                },
                new {
                    Role = "Customer",
                    Username = "customer",
                    Email = "customer@hotel.com",
                    Password = "Customer123",
                    Description = "Khách hàng - Đặt phòng và quản lý booking",
                    Features = new[] { "Tìm kiếm phòng", "Đặt phòng", "Xem lịch sử", "Đánh giá dịch vụ" }
                }
            };

            return View(demoAccounts);
        }

        // Debug action to check user info
        public async Task<IActionResult> CheckUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Json(new { found = false, message = "User not found" });
            }

            var roles = await _userManager.GetRolesAsync(user);
            return Json(new {
                found = true,
                username = user.UserName,
                email = user.Email,
                isActive = user.IsActive,
                emailConfirmed = user.EmailConfirmed,
                roles = roles,
                customRoleId = user.CustomRoleId,
                createdDate = user.CreatedDate
            });
        }

    }
}
