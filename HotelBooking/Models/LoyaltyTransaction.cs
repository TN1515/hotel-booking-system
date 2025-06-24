using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Models
{
    public class LoyaltyTransaction
    {
        public int LoyaltyTransactionID { get; set; }
        
        public int UserID { get; set; }
        
        public int? LoyaltyProgramID { get; set; }
        
        public int? ReservationID { get; set; }
        
        [StringLength(50)]
        public string? TransactionType { get; set; } // Earned, Redeemed, Expired, Bonus
        
        public int PointsEarned { get; set; } = 0;
        
        public int PointsRedeemed { get; set; } = 0;
        
        public int PointsBalance { get; set; } = 0;
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountSpent { get; set; } = 0;
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; } = 0;
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        [StringLength(100)]
        public string? ReferenceNumber { get; set; }
        
        public DateTime TransactionDate { get; set; } = DateTime.Now;
        
        public DateTime? ExpiryDate { get; set; }
        
        [StringLength(50)]
        public string? Status { get; set; } = "Active"; // Active, Used, Expired, Cancelled
        
        [StringLength(100)]
        public string? CreatedBy { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation properties
        public CustomUser? User { get; set; }
        public LoyaltyProgram? LoyaltyProgram { get; set; }
        public Reservation? Reservation { get; set; }
    }
}
