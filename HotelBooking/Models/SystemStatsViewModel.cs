namespace HotelBooking.Models
{
    public class SystemStatsViewModel
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int TotalRooms { get; set; }
        public int AvailableRooms { get; set; }
        public int TotalReservations { get; set; }
        public int TodayReservations { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal MonthlyRevenue { get; set; }
        public int TotalServices { get; set; }
        public int ActiveServices { get; set; }
        public int TotalNotifications { get; set; }
        public int UnreadNotifications { get; set; }
        public decimal TotalPayments { get; set; }
        public decimal PendingPayments { get; set; }
        public decimal CompletedPayments { get; set; }
        
        // Calculated properties
        public int InactiveUsers => TotalUsers - ActiveUsers;
        public int OccupiedRooms => TotalRooms - AvailableRooms;
        public decimal OccupancyRate => TotalRooms > 0 ? (decimal)OccupiedRooms / TotalRooms * 100 : 0;
        public decimal PaymentCompletionRate => TotalPayments > 0 ? CompletedPayments / TotalPayments * 100 : 0;
        public int ReadNotifications => TotalNotifications - UnreadNotifications;
    }
}
