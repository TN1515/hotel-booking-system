using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Models
{
    public class Payment
    {
        public int PaymentID { get; set; }
        public int ReservationID { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }
        public int PaymentBatchID { get; set; }

        public Reservation? Reservation { get; set; }
        public PaymentBatch? PaymentBatch { get; set; }
    }
}
