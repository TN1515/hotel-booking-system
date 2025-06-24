namespace HotelBooking.Models
{
    public class HomeViewModel
    {
        public List<Room> FeaturedRooms { get; set; } = new List<Room>();
        public List<RoomType> RoomTypes { get; set; } = new List<RoomType>();
    }
}
