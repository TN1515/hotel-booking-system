using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Models
{
    public class LoyaltyPoint
    {
        public int LoyaltyPointID { get; set; }
        public int UserID { get; set; }
        public int? ReservationID { get; set; }
        public int? ServiceUsageID { get; set; }
        [StringLength(50)]
        public string? PointType { get; set; } // Room, Service, Bonus, Referral
        public int PointsEarned { get; set; }
        public int PointsUsed { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountSpent { get; set; }
        [StringLength(500)]
        public string? Description { get; set; }
        public DateTime EarnedDate { get; set; }
        public DateTime? UsedDate { get; set; }
        [StringLength(50)]
        public string? Status { get; set; } // Active, Used, Expired
        public DateTime? ExpiryDate { get; set; }
        [StringLength(100)]
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigation properties
        public CustomUser? User { get; set; }
        public Reservation? Reservation { get; set; }
        public BookingServiceUsage? ServiceUsage { get; set; }
    }

    public class LoyaltyTier
    {
        public int LoyaltyTierID { get; set; }
        [StringLength(50)]
        public string? TierName { get; set; } // Bronze, Silver, Gold, Platinum
        public int MinPoints { get; set; }
        public int MaxPoints { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal DiscountPercentage { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal PointMultiplier { get; set; }
        [StringLength(500)]
        public string? Benefits { get; set; }
        public bool IsActive { get; set; } = true;
        [StringLength(100)]
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigation properties
        public ICollection<CustomerLoyalty>? CustomerLoyalties { get; set; }
    }

    public class CustomerLoyalty
    {
        public int CustomerLoyaltyID { get; set; }
        public int UserID { get; set; }
        public int LoyaltyTierID { get; set; }
        public int TotalPointsEarned { get; set; }
        public int TotalPointsUsed { get; set; }
        public int CurrentPoints { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmountSpent { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime? LastActivityDate { get; set; }
        [StringLength(100)]
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        [StringLength(100)]
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        // Navigation properties
        public CustomUser? User { get; set; }
        public LoyaltyTier? LoyaltyTier { get; set; }
    }
}
