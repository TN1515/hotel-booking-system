using HotelBooking.Models;

namespace HotelBooking.Models.ViewModels
{
    public class StaffProfileViewModel
    {
        public CustomUser? User { get; set; }
        public int TodayReservations { get; set; }
        public int MonthlyReservations { get; set; }
        public int PendingReservations { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<RecentActivity> RecentActivities { get; set; } = new();
    }

    public class RecentActivity
    {
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Type { get; set; } = string.Empty;
    }
}
