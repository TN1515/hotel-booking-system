namespace HotelBooking.Models.ViewModels
{
    public class PaymentSuccessViewModel
    {
        public string TransactionId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public DateTime PaymentDate { get; set; }
        public string Status { get; set; } = "Completed";
        
        // Booking information
        public int ReservationId { get; set; }
        public string GuestName { get; set; } = string.Empty;
        public string RoomNumber { get; set; } = string.Empty;
        public string RoomType { get; set; } = string.Empty;
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        
        // Additional information
        public string? ConfirmationCode { get; set; }
        public string? Notes { get; set; }
    }
}
