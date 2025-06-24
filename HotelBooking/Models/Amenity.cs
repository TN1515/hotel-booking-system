using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models
{
    public class Amenity
    {
        public int AmenityID { get; set; }
        [StringLength(100)]
        public string? AmenityName { get; set; }
        [StringLength(255)]
        public string? Description { get; set; }
        [StringLength(50)]
        public string? Category { get; set; }
        [StringLength(50)]
        public string? Icon { get; set; }
        public bool IsActive { get; set; }
        [StringLength(100)]
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        [StringLength(100)]
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public ICollection<RoomAmenity>? RoomAmenities { get; set; }
    }
}
