using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using HotelBooking.Data;
using HotelBooking.Models;
using HotelBooking.Models.ViewModels;

namespace HotelBooking.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly HotelBookingContext _context;
        private readonly UserManager<CustomUser> _userManager;

        public NotificationController(HotelBookingContext context, UserManager<CustomUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Notification - For Admin/Staff
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Index(string searchTerm, string typeFilter, string statusFilter, DateTime? fromDate, DateTime? toDate, int page = 1, int pageSize = 10)
        {
            var query = _context.Notifications
                .Include(n => n.User)
                .AsQueryable();

            // Search
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(n => n.Title!.Contains(searchTerm) ||
                                        n.Message!.Contains(searchTerm) ||
                                        n.User!.UserName!.Contains(searchTerm));
            }

            // Type filter
            if (!string.IsNullOrEmpty(typeFilter))
            {
                query = query.Where(n => n.Type == typeFilter);
            }

            // Status filter
            if (!string.IsNullOrEmpty(statusFilter))
            {
                query = query.Where(n => n.Status == statusFilter);
            }

            // Date filter
            if (fromDate.HasValue)
            {
                query = query.Where(n => n.CreatedDate >= fromDate.Value);
            }
            if (toDate.HasValue)
            {
                query = query.Where(n => n.CreatedDate <= toDate.Value);
            }

            var totalNotifications = await query.CountAsync();

            var notifications = await query
                .OrderByDescending(n => n.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(n => new NotificationViewModel
                {
                    NotificationID = n.NotificationID,
                    UserID = n.UserID,
                    Title = n.Title,
                    Message = n.Message,
                    Type = n.Type,
                    Status = n.Status,
                    CreatedDate = n.CreatedDate,
                    SentDate = n.SentDate,
                    IsRead = n.IsRead,
                    UserName = n.User!.UserName,
                    UserEmail = n.User.Email
                })
                .ToListAsync();

            var viewModel = new NotificationListViewModel
            {
                Notifications = notifications,
                TotalNotifications = totalNotifications,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)totalNotifications / pageSize),
                SearchTerm = searchTerm,
                TypeFilter = typeFilter,
                StatusFilter = statusFilter,
                FromDate = fromDate,
                ToDate = toDate
            };

            return View(viewModel);
        }

        // GET: Notification/My - For Customer to view their notifications
        public async Task<IActionResult> My(int page = 1, int pageSize = 10)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var query = _context.Notifications
                .Where(n => n.UserID == userId)
                .OrderByDescending(n => n.CreatedDate);

            var totalNotifications = await query.CountAsync();
            var unreadCount = await query.CountAsync(n => !n.IsRead);

            var notifications = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(n => new NotificationViewModel
                {
                    NotificationID = n.NotificationID,
                    UserID = n.UserID,
                    Title = n.Title,
                    Message = n.Message,
                    Type = n.Type,
                    Status = n.Status,
                    CreatedDate = n.CreatedDate,
                    SentDate = n.SentDate,
                    IsRead = n.IsRead
                })
                .ToListAsync();

            var viewModel = new MyNotificationsViewModel
            {
                Notifications = notifications,
                TotalNotifications = totalNotifications,
                UnreadCount = unreadCount,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)totalNotifications / pageSize)
            };

            return View(viewModel);
        }

        // GET: Notification/GetUnreadCount - AJAX endpoint for notification bell
        [HttpGet]
        public async Task<IActionResult> GetUnreadCount()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Json(new { count = 0 });
            }

            var unreadCount = await _context.Notifications
                .CountAsync(n => n.UserID == userId && !n.IsRead);

            return Json(new { count = unreadCount });
        }

        // GET: Notification/GetRecent - AJAX endpoint for notification dropdown
        [HttpGet]
        public async Task<IActionResult> GetRecent(int count = 5)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Json(new { notifications = new List<object>() });
            }

            var notifications = await _context.Notifications
                .Where(n => n.UserID == userId)
                .OrderByDescending(n => n.CreatedDate)
                .Take(count)
                .Select(n => new
                {
                    id = n.NotificationID,
                    title = n.Title,
                    message = n.Message,
                    type = n.Type,
                    isRead = n.IsRead,
                    createdDate = n.CreatedDate,
                    timeAgo = "" // Will be calculated on client side
                })
                .ToListAsync();

            return Json(new { notifications });
        }

        // GET: Notification/Send
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Send(string? title, string? message, string? type)
        {
            var viewModel = new SendNotificationViewModel
            {
                SendImmediately = true,
                Users = await _context.Users.Where(u => u.IsActive).ToListAsync(),
                Title = title ?? string.Empty,
                Message = message ?? string.Empty,
                Type = type ?? string.Empty
            };

            return View(viewModel);
        }

        // POST: Notification/Send
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(SendNotificationViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userIds = new List<int>();

                    // Determine target users
                    switch (viewModel.SendTo)
                    {
                        case "All":
                            userIds = await _context.Users.Where(u => u.IsActive).Select(u => u.Id).ToListAsync();
                            break;
                        case "Specific":
                            userIds = viewModel.UserIDs ?? new List<int>();
                            break;
                        case "Role":
                            if (!string.IsNullOrEmpty(viewModel.RoleName))
                            {
                                // Get users by role name using UserManager
                                var usersInRole = await _userManager.GetUsersInRoleAsync(viewModel.RoleName);
                                userIds = usersInRole.Where(u => u.IsActive).Select(u => u.Id).ToList();
                            }
                            break;
                    }

                    // Create notifications for each user
                    var notifications = new List<Notification>();
                    foreach (var userId in userIds)
                    {
                        var notification = new Notification
                        {
                            UserID = userId,
                            Title = viewModel.Title,
                            Message = viewModel.Message,
                            Type = viewModel.Type,
                            Status = viewModel.SendImmediately ? "Sent" : "Pending",
                            CreatedDate = DateTime.Now,
                            SentDate = viewModel.SendImmediately ? DateTime.Now : viewModel.ScheduleDate,
                            IsRead = false,
                            CreatedBy = User.Identity?.Name
                        };
                        notifications.Add(notification);
                    }

                    _context.Notifications.AddRange(notifications);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = $"Notification sent to {notifications.Count} users successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while sending notifications: " + ex.Message);
                }
            }

            viewModel.Users = await _context.Users.Where(u => u.IsActive).ToListAsync();
            return View(viewModel);
        }

        // GET: Notification/Bulk
        public async Task<IActionResult> Bulk()
        {
            var viewModel = new BulkNotificationViewModel
            {
                Roles = await _context.Roles.Where(r => r.IsActive).ToListAsync()
            };

            return View(viewModel);
        }

        // POST: Notification/Bulk
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Bulk(BulkNotificationViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userIds = new List<int>();

                    // Determine target users based on audience
                    switch (viewModel.TargetAudience)
                    {
                        case "AllUsers":
                            userIds = await _context.Users
                                .Where(u => viewModel.IncludeInactiveUsers || u.IsActive)
                                .Select(u => u.Id)
                                .ToListAsync();
                            break;
                        case "AllGuests":
                            userIds = await _context.Users
                                .Where(u => (viewModel.IncludeInactiveUsers || u.IsActive) && u.CustomRoleId == 2) // Customer role
                                .Select(u => u.Id)
                                .ToListAsync();
                            break;
                        case "AllStaff":
                            userIds = await _context.Users
                                .Where(u => (viewModel.IncludeInactiveUsers || u.IsActive) && (u.CustomRoleId == 1 || u.CustomRoleId == 3)) // Admin or Staff
                                .Select(u => u.Id)
                                .ToListAsync();
                            break;
                        case "SpecificRole":
                            if (viewModel.RoleID.HasValue)
                            {
                                userIds = await _context.Users
                                    .Where(u => (viewModel.IncludeInactiveUsers || u.IsActive) && u.CustomRoleId == viewModel.RoleID.Value)
                                    .Select(u => u.Id)
                                    .ToListAsync();
                            }
                            break;
                    }

                    // Create notifications
                    var notifications = new List<Notification>();
                    foreach (var userId in userIds)
                    {
                        var notification = new Notification
                        {
                            UserID = userId,
                            Title = viewModel.Title,
                            Message = viewModel.Message,
                            Type = viewModel.Type,
                            Status = "Sent",
                            CreatedDate = DateTime.Now,
                            SentDate = DateTime.Now,
                            IsRead = false,
                            CreatedBy = User.Identity?.Name
                        };
                        notifications.Add(notification);
                    }

                    _context.Notifications.AddRange(notifications);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = $"Bulk notification sent to {notifications.Count} users successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while sending bulk notifications: " + ex.Message);
                }
            }

            viewModel.Roles = await _context.Roles.Where(r => r.IsActive).ToListAsync();
            return View(viewModel);
        }

        // GET: Notification/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var notification = await _context.Notifications
                .Include(n => n.User)
                .FirstOrDefaultAsync(n => n.NotificationID == id);

            if (notification == null)
            {
                return NotFound();
            }

            var viewModel = new NotificationViewModel
            {
                NotificationID = notification.NotificationID,
                UserID = notification.UserID,
                Title = notification.Title,
                Message = notification.Message,
                Type = notification.Type,
                Status = notification.Status,
                CreatedDate = notification.CreatedDate,
                SentDate = notification.SentDate,
                IsRead = notification.IsRead,
                UserName = notification.User?.UserName,
                UserEmail = notification.User?.Email
            };

            return View(viewModel);
        }

        // POST: Notification/MarkAsRead/5
        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Json(new { success = false, message = "User not authenticated" });
            }

            var notification = await _context.Notifications.FindAsync(id);
            if (notification != null && (notification.UserID == userId || User.IsInRole("Admin") || User.IsInRole("Staff")))
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Notification not found or access denied" });
        }

        // POST: Notification/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification != null)
            {
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Notification deleted successfully.";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Notification/Templates
        public IActionResult Templates()
        {
            var templates = new List<NotificationTemplateViewModel>
            {
                new NotificationTemplateViewModel
                {
                    TemplateName = "Booking Confirmation",
                    Title = "Booking Confirmed - {ReservationID}",
                    Message = "Dear {GuestName}, your booking for {RoomType} room {RoomNumber} from {CheckInDate} to {CheckOutDate} has been confirmed.",
                    Type = "Email"
                },
                new NotificationTemplateViewModel
                {
                    TemplateName = "Payment Reminder",
                    Title = "Payment Reminder - {ReservationID}",
                    Message = "Dear {GuestName}, this is a reminder that payment of {Amount} is due for your reservation {ReservationID}.",
                    Type = "Email"
                },
                new NotificationTemplateViewModel
                {
                    TemplateName = "Check-in Reminder",
                    Title = "Check-in Reminder - Tomorrow",
                    Message = "Dear {GuestName}, this is a reminder that your check-in is scheduled for tomorrow at {CheckInDate}. Room {RoomNumber} will be ready for you.",
                    Type = "SMS"
                }
            };

            return View(templates);
        }

        // GET: Notification/SeedSampleNotifications - For testing
        [HttpGet]
        public async Task<IActionResult> SeedSampleNotifications()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Json(new { success = false, message = "User not authenticated" });
            }

            // Check if user already has notifications
            var existingCount = await _context.Notifications.CountAsync(n => n.UserID == userId);
            if (existingCount > 0)
            {
                return Json(new { success = false, message = "User already has notifications" });
            }

            var sampleNotifications = new List<Notification>
            {
                new Notification
                {
                    UserID = userId,
                    Title = "Welcome to Hotel Booking System!",
                    Message = "Thank you for joining us! Enjoy exclusive deals and seamless booking experience.",
                    Type = "System",
                    Status = "Sent",
                    CreatedDate = DateTime.Now.AddMinutes(-30),
                    SentDate = DateTime.Now.AddMinutes(-30),
                    IsRead = false,
                    CreatedBy = "System"
                },
                new Notification
                {
                    UserID = userId,
                    Title = "Special Promotion Available",
                    Message = "Get 20% off on your next booking! Use code WELCOME20 at checkout. Valid until end of month.",
                    Type = "Promotion",
                    Status = "Sent",
                    CreatedDate = DateTime.Now.AddMinutes(-15),
                    SentDate = DateTime.Now.AddMinutes(-15),
                    IsRead = false,
                    CreatedBy = "System"
                },
                new Notification
                {
                    UserID = userId,
                    Title = "Booking Reminder",
                    Message = "Don't forget to complete your booking! Your selected room is still available.",
                    Type = "Booking",
                    Status = "Sent",
                    CreatedDate = DateTime.Now.AddMinutes(-5),
                    SentDate = DateTime.Now.AddMinutes(-5),
                    IsRead = false,
                    CreatedBy = "System"
                }
            };

            _context.Notifications.AddRange(sampleNotifications);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = $"Created {sampleNotifications.Count} sample notifications" });
        }

        // GET: Notification/TestSend - Simple test form
        [Authorize(Roles = "Admin,Staff")]
        public IActionResult TestSend()
        {
            return View();
        }

        // GET: Notification/QuickSend - Quick send to all users for testing
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> QuickSend(string? role = null)
        {
            try
            {
                var userIds = new List<int>();
                var targetDescription = "";

                if (string.IsNullOrEmpty(role))
                {
                    // Send to all users
                    userIds = await _context.Users.Where(u => u.IsActive).Select(u => u.Id).ToListAsync();
                    targetDescription = "all users";
                }
                else
                {
                    // Send to specific role
                    var usersInRole = await _userManager.GetUsersInRoleAsync(role);
                    userIds = usersInRole.Where(u => u.IsActive).Select(u => u.Id).ToList();
                    targetDescription = $"all {role}s";
                }

                var notifications = new List<Notification>();
                foreach (var userId in userIds)
                {
                    var notification = new Notification
                    {
                        UserID = userId,
                        Title = $"Admin Announcement - {DateTime.Now:HH:mm}",
                        Message = $"This is a test notification sent by Admin to {targetDescription} at {DateTime.Now:yyyy-MM-dd HH:mm:ss}. Please check your notification system is working properly.",
                        Type = "System",
                        Status = "Sent",
                        CreatedDate = DateTime.Now,
                        SentDate = DateTime.Now,
                        IsRead = false,
                        CreatedBy = User.Identity?.Name ?? "Admin"
                    };
                    notifications.Add(notification);
                }

                _context.Notifications.AddRange(notifications);
                await _context.SaveChangesAsync();

                return Json(new {
                    success = true,
                    message = $"Successfully sent notification to {notifications.Count} {targetDescription}",
                    count = notifications.Count,
                    target = targetDescription
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Helper method to calculate time ago
        private string GetTimeAgo(DateTime dateTime)
        {
            var timeSpan = DateTime.Now - dateTime;

            if (timeSpan.TotalMinutes < 1)
                return "Just now";
            if (timeSpan.TotalMinutes < 60)
                return $"{(int)timeSpan.TotalMinutes} minutes ago";
            if (timeSpan.TotalHours < 24)
                return $"{(int)timeSpan.TotalHours} hours ago";
            if (timeSpan.TotalDays < 7)
                return $"{(int)timeSpan.TotalDays} days ago";
            if (timeSpan.TotalDays < 30)
                return $"{(int)(timeSpan.TotalDays / 7)} weeks ago";
            if (timeSpan.TotalDays < 365)
                return $"{(int)(timeSpan.TotalDays / 30)} months ago";

            return $"{(int)(timeSpan.TotalDays / 365)} years ago";
        }
    }
}
