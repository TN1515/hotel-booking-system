using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Data;
using HotelBooking.Models;
using HotelBooking.Models.ViewModels;

namespace HotelBooking.Controllers
{
    [Authorize(Roles = "Staff")]
    public class StaffController : BaseController
    {
        private readonly HotelBookingContext _context;
        private readonly UserManager<CustomUser> _userManager;

        public StaffController(HotelBookingContext context, UserManager<CustomUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Staff/Profile
        public async Task<IActionResult> Profile()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound();
            }

            // Get staff statistics
            var today = DateTime.Today;
            var thisMonth = new DateTime(today.Year, today.Month, 1);

            var staffStats = new StaffProfileViewModel
            {
                User = currentUser,
                TodayReservations = await _context.Reservations
                    .CountAsync(r => r.CreatedDate.Date == today),
                MonthlyReservations = await _context.Reservations
                    .CountAsync(r => r.CreatedDate >= thisMonth),
                PendingReservations = await _context.Reservations
                    .CountAsync(r => r.Status == "Pending"),
                TotalRevenue = await GetTotalRevenueAsync(),
                RecentActivities = await GetRecentActivitiesAsync()
            };

            return View(staffStats);
        }

        // GET: Staff/Dashboard
        public IActionResult Dashboard()
        {
            return RedirectToAction("Index", "Dashboard");
        }

        // GET: Staff/Reservations
        public IActionResult Reservations()
        {
            return RedirectToAction("Index", "Reservations");
        }

        // GET: Staff/Guests
        public IActionResult Guests()
        {
            return RedirectToAction("Index", "Guests");
        }

        // GET: Staff/Reports
        public IActionResult Reports()
        {
            return RedirectToAction("Reports", "Admin");
        }

        private async Task<decimal> GetTotalRevenueAsync()
        {
            try
            {
                var confirmedReservations = await _context.Reservations
                    .Include(r => r.Room)
                    .Where(r => r.Status == "Confirmed")
                    .ToListAsync();

                return confirmedReservations.Sum(r =>
                    (r.Room?.Price ?? 0) * (r.CheckOutDate - r.CheckInDate).Days);
            }
            catch
            {
                return 0;
            }
        }

        private async Task<List<RecentActivity>> GetRecentActivitiesAsync()
        {
            try
            {
                var recentReservations = await _context.Reservations
                    .Include(r => r.User)
                    .Include(r => r.Room)
                    .OrderByDescending(r => r.CreatedDate)
                    .Take(5)
                    .ToListAsync();

                return recentReservations.Select(r => new RecentActivity
                {
                    Description = $"New reservation by {r.User?.UserName ?? "Unknown"} for Room {r.Room?.RoomNumber ?? "N/A"}",
                    Date = r.CreatedDate,
                    Type = "Reservation"
                }).ToList();
            }
            catch
            {
                return new List<RecentActivity>();
            }
        }
    }


}
