using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Models
{
    public class RoomImage
    {
        public int RoomImageID { get; set; }
        
        public int RoomID { get; set; }
        
        [StringLength(500)]
        public string? ImagePath { get; set; }

        [StringLength(200)]
        public string? ImageName { get; set; }

        // Store image as binary data
        public byte[]? ImageData { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }
        
        public bool IsPrimary { get; set; } = false;
        
        public int DisplayOrder { get; set; } = 0;
        
        public bool IsActive { get; set; } = true;
        
        [StringLength(100)]
        public string? CreatedBy { get; set; }
        
        public DateTime CreatedDate { get; set; }
        
        [StringLength(100)]
        public string? ModifiedBy { get; set; }
        
        public DateTime? ModifiedDate { get; set; }

        // Navigation properties
        public virtual Room? Room { get; set; }
    }
}
