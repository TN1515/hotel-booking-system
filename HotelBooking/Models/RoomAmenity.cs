namespace HotelBooking.Models
{
    public class RoomAmenity
    {
        public int RoomID { get; set; }
        public int AmenityID { get; set; }

        public Room? Room { get; set; }
        public Amenity? Amenity { get; set; }
    }
}
