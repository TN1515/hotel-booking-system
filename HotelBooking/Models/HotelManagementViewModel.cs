namespace HotelBooking.Models
{
    public class HotelManagementViewModel
    {
        public List<HotelCardViewModel> Hotels { get; set; } = new List<HotelCardViewModel>();
        public string SearchQuery { get; set; } = string.Empty;
        public int TotalHotels { get; set; }
        public int ActiveHotels { get; set; }
    }

    public class HotelCardViewModel
    {
        public int HotelId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal Rating { get; set; }
        public bool IsActive { get; set; }
        public string Status => IsActive ? "Active" : "Inactive";
        public string StatusClass => IsActive ? "success" : "secondary";
    }
}
