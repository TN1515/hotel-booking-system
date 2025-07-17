using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models
{
    public class Feedback
    {
        public int FeedbackID { get; set; }
        public int ReservationID { get; set; }
        public int GuestID { get; set; }
        public int Rating { get; set; }
        [StringLength(1000)]
        public string? Comment { get; set; }
        [StringLength(1000)]
        public string? Comments { get; set; }
        [StringLength(50)]
        public string? Category { get; set; }
        [StringLength(1000)]
        public string? Response { get; set; }
        public DateTime? ResponseDate { get; set; }
        [StringLength(100)]
        public string? ResponseBy { get; set; }
        public DateTime FeedbackDate { get; set; }

        public Reservation? Reservation { get; set; }
        public Guest? Guest { get; set; }
    }
}
