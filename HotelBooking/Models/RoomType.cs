using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models
{
    public class RoomType
    {
        public int RoomTypeID { get; set; }
        [StringLength(50)]
        public string? TypeName { get; set; }
        [StringLength(255)]
        public string? AccessibilityFeatures { get; set; }
        [StringLength(255)]
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        [StringLength(100)]
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        [StringLength(100)]
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

}
