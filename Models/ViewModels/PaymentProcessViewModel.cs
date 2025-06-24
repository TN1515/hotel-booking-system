using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models.ViewModels
{
    public class PaymentProcessViewModel
    {
        public int PaymentID { get; set; }
        public int ReservationID { get; set; }
        public decimal Amount { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Status { get; set; }
        public DateTime PaymentDate { get; set; }
        
        // Reservation details
        public string? GuestName { get; set; }
        public string? RoomNumber { get; set; }
        public string? RoomType { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfGuests { get; set; }
        
        // User details
        public string? UserEmail { get; set; }
        public string? UserPhone { get; set; }
    }

    public class ProcessPaymentViewModel
    {
        [Required]
        public int ReservationID { get; set; }
        
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }
        
        [Required]
        [Display(Name = "Payment Method")]
        public string? PaymentMethod { get; set; }
        
        public string? Notes { get; set; }
        
        // Reservation details for display
        public string? GuestName { get; set; }
        public string? RoomNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }
    }

    public class PaymentListViewModel
    {
        public List<PaymentProcessViewModel>? Payments { get; set; }
        public int TotalPayments { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string? SearchTerm { get; set; }
        public string? StatusFilter { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class RefundProcessViewModel
    {
        [Required]
        public int PaymentID { get; set; }
        
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Refund amount must be greater than 0")]
        [Display(Name = "Refund Amount")]
        public decimal RefundAmount { get; set; }
        
        [Required]
        [Display(Name = "Refund Reason")]
        public string? RefundReason { get; set; }
        
        [Required]
        [Display(Name = "Refund Method")]
        public int RefundMethodID { get; set; }
        
        // Payment details for display
        public decimal OriginalAmount { get; set; }
        public string? GuestName { get; set; }
        public string? RoomNumber { get; set; }
        public DateTime PaymentDate { get; set; }
        
        // For dropdown
        public List<RefundMethod>? RefundMethods { get; set; }
    }
}
