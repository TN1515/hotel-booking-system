using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models.ViewModels
{
    public class RoomChangeViewModel
    {
        public int ReservationID { get; set; }
        public int CurrentRoomID { get; set; }
        
        [Required]
        [Display(Name = "New Room")]
        public int NewRoomID { get; set; }
        
        [Required]
        [Display(Name = "Change Date")]
        [DataType(DataType.Date)]
        public DateTime ChangeDate { get; set; }
        
        [Required]
        [StringLength(500)]
        [Display(Name = "Reason for Change")]
        public string? Reason { get; set; }

        // Display properties
        public string? CurrentRoomNumber { get; set; }
        public string? CurrentRoomType { get; set; }
        public decimal CurrentRoomPrice { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        
        // Available rooms for selection
        public List<Room>? AvailableRooms { get; set; }
        
        // Price calculation
        public decimal PriceDifference { get; set; }
        public string? PriceDifferenceText { get; set; }
    }

    public class CheckoutViewModel
    {
        public int ReservationID { get; set; }
        public string? RoomNumber { get; set; }
        public string? RoomType { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfNights { get; set; }
        
        // Cost breakdown
        public decimal RoomCost { get; set; }
        public decimal ServiceCost { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalCost { get; set; }
        
        // Services used
        public List<BookingServiceUsage>? ServicesUsed { get; set; }
        
        // Room changes
        public List<RoomChangeHistory>? RoomChanges { get; set; }
        
        // Payment status
        public string? PaymentStatus { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }
    }

    public class AddServiceViewModel
    {
        public int ReservationID { get; set; }
        
        [Required]
        [Display(Name = "Service")]
        public int ServiceID { get; set; }
        
        [Required]
        [Range(1, 100)]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }
        
        [StringLength(500)]
        [Display(Name = "Special Notes")]
        public string? Note { get; set; }

        // Available services
        public List<Service>? AvailableServices { get; set; }
        
        // Current reservation info
        public string? RoomNumber { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}
