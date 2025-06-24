using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Data;
using HotelBooking.Models;
using HotelBooking.Models.ViewModels;

namespace HotelBooking.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserManagementController : Controller
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly RoleManager<CustomRole> _roleManager;
        private readonly HotelBookingContext _context;

        public UserManagementController(
            UserManager<CustomUser> userManager,
            RoleManager<CustomRole> roleManager,
            HotelBookingContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        // GET: UserManagement
        public async Task<IActionResult> Index(string searchTerm, string roleFilter, string statusFilter, int page = 1, int pageSize = 10)
        {
            var query = _context.Users.AsQueryable();

            // Search
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(u => u.UserName!.Contains(searchTerm) ||
                                        u.Email!.Contains(searchTerm) ||
                                        u.PhoneNumber!.Contains(searchTerm));
            }

            // Role filter
            if (!string.IsNullOrEmpty(roleFilter) && roleFilter != "All")
            {
                if (int.TryParse(roleFilter, out int roleId))
                {
                    query = query.Where(u => u.CustomRoleId == roleId);
                }
            }

            // Status filter
            if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "All")
            {
                bool isActive = statusFilter == "Active";
                query = query.Where(u => u.IsActive == isActive);
            }

            var totalUsers = await query.CountAsync();

            var users = await query
                .OrderByDescending(u => u.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var userViewModels = new List<UserManagementViewModel>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == user.CustomRoleId);

                userViewModels.Add(new UserManagementViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    IsActive = user.IsActive,
                    CreatedDate = user.CreatedDate,
                    LastLogin = user.LastLogin,
                    RoleName = role?.RoleName ?? "Unknown",
                    RoleId = user.CustomRoleId ?? 0
                });
            }

            var viewModel = new UserListViewModel
            {
                Users = userViewModels,
                TotalUsers = totalUsers,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)totalUsers / pageSize),
                SearchTerm = searchTerm,
                RoleFilter = roleFilter,
                StatusFilter = statusFilter,
                Roles = await _context.Roles.Where(r => r.IsActive).ToListAsync()
            };

            return View(viewModel);
        }

        // GET: UserManagement/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == user.CustomRoleId);

            var viewModel = new UserDetailsViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive,
                CreatedDate = user.CreatedDate,
                LastLogin = user.LastLogin,
                RoleName = role?.RoleName ?? "Unknown",
                CreatedBy = user.CreatedBy
            };

            return View(viewModel);
        }

        // GET: UserManagement/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new CreateUserViewModel
            {
                Roles = await _context.Roles.Where(r => r.IsActive).ToListAsync(),
                IsActive = true
            };

            return View(viewModel);
        }

        // POST: UserManagement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new CustomUser
                {
                    UserName = viewModel.Email,
                    Email = viewModel.Email,
                    PhoneNumber = viewModel.PhoneNumber,
                    IsActive = viewModel.IsActive,
                    CustomRoleId = viewModel.RoleId,
                    CreatedBy = User.Identity?.Name ?? "System",
                    CreatedDate = DateTime.Now,
                    CreatedAt = DateTime.Now
                };

                var result = await _userManager.CreateAsync(user, viewModel.Password);

                if (result.Succeeded)
                {
                    // Add user to role
                    var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == viewModel.RoleId);
                    if (role != null)
                    {
                        await _userManager.AddToRoleAsync(user, role.Name!);
                    }

                    TempData["SuccessMessage"] = "User created successfully.";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            viewModel.Roles = await _context.Roles.Where(r => r.IsActive).ToListAsync();
            return View(viewModel);
        }

        // GET: UserManagement/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new EditUserViewModel
            {
                UserId = user.Id,
                UserName = user.UserName ?? "",
                Email = user.Email ?? "",
                PhoneNumber = user.PhoneNumber ?? "",
                IsActive = user.IsActive,
                RoleId = user.CustomRoleId ?? 0,
                Roles = await _context.Roles.Where(r => r.IsActive).ToListAsync()
            };

            return View(viewModel);
        }

        // POST: UserManagement/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditUserViewModel viewModel)
        {
            if (id != viewModel.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
                    if (user == null)
                    {
                        return NotFound();
                    }

                    user.UserName = viewModel.Email;
                    user.Email = viewModel.Email;
                    user.PhoneNumber = viewModel.PhoneNumber;
                    user.IsActive = viewModel.IsActive;
                    user.ModifiedBy = User.Identity?.Name ?? "System";
                    user.ModifiedDate = DateTime.Now;

                    // Update role if changed
                    if (user.CustomRoleId != viewModel.RoleId)
                    {
                        // Remove from old role
                        var oldRole = await _context.Roles.FirstOrDefaultAsync(r => r.Id == user.CustomRoleId);
                        if (oldRole != null)
                        {
                            await _userManager.RemoveFromRoleAsync(user, oldRole.Name!);
                        }

                        // Add to new role
                        var newRole = await _context.Roles.FirstOrDefaultAsync(r => r.Id == viewModel.RoleId);
                        if (newRole != null)
                        {
                            await _userManager.AddToRoleAsync(user, newRole.Name!);
                            user.CustomRoleId = viewModel.RoleId;
                        }
                    }

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        TempData["SuccessMessage"] = "User updated successfully.";
                        return RedirectToAction(nameof(Index));
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred: " + ex.Message);
                }
            }

            viewModel.Roles = await _context.Roles.Where(r => r.IsActive).ToListAsync();
            return View(viewModel);
        }

        // POST: UserManagement/ToggleStatus/5
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return Json(new { success = false, message = "User not found" });
            }

            user.IsActive = !user.IsActive;
            user.ModifiedBy = User.Identity?.Name ?? "System";
            user.ModifiedDate = DateTime.Now;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Json(new { success = true, isActive = user.IsActive });
            }

            return Json(new { success = false, message = "Failed to update user status" });
        }

        // POST: UserManagement/ResetPassword/5
        [HttpPost]
        public async Task<IActionResult> ResetPassword(int id, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return Json(new { success = false, message = "User not found" });
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (result.Succeeded)
            {
                return Json(new { success = true, message = "Password reset successfully" });
            }

            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Json(new { success = false, message = errors });
        }
    }
}
