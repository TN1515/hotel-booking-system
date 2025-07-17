using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Models
{
    public class ServiceInventory
    {
        public int ServiceInventoryID { get; set; }
        public int ServiceID { get; set; }
        [StringLength(100)]
        public string? ItemName { get; set; }
        public int CurrentStock { get; set; }
        public int MinimumStock { get; set; }
        public int MaximumStock { get; set; }
        public int ReorderLevel { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostPerUnit { get; set; }
        [StringLength(50)]
        public string? Unit { get; set; } // pieces, bottles, kg, etc.
        [StringLength(100)]
        public string? Supplier { get; set; }
        public DateTime? LastRestockDate { get; set; }
        public DateTime? NextRestockDate { get; set; }
        [StringLength(50)]
        public string? Status { get; set; } // In Stock, Low Stock, Out of Stock, Discontinued
        [StringLength(500)]
        public string? Notes { get; set; }
        [StringLength(100)]
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        [StringLength(100)]
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        // Navigation properties
        public Service? Service { get; set; }
        public ICollection<InventoryTransaction>? InventoryTransactions { get; set; }
    }

    public class InventoryTransaction
    {
        public int InventoryTransactionID { get; set; }
        public int ServiceInventoryID { get; set; }
        [StringLength(50)]
        public string? TransactionType { get; set; } // In, Out, Adjustment, Waste
        public int Quantity { get; set; }
        public int PreviousStock { get; set; }
        public int NewStock { get; set; }
        [StringLength(500)]
        public string? Reason { get; set; }
        [StringLength(100)]
        public string? Reference { get; set; } // Booking ID, Purchase Order, etc.
        public DateTime TransactionDate { get; set; }
        [StringLength(100)]
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigation properties
        public ServiceInventory? ServiceInventory { get; set; }
    }

    public class ServiceAvailability
    {
        public int ServiceAvailabilityID { get; set; }
        public int ServiceID { get; set; }
        public DateTime AvailableDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public int MaxCapacity { get; set; }
        public int CurrentBookings { get; set; }
        public bool IsAvailable { get; set; } = true;
        [StringLength(500)]
        public string? Notes { get; set; }
        [StringLength(100)]
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigation properties
        public Service? Service { get; set; }
    }
}
