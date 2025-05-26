using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models
{
    public class Reservation
    {
        public int ReservationID { get; set; }
        public int UserID { get; set; }
        public int RoomID { get; set; }
        [Column(TypeName = "date")]
        public DateTime BookingDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime CheckInDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime CheckOutDate { get; set; }
        public int NumberOfGuests { get; set; }
        public string? Status { get; set; }
        [StringLength(100)]
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        [StringLength(100)]
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public CustomUser? User { get; set; }
        public Room? Room { get; set; }
    }
}
