using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Models
{
    public class RoomChange
    {
        public int RoomChangeID { get; set; }
        
        public int ReservationID { get; set; }
        
        public int FromRoomID { get; set; }
        
        public int ToRoomID { get; set; }
        
        public DateTime ChangeDate { get; set; } = DateTime.Now;
        
        [StringLength(50)]
        public string? ChangeReason { get; set; } // Upgrade, Downgrade, Maintenance, Guest Request
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal PriceDifference { get; set; } = 0;
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        public int ChangedByUserID { get; set; }
        
        [StringLength(50)]
        public string? Status { get; set; } = "Completed"; // Pending, Completed, Cancelled
        
        public DateTime? EffectiveDate { get; set; }
        
        [StringLength(100)]
        public string? ApprovalReference { get; set; }
        
        [StringLength(100)]
        public string? CreatedBy { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation properties
        public Reservation? Reservation { get; set; }
        public Room? FromRoom { get; set; }
        public Room? ToRoom { get; set; }
        public CustomUser? ChangedByUser { get; set; }
    }
}
