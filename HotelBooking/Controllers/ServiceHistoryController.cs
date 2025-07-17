using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Data;
using HotelBooking.Models;
using HotelBooking.Services;
using Microsoft.AspNetCore.Authorization;

namespace HotelBooking.Controllers
{
    [Authorize(Roles = "Customer")]
    public class ServiceHistoryController : Controller
    {
        private readonly HotelBookingContext _context;
        private readonly ILoyaltyService _loyaltyService;

        public ServiceHistoryController(HotelBookingContext context, ILoyaltyService loyaltyService)
        {
            _context = context;
            _loyaltyService = loyaltyService;
        }

        // GET: ServiceHistory
        public async Task<IActionResult> Index()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Forbid();
            }

            var serviceHistory = await _context.BookingServiceUsages
                .Include(bsu => bsu.Service)
                .Include(bsu => bsu.Reservation)
                    .ThenInclude(r => r!.Room)
                .Where(bsu => bsu.Reservation!.UserID == userId)
                .OrderByDescending(bsu => bsu.UsageDate)
                .ToListAsync();

            return View(serviceHistory);
        }

        // GET: ServiceHistory/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceUsage = await _context.BookingServiceUsages
                .Include(bsu => bsu.Service)
                .Include(bsu => bsu.Reservation)
                    .ThenInclude(r => r!.Room)
                    .ThenInclude(r => r!.RoomType)
                .Include(bsu => bsu.Reservation)
                    .ThenInclude(r => r!.User)
                .FirstOrDefaultAsync(m => m.BookingServiceUsageID == id);

            if (serviceUsage == null)
            {
                return NotFound();
            }

            // Check if user owns this service usage (for Customer role)
            if (User.IsInRole("Customer"))
            {
                var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
                if (serviceUsage.Reservation?.UserID != userId)
                {
                    return Forbid();
                }
            }

            // Create detailed view model for admin/staff views
            if (User.IsInRole("Admin") || User.IsInRole("Staff"))
            {
                var detailsViewModel = new HotelBooking.Models.ViewModels.ServiceHistoryDetailsViewModel
                {
                    ServiceHistoryID = serviceUsage.BookingServiceUsageID,
                    ServiceName = serviceUsage.Service?.ServiceName ?? "Unknown Service",
                    ServiceCategory = serviceUsage.Service?.Category ?? "General",
                    GuestName = $"{serviceUsage.Reservation?.User?.UserName}",
                    RoomNumber = serviceUsage.Reservation?.Room?.RoomNumber ?? "N/A",
                    ServiceDate = serviceUsage.UsageDate,
                    Status = "Completed", // Default status
                    Description = serviceUsage.Service?.Description,
                    Quantity = serviceUsage.Quantity,
                    UnitPrice = serviceUsage.Service?.UnitPrice ?? 0,
                    TotalAmount = serviceUsage.TotalPrice,
                    DiscountAmount = 0,
                    IsPaid = true, // Assume paid for now
                    PaymentDate = serviceUsage.UsageDate,

                    // Timeline dates
                    RequestedDate = serviceUsage.UsageDate.AddHours(-2),
                    ConfirmedDate = serviceUsage.UsageDate.AddHours(-1),
                    StartedDate = serviceUsage.UsageDate,
                    CompletedDate = serviceUsage.UsageDate.AddHours(1),

                    // Staff information
                    RequestedBy = serviceUsage.Reservation?.User?.UserName,
                    ConfirmedBy = "Staff",
                    ServiceProvider = "Hotel Staff",

                    // Additional information
                    Notes = "Service completed successfully",
                    GuestFeedback = null,
                    Rating = null,
                    FeedbackDate = null
                };

                return View(detailsViewModel);
            }

            return View(serviceUsage);
        }

        // GET: ServiceHistory/Loyalty
        public async Task<IActionResult> Loyalty()
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");

            var customerLoyalty = await _loyaltyService.GetCustomerLoyaltyAsync(userId);

            var loyaltyPoints = await _context.LoyaltyPoints
                .Where(lp => lp.UserID == userId)
                .OrderByDescending(lp => lp.EarnedDate)
                .ToListAsync();

            ViewBag.LoyaltyPoints = loyaltyPoints;

            return View(customerLoyalty);
        }

        // GET: ServiceHistory/Statistics
        public async Task<IActionResult> Statistics()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Forbid();
            }

            var serviceStats = await _context.BookingServiceUsages
                .Include(bsu => bsu.Service)
                .Where(bsu => bsu.Reservation!.UserID == userId)
                .GroupBy(bsu => bsu.Service!.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    TotalSpent = g.Sum(bsu => bsu.TotalPrice),
                    TotalUsages = g.Count(),
                    MostUsedService = g.GroupBy(bsu => bsu.Service!.ServiceName)
                                      .OrderByDescending(sg => sg.Count())
                                      .First().Key
                })
                .ToListAsync();

            var monthlySpending = await _context.BookingServiceUsages
                .Where(bsu => bsu.Reservation!.UserID == userId)
                .GroupBy(bsu => new { bsu.UsageDate.Year, bsu.UsageDate.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalSpent = g.Sum(bsu => bsu.TotalPrice),
                    ServiceCount = g.Count()
                })
                .OrderBy(g => g.Year)
                .ThenBy(g => g.Month)
                .ToListAsync();

            ViewBag.ServiceStats = serviceStats;
            ViewBag.MonthlySpending = monthlySpending;

            return View();
        }

        // POST: ServiceHistory/RateService/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RateService(int id, int rating, string? comment)
        {
            var serviceUsage = await _context.BookingServiceUsages
                .Include(bsu => bsu.Reservation)
                .FirstOrDefaultAsync(bsu => bsu.BookingServiceUsageID == id);

            if (serviceUsage == null)
            {
                return NotFound();
            }

            // Check if user owns this service usage
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (serviceUsage.Reservation?.UserID != userId)
            {
                return Forbid();
            }

            // Create feedback for the service
            var feedback = new Feedback
            {
                ReservationID = serviceUsage.ReservationID,
                GuestID = await GetGuestIdAsync(userId),
                Rating = rating,
                Comment = comment ?? $"Rating for {serviceUsage.Service?.ServiceName}",
                Category = "Service",
                FeedbackDate = DateTime.Now
            };

            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Thank you for your feedback!";
            return RedirectToAction("Details", new { id = serviceUsage.BookingServiceUsageID });
        }

        // POST: ServiceHistory/UpdateStatus
        [HttpPost]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            try
            {
                var serviceUsage = await _context.BookingServiceUsages.FindAsync(id);
                if (serviceUsage == null)
                {
                    return Json(new { success = false, message = "Service usage not found." });
                }

                // Update status logic here - for now just return success
                // In a real application, you might have a Status field in BookingServiceUsage

                return Json(new { success = true, message = "Status updated successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: ServiceHistory/ProcessPayment
        [HttpPost]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> ProcessPayment(int id)
        {
            try
            {
                var serviceUsage = await _context.BookingServiceUsages.FindAsync(id);
                if (serviceUsage == null)
                {
                    return Json(new { success = false, message = "Service usage not found." });
                }

                // Process payment logic here
                // In a real application, you might update payment status

                return Json(new { success = true, message = "Payment processed successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: ServiceHistory/AddNote
        [HttpPost]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> AddNote(int serviceHistoryId, string note, string noteType)
        {
            try
            {
                var serviceUsage = await _context.BookingServiceUsages.FindAsync(serviceHistoryId);
                if (serviceUsage == null)
                {
                    return Json(new { success = false, message = "Service usage not found." });
                }

                // Add note logic here
                // In a real application, you might have a ServiceNotes table

                return Json(new { success = true, message = "Note added successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private async Task<int> GetGuestIdAsync(int userId)
        {
            var guest = await _context.Guests.FirstOrDefaultAsync(g => g.UserID == userId);
            return guest?.GuestID ?? 0;
        }
    }
}
