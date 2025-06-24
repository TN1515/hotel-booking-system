using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models
{
    public class Cancellation
    {
        public int CancellationID { get; set; }
        public int ReservationID { get; set; }
        public DateTime CancellationDate { get; set; }
        [StringLength(255)]
        public string? Reason { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal CancellationFee { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal RefundAmount { get; set; }
        public string? CancellationStatus { get; set; }
        [StringLength(100)]
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        [StringLength(100)]
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public Reservation? Reservation { get; set; }
    }
}
