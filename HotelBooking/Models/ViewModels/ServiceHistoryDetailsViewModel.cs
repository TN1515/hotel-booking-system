using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models.ViewModels
{
    public class ServiceHistoryDetailsViewModel
    {
        public int ServiceHistoryID { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public string ServiceCategory { get; set; } = string.Empty;
        public string GuestName { get; set; } = string.Empty;
        public string RoomNumber { get; set; } = string.Empty;
        public DateTime ServiceDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? PaymentDate { get; set; }
        
        // Timeline dates
        public DateTime RequestedDate { get; set; }
        public DateTime? ConfirmedDate { get; set; }
        public DateTime? StartedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        
        // Staff information
        public string? RequestedBy { get; set; }
        public string? ConfirmedBy { get; set; }
        public string? ServiceProvider { get; set; }
        
        // Additional information
        public string? Notes { get; set; }
        public string? GuestFeedback { get; set; }
        public int? Rating { get; set; }
        public DateTime? FeedbackDate { get; set; }
    }
}
