using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models.ViewModels
{
    public class QRPaymentGenerateViewModel
    {
        public int QRPaymentID { get; set; }
        public int ReservationID { get; set; }
        
        [Display(Name = "Amount")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }
        
        [Display(Name = "Bank Code")]
        public string BankCode { get; set; } = string.Empty;
        
        [Display(Name = "Account Number")]
        public string AccountNumber { get; set; } = string.Empty;
        
        [Display(Name = "Account Name")]
        public string AccountName { get; set; } = string.Empty;
        
        public string QRCodeData { get; set; } = string.Empty;
        
        [Display(Name = "Transaction Description")]
        public string TransactionDescription { get; set; } = string.Empty;
        
        [Display(Name = "Transaction Reference")]
        public string TransactionReference { get; set; } = string.Empty;
        
        // Reservation details
        [Display(Name = "Room Number")]
        public string RoomNumber { get; set; } = string.Empty;
        
        [Display(Name = "Room Type")]
        public string RoomType { get; set; } = string.Empty;
        
        [Display(Name = "Check-in Date")]
        [DataType(DataType.Date)]
        public DateTime CheckInDate { get; set; }
        
        [Display(Name = "Check-out Date")]
        [DataType(DataType.Date)]
        public DateTime CheckOutDate { get; set; }
        
        [Display(Name = "Number of Nights")]
        public int NumberOfNights { get; set; }
        
        [Display(Name = "Guest Name")]
        public string GuestName { get; set; } = string.Empty;
    }
}
