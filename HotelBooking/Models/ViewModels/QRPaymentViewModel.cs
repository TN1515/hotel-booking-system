using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models.ViewModels
{
    public class QRPaymentViewModel
    {
        [Required]
        public int ReservationID { get; set; }
        
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        [Display(Name = "Payment Amount")]
        public decimal Amount { get; set; }
        
        [Display(Name = "Notes")]
        public string? Notes { get; set; }
        
        // Reservation details for display
        [Display(Name = "Guest Name")]
        public string? GuestName { get; set; }
        
        [Display(Name = "Room Number")]
        public string? RoomNumber { get; set; }
        
        [Display(Name = "Room Type")]
        public string? RoomType { get; set; }
        
        [Display(Name = "Check-in Date")]
        public DateTime CheckInDate { get; set; }
        
        [Display(Name = "Check-out Date")]
        public DateTime CheckOutDate { get; set; }
        
        [Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }
        
        [Display(Name = "Paid Amount")]
        public decimal PaidAmount { get; set; }
        
        [Display(Name = "Remaining Amount")]
        public decimal RemainingAmount { get; set; }
    }

    public class QRPaymentDisplayViewModel
    {
        public int QRPaymentID { get; set; }
        public int ReservationID { get; set; }
        public decimal Amount { get; set; }
        public string? BankCode { get; set; }
        public string? AccountNumber { get; set; }
        public string? AccountName { get; set; }
        public string? QRCodeData { get; set; }
        public string? TransactionDescription { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? Status { get; set; }
        public string? TransactionReference { get; set; }
        
        // Reservation details
        public string? GuestName { get; set; }
        public string? RoomNumber { get; set; }
        public string? RoomType { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}
