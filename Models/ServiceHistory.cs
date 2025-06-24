using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Models
{
    public class ServiceHistory
    {
        public int ServiceHistoryID { get; set; }
        
        public int UserID { get; set; }
        
        public int? ReservationID { get; set; }
        
        public int ServiceID { get; set; }
        
        public DateTime ServiceDate { get; set; } = DateTime.Now;
        
        public int Quantity { get; set; } = 1;
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        
        [StringLength(50)]
        public string? Status { get; set; } = "Completed"; // Requested, In Progress, Completed, Cancelled
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        [StringLength(500)]
        public string? SpecialInstructions { get; set; }
        
        public DateTime? RequestedTime { get; set; }
        
        public DateTime? CompletedTime { get; set; }
        
        public int? ServicedByUserID { get; set; }
        
        public int? Rating { get; set; } // 1-5 stars
        
        [StringLength(1000)]
        public string? Feedback { get; set; }
        
        [StringLength(100)]
        public string? CreatedBy { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        [StringLength(100)]
        public string? ModifiedBy { get; set; }
        
        public DateTime? ModifiedDate { get; set; }

        // Navigation properties
        public CustomUser? User { get; set; }
        public Reservation? Reservation { get; set; }
        public Service? Service { get; set; }
        public CustomUser? ServicedByUser { get; set; }
    }
}
