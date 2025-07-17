using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Models
{
    public class Hotel
    {
        public int HotelID { get; set; }
        
        [StringLength(200)]
        public string? HotelName { get; set; }
        
        [StringLength(500)]
        public string? Address { get; set; }
        
        [StringLength(100)]
        public string? City { get; set; }
        
        public int? StateID { get; set; }
        
        public int? CountryID { get; set; }
        
        [StringLength(20)]
        public string? PostalCode { get; set; }
        
        [StringLength(20)]
        public string? Phone { get; set; }
        
        [StringLength(100)]
        public string? Email { get; set; }
        
        [StringLength(200)]
        public string? Website { get; set; }
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        public int StarRating { get; set; } = 5;
        
        [StringLength(500)]
        public string? Amenities { get; set; }
        
        [StringLength(200)]
        public string? CheckInTime { get; set; } = "14:00";
        
        [StringLength(200)]
        public string? CheckOutTime { get; set; } = "12:00";
        
        [StringLength(500)]
        public string? Policies { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        [StringLength(100)]
        public string? CreatedBy { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        [StringLength(100)]
        public string? ModifiedBy { get; set; }
        
        public DateTime? ModifiedDate { get; set; }

        // Navigation properties
        public State? State { get; set; }
        public Country? Country { get; set; }
    }
}
