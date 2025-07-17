using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Models
{
    public class RoomChangeHistory
    {
        public int RoomChangeHistoryID { get; set; }
        public int ReservationID { get; set; }
        public int OldRoomID { get; set; }
        public int NewRoomID { get; set; }
        public DateTime ChangeDate { get; set; }
        [StringLength(500)]
        public string? Reason { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal OldRoomPrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal NewRoomPrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PriceDifference { get; set; }
        [StringLength(50)]
        public string? Status { get; set; } // Pending, Approved, Rejected
        [StringLength(100)]
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        [StringLength(100)]
        public string? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }

        // Navigation properties
        public Reservation? Reservation { get; set; }
        public Room? OldRoom { get; set; }
        public Room? NewRoom { get; set; }
    }
}
