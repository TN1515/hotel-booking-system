namespace HotelBooking.Models
{
    public class DashboardViewModel
    {
        public DashboardStats Stats { get; set; } = new DashboardStats();
        public List<RecentReservation> RecentReservations { get; set; } = new List<RecentReservation>();
        public List<HotelCardViewModel> FeaturedHotels { get; set; } = new List<HotelCardViewModel>();
        public List<RoomOccupancyData> RoomOccupancy { get; set; } = new List<RoomOccupancyData>();
        public List<RevenueData> MonthlyRevenue { get; set; } = new List<RevenueData>();
    }

    public class DashboardStats
    {
        public int TotalHotels { get; set; }
        public int TotalRooms { get; set; }
        public int ActiveReservations { get; set; }
        public int TotalGuests { get; set; }
        public decimal TodayRevenue { get; set; }
        public decimal MonthlyRevenue { get; set; }
        public double OccupancyRate { get; set; }
        public int CheckInsToday { get; set; }
        public int CheckOutsToday { get; set; }
        public int AvailableRooms { get; set; }
    }

    public class RecentReservation
    {
        public int ReservationId { get; set; }
        public string GuestName { get; set; } = string.Empty;
        public string RoomNumber { get; set; } = string.Empty;
        public string RoomType { get; set; } = string.Empty;
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string StatusClass => Status.ToLower() switch
        {
            "confirmed" => "success",
            "pending" => "warning",
            "cancelled" => "danger",
            "checked-in" => "info",
            "checked-out" => "secondary",
            _ => "primary"
        };
    }

    public class RoomOccupancyData
    {
        public string RoomType { get; set; } = string.Empty;
        public int TotalRooms { get; set; }
        public int OccupiedRooms { get; set; }
        public double OccupancyPercentage => TotalRooms > 0 ? (double)OccupiedRooms / TotalRooms * 100 : 0;
    }

    public class RevenueData
    {
        public string Month { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
        public int Bookings { get; set; }
    }
}
