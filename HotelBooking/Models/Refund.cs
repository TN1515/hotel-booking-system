using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models
{
    public class Refund
    {
        public int RefundID { get; set; }
        public int PaymentID { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal RefundAmount { get; set; }
        public DateTime RefundDate { get; set; }
        [StringLength(255)]
        public string? RefundReason { get; set; }
        public int RefundMethodID { get; set; }
        public int ProcessedByUserID { get; set; }
        [StringLength(50)]
        public string? RefundStatus { get; set; }

        public Payment? Payment { get; set; }
        public RefundMethod? RefundMethod { get; set; }
        public User? ProcessedByUser { get; set; }
    }
}
