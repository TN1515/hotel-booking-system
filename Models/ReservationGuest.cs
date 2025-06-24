namespace HotelBooking.Models
{
    public class ReservationGuest
    {
        public int ReservationGuestID { get; set; }
        public int ReservationID { get; set; }
        public int GuestID { get; set; }

        public Reservation? Reservation { get; set; }
        public Guest? Guest { get; set; }
    }
}
