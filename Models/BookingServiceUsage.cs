using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Models
{
    public class BookingServiceUsage
    {
        public int BookingServiceUsageID { get; set; }
        public int ReservationID { get; set; }
        public int ServiceID { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }
        public DateTime UsageDate { get; set; }
        [StringLength(500)]
        public string? Note { get; set; }
        [StringLength(50)]
        public string? Status { get; set; } // Ordered, Delivered, Paid
        [StringLength(100)]
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigation properties
        public Reservation? Reservation { get; set; }
        public Service? Service { get; set; }
    }
}
