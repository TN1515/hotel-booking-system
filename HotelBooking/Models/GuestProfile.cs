using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models
{
    public class GuestProfile
    {
        public int GuestProfileID { get; set; }
        
        public int UserID { get; set; }
        
        [StringLength(100)]
        public string? FirstName { get; set; }
        
        [StringLength(100)]
        public string? LastName { get; set; }
        
        public DateTime? DateOfBirth { get; set; }
        
        [StringLength(10)]
        public string? Gender { get; set; }
        
        [StringLength(100)]
        public string? Nationality { get; set; }
        
        [StringLength(50)]
        public string? IDType { get; set; } // Passport, National ID, Driver License
        
        [StringLength(50)]
        public string? IDNumber { get; set; }
        
        [StringLength(20)]
        public string? Phone { get; set; }
        
        [StringLength(100)]
        public string? Email { get; set; }
        
        [StringLength(500)]
        public string? Address { get; set; }
        
        [StringLength(100)]
        public string? City { get; set; }
        
        [StringLength(100)]
        public string? Country { get; set; }
        
        [StringLength(20)]
        public string? PostalCode { get; set; }
        
        [StringLength(100)]
        public string? EmergencyContactName { get; set; }
        
        [StringLength(20)]
        public string? EmergencyContactPhone { get; set; }
        
        [StringLength(500)]
        public string? SpecialRequests { get; set; }
        
        [StringLength(500)]
        public string? DietaryRestrictions { get; set; }
        
        [StringLength(500)]
        public string? Preferences { get; set; }
        
        public bool IsVIP { get; set; } = false;
        
        [StringLength(50)]
        public string? LoyaltyTier { get; set; }
        
        public int TotalStays { get; set; } = 0;
        
        public DateTime? LastStayDate { get; set; }
        
        [StringLength(100)]
        public string? CreatedBy { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        [StringLength(100)]
        public string? ModifiedBy { get; set; }
        
        public DateTime? ModifiedDate { get; set; }

        // Navigation properties
        public CustomUser? User { get; set; }
    }
}
