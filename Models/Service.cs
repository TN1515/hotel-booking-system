using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Models
{
    public class Service
    {
        public int ServiceID { get; set; }
        [StringLength(100)]
        public string? ServiceName { get; set; }
        [StringLength(500)]
        public string? Description { get; set; }
        [StringLength(50)]
        public string? Category { get; set; } // Food, Beverage, Spa, Laundry, etc.

        public int? ServiceCategoryID { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
        [StringLength(20)]
        public string? Unit { get; set; } // per item, per hour, per service
        public bool IsActive { get; set; } = true;
        [StringLength(100)]
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        [StringLength(100)]
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        // Navigation properties
        public ServiceCategory? ServiceCategory { get; set; }
        public ICollection<BookingServiceUsage>? BookingServiceUsages { get; set; }
        public ICollection<ServiceInventory>? ServiceInventories { get; set; }
        public ICollection<ServiceHistory>? ServiceHistories { get; set; }
    }
}
