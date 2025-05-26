using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models
{
    public class PaymentBatch
    {
        public int PaymentBatchID { get; set; }
        public int UserID { get; set; }
        public DateTime PaymentDate { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }
        [StringLength(50)]
        public string? PaymentMethod { get; set; }

        public User? User { get; set; }
    }

}
