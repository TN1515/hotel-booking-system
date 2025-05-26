namespace HotelBooking.Models
{
    public class RoomAmenity
    {
        public int RoomTypeID { get; set; }
        public int AmenityID { get; set; }

        public RoomType? RoomType { get; set; }
        public Amenity? Amenity { get; set; }
    }
}
