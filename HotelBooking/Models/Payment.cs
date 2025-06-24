using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Models
{
    public class Payment
    {
        public int PaymentID { get; set; }

        public int ReservationID { get; set; }

        public int? PaymentMethodID { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [StringLength(100)]
        public string? TransactionReference { get; set; }

        [StringLength(50)]
        public string? PaymentStatus { get; set; } = "Pending"; // Pending, Completed, Failed, Refunded

        public DateTime PaymentDate { get; set; } = DateTime.Now;

        public DateTime? ProcessedDate { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        [StringLength(100)]
        public string? ProcessedBy { get; set; }

        public int? PaymentBatchID { get; set; }

        [StringLength(100)]
        public string? CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation properties
        public Reservation? Reservation { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public PaymentBatch? PaymentBatch { get; set; }
    }
}
