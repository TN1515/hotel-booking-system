using Microsoft.AspNetCore.Mvc;
using HotelBooking.Models;
using HotelBooking.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace HotelBooking.Controllers
{
    [Authorize(Roles = "Admin,Staff")]
    public class DashboardController : Controller
    {
        private readonly HotelBookingContext _context;

        public DashboardController(HotelBookingContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new DashboardViewModel
            {
                Stats = await GetDashboardStatsAsync(),
                RecentReservations = await GetRecentReservationsAsync(),
                FeaturedHotels = await GetFeaturedHotelsAsync(),
                RoomOccupancy = await GetRoomOccupancyDataAsync(),
                MonthlyRevenue = await GetMonthlyRevenueDataAsync()
            };

            return View(viewModel);
        }

        private async Task<DashboardStats> GetDashboardStatsAsync()
        {
            var today = DateTime.Today;
            var thisMonth = new DateTime(today.Year, today.Month, 1);

            var totalRooms = await _context.Rooms.CountAsync(r => r.IsActive);
            var availableRooms = await _context.Rooms.CountAsync(r => r.IsActive && r.Status == "Available");
            var occupiedRooms = totalRooms - availableRooms;

            var todayReservations = await _context.Reservations
                .Where(r => r.Status == "Confirmed" && r.CheckInDate.Date <= today && r.CheckOutDate.Date > today)
                .Include(r => r.Room)
                .ToListAsync();
            var todayRevenue = todayReservations.Sum(r => r.Room?.Price ?? 0);

            var monthlyReservations = await _context.Reservations
                .Where(r => r.Status == "Confirmed" && r.CreatedDate >= thisMonth)
                .Include(r => r.Room)
                .ToListAsync();
            var monthlyRevenue = monthlyReservations.Sum(r => (r.Room?.Price ?? 0) * (r.CheckOutDate - r.CheckInDate).Days);

            return new DashboardStats
            {
                TotalHotels = 1, // Single hotel system
                TotalRooms = totalRooms,
                ActiveReservations = await _context.Reservations.CountAsync(r => r.Status == "Confirmed"),
                TotalGuests = await _context.Users.CountAsync(u => u.IsActive),
                TodayRevenue = todayRevenue,
                MonthlyRevenue = monthlyRevenue,
                OccupancyRate = totalRooms > 0 ? (double)occupiedRooms / totalRooms * 100 : 0,
                CheckInsToday = await _context.Reservations.CountAsync(r => r.CheckInDate.Date == today),
                CheckOutsToday = await _context.Reservations.CountAsync(r => r.CheckOutDate.Date == today),
                AvailableRooms = availableRooms
            };
        }

        private async Task<List<RecentReservation>> GetRecentReservationsAsync()
        {
            var reservations = await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Room)
                .ThenInclude(room => room!.RoomType)
                .OrderByDescending(r => r.CreatedDate)
                .Take(5)
                .ToListAsync();

            return reservations.Select(r => new RecentReservation
            {
                ReservationId = r.ReservationID,
                GuestName = r.User?.UserName ?? "Unknown",
                RoomNumber = r.Room?.RoomNumber ?? "N/A",
                RoomType = r.Room?.RoomType?.TypeName ?? "N/A",
                CheckInDate = r.CheckInDate,
                CheckOutDate = r.CheckOutDate,
                Status = r.Status ?? "Unknown",
                TotalAmount = r.Room?.Price * (r.CheckOutDate - r.CheckInDate).Days ?? 0
            }).ToList();
        }

        private async Task<List<HotelCardViewModel>> GetFeaturedHotelsAsync()
        {
            // For single hotel system, show featured rooms instead
            var featuredRooms = await _context.Rooms
                .Include(r => r.RoomType)
                .Include(r => r.RoomImages!.Where(ri => ri.IsActive))
                .Where(r => r.IsActive && r.Status == "Available")
                .OrderBy(r => r.RoomNumber)
                .Take(3)
                .ToListAsync();

            return featuredRooms.Select(room => new HotelCardViewModel
            {
                HotelId = room.RoomID,
                Name = $"Room {room.RoomNumber} - {room.RoomType?.TypeName}",
                Location = "Khách sạn của chúng tôi",
                Address = "Địa chỉ khách sạn",
                Description = room.Description ?? $"Phòng {room.RoomType?.TypeName} với đầy đủ tiện nghi",
                ImageUrl = room.RoomImages?.FirstOrDefault()?.ImageData != null ?
                    $"data:image/jpeg;base64,{Convert.ToBase64String(room.RoomImages.First().ImageData!)}" :
                    "/images/default-room.jpg",
                Rating = 4.5m,
                Phone = "+84 123 456 789",
                Email = "info@hotel.com",
                IsActive = true
            }).ToList();
        }

        private async Task<List<RoomOccupancyData>> GetRoomOccupancyDataAsync()
        {
            var roomOccupancy = await _context.Rooms
                .Include(r => r.RoomType)
                .Where(r => r.IsActive)
                .GroupBy(r => r.RoomType!.TypeName)
                .Select(g => new RoomOccupancyData
                {
                    RoomType = g.Key ?? "Unknown",
                    TotalRooms = g.Count(),
                    OccupiedRooms = g.Count(r => r.Status != "Available")
                })
                .ToListAsync();

            return roomOccupancy;
        }

        private async Task<List<RevenueData>> GetMonthlyRevenueDataAsync()
        {
            try
            {
                var sixMonthsAgo = DateTime.Now.AddMonths(-6);
                var monthlyData = new List<RevenueData>();

                var confirmedReservations = await _context.Reservations
                    .Include(r => r.Room)
                    .Where(r => r.Status == "Confirmed" && r.CreatedDate >= sixMonthsAgo)
                    .ToListAsync();

                var groupedData = confirmedReservations
                    .GroupBy(r => new { r.CreatedDate.Year, r.CreatedDate.Month })
                    .Select(g => new RevenueData
                    {
                        Month = $"{g.Key.Year}-{g.Key.Month:00}",
                        Revenue = g.Sum(r => r.Room != null ? r.Room.Price * (r.CheckOutDate - r.CheckInDate).Days : 0),
                        Bookings = g.Count()
                    })
                    .OrderBy(x => x.Month)
                    .ToList();

                return groupedData;
            }
            catch (Exception)
            {
                // Return empty list if there's an error
                return new List<RevenueData>();
            }
        }
    }
}
