using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models
{
    public class ServiceCategory
    {
        public int ServiceCategoryID { get; set; }
        
        [StringLength(100)]
        public string? CategoryName { get; set; }
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        [StringLength(50)]
        public string? Icon { get; set; }
        
        public int DisplayOrder { get; set; } = 0;
        
        public bool IsActive { get; set; } = true;
        
        [StringLength(100)]
        public string? CreatedBy { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        [StringLength(100)]
        public string? ModifiedBy { get; set; }
        
        public DateTime? ModifiedDate { get; set; }

        // Navigation properties
        public ICollection<Service>? Services { get; set; }
    }
}
