using Microsoft.AspNetCore.Mvc;
using HotelBooking.Models;

namespace HotelBooking.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            var viewModel = new DashboardViewModel
            {
                Stats = GetDashboardStats(),
                RecentReservations = GetRecentReservations(),
                FeaturedHotels = GetFeaturedHotels(),
                RoomOccupancy = GetRoomOccupancyData(),
                MonthlyRevenue = GetMonthlyRevenueData()
            };

            return View(viewModel);
        }

        private DashboardStats GetDashboardStats()
        {
            return new DashboardStats
            {
                TotalHotels = 12,
                TotalRooms = 485,
                ActiveReservations = 127,
                TotalGuests = 342,
                TodayRevenue = 15750.00m,
                MonthlyRevenue = 485200.00m,
                OccupancyRate = 78.5,
                CheckInsToday = 23,
                CheckOutsToday = 18,
                AvailableRooms = 104
            };
        }

        private List<RecentReservation> GetRecentReservations()
        {
            return new List<RecentReservation>
            {
                new RecentReservation
                {
                    ReservationId = 1001,
                    GuestName = "John Smith",
                    RoomNumber = "A-101",
                    RoomType = "Deluxe Suite",
                    CheckInDate = DateTime.Today,
                    CheckOutDate = DateTime.Today.AddDays(3),
                    Status = "Confirmed",
                    TotalAmount = 450.00m
                },
                new RecentReservation
                {
                    ReservationId = 1002,
                    GuestName = "Sarah Johnson",
                    RoomNumber = "B-205",
                    RoomType = "Standard Room",
                    CheckInDate = DateTime.Today.AddDays(1),
                    CheckOutDate = DateTime.Today.AddDays(4),
                    Status = "Pending",
                    TotalAmount = 320.00m
                },
                new RecentReservation
                {
                    ReservationId = 1003,
                    GuestName = "Michael Brown",
                    RoomNumber = "C-301",
                    RoomType = "Executive Suite",
                    CheckInDate = DateTime.Today.AddDays(-1),
                    CheckOutDate = DateTime.Today.AddDays(2),
                    Status = "Checked-in",
                    TotalAmount = 680.00m
                },
                new RecentReservation
                {
                    ReservationId = 1004,
                    GuestName = "Emily Davis",
                    RoomNumber = "A-205",
                    RoomType = "Standard Room",
                    CheckInDate = DateTime.Today.AddDays(-2),
                    CheckOutDate = DateTime.Today,
                    Status = "Checked-out",
                    TotalAmount = 240.00m
                },
                new RecentReservation
                {
                    ReservationId = 1005,
                    GuestName = "David Wilson",
                    RoomNumber = "B-101",
                    RoomType = "Deluxe Room",
                    CheckInDate = DateTime.Today.AddDays(2),
                    CheckOutDate = DateTime.Today.AddDays(5),
                    Status = "Confirmed",
                    TotalAmount = 540.00m
                }
            };
        }

        private List<HotelCardViewModel> GetFeaturedHotels()
        {
            return new List<HotelCardViewModel>
            {
                new HotelCardViewModel
                {
                    HotelId = 1,
                    Name = "Grand Plaza Hotel",
                    Location = "New York",
                    Address = "123 Main Street",
                    Description = "Luxury downtown hotel with stunning city views and world-class amenities",
                    ImageUrl = "https://images.unsplash.com/photo-1566073771259-6a8506099945?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80",
                    Rating = 4.5m,
                    Phone = "+1 (212) 555-0123",
                    Email = "info@grandplaza.com",
                    IsActive = true
                },
                new HotelCardViewModel
                {
                    HotelId = 2,
                    Name = "Heritage Inn",
                    Location = "San Francisco",
                    Address = "789 Heritage Avenue",
                    Description = "Boutique hotel in historic downtown with modern amenities and classic charm",
                    ImageUrl = "https://images.unsplash.com/photo-1551882547-ff40c63fe5fa?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80",
                    Rating = 4.3m,
                    Phone = "+1 (415) 555-0789",
                    Email = "contact@heritageinn.com",
                    IsActive = true
                },
                new HotelCardViewModel
                {
                    HotelId = 3,
                    Name = "Ocean View Resort",
                    Location = "Los Angeles",
                    Address = "456 Coastal Drive",
                    Description = "Beachfront resort with spectacular ocean views and premium spa services",
                    ImageUrl = "https://images.unsplash.com/photo-1520250497591-112f2f40a3f4?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80",
                    Rating = 4.7m,
                    Phone = "+1 (310) 555-0456",
                    Email = "reservations@oceanview.com",
                    IsActive = true
                }
            };
        }

        private List<RoomOccupancyData> GetRoomOccupancyData()
        {
            return new List<RoomOccupancyData>
            {
                new RoomOccupancyData { RoomType = "Standard", TotalRooms = 150, OccupiedRooms = 120 },
                new RoomOccupancyData { RoomType = "Deluxe", TotalRooms = 100, OccupiedRooms = 75 },
                new RoomOccupancyData { RoomType = "Suite", TotalRooms = 50, OccupiedRooms = 42 },
                new RoomOccupancyData { RoomType = "Executive", TotalRooms = 30, OccupiedRooms = 28 }
            };
        }

        private List<RevenueData> GetMonthlyRevenueData()
        {
            return new List<RevenueData>
            {
                new RevenueData { Month = "Jan", Revenue = 425000, Bookings = 156 },
                new RevenueData { Month = "Feb", Revenue = 380000, Bookings = 142 },
                new RevenueData { Month = "Mar", Revenue = 465000, Bookings = 178 },
                new RevenueData { Month = "Apr", Revenue = 520000, Bookings = 195 },
                new RevenueData { Month = "May", Revenue = 485000, Bookings = 187 },
                new RevenueData { Month = "Jun", Revenue = 550000, Bookings = 210 }
            };
        }
    }
}
