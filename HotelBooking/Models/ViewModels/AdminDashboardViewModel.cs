namespace HotelBooking.Models
{
    public class AdminDashboardViewModel
    {
        public int TotalReservations { get; set; }
        public int ConfirmedReservations { get; set; }
        public int PendingReservations { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<Reservation> RecentReservations { get; set; } = new List<Reservation>();
    }
}
