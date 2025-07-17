using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Models
{
    public class LoyaltyProgram
    {
        public int LoyaltyProgramID { get; set; }
        
        [StringLength(100)]
        public string? ProgramName { get; set; }
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        public int PointsPerVND { get; set; } = 1; // 1 point per 1000 VND
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal MinimumSpend { get; set; } = 0;
        
        [StringLength(50)]
        public string? TierLevel { get; set; } // Bronze, Silver, Gold, Platinum
        
        public int RequiredPoints { get; set; } = 0;
        
        [Column(TypeName = "decimal(5,2)")]
        public decimal DiscountPercentage { get; set; } = 0;
        
        [StringLength(500)]
        public string? Benefits { get; set; }
        
        public DateTime StartDate { get; set; } = DateTime.Now;
        
        public DateTime? EndDate { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        [StringLength(100)]
        public string? CreatedBy { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        [StringLength(100)]
        public string? ModifiedBy { get; set; }
        
        public DateTime? ModifiedDate { get; set; }

        // Navigation properties
        public ICollection<LoyaltyTransaction>? LoyaltyTransactions { get; set; }
    }
}
