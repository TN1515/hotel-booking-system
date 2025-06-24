using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Models
{
    public class RoomChangeRequest
    {
        public int RoomChangeRequestID { get; set; }
        
        public int ReservationID { get; set; }
        
        public int RequestedByUserID { get; set; }
        
        public int? CurrentRoomID { get; set; }
        
        public int? RequestedRoomID { get; set; }
        
        [StringLength(50)]
        public string? RequestReason { get; set; } // Upgrade, Different View, Accessibility, Other
        
        [StringLength(1000)]
        public string? RequestDetails { get; set; }
        
        public DateTime RequestDate { get; set; } = DateTime.Now;
        
        public DateTime? PreferredDate { get; set; }
        
        [StringLength(50)]
        public string? Status { get; set; } = "Pending"; // Pending, Approved, Rejected, Completed
        
        public int? ProcessedByUserID { get; set; }
        
        public DateTime? ProcessedDate { get; set; }
        
        [StringLength(500)]
        public string? ProcessingNotes { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? AdditionalCost { get; set; }
        
        public int? ApprovedRoomID { get; set; }
        
        [StringLength(100)]
        public string? CreatedBy { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation properties
        public Reservation? Reservation { get; set; }
        public CustomUser? RequestedByUser { get; set; }
        public CustomUser? ProcessedByUser { get; set; }
        public Room? CurrentRoom { get; set; }
        public Room? RequestedRoom { get; set; }
        public Room? ApprovedRoom { get; set; }
    }
}
