using HotelBooking.Models;

namespace HotelBooking.Models.ViewModels
{
    public class SettingsViewModel
    {
        // System Statistics
        public int TotalUsers { get; set; }
        public int TotalRooms { get; set; }
        public int TotalReservations { get; set; }
        public decimal TotalRevenue { get; set; }

        // Configuration Data
        public List<RoomType> RoomTypes { get; set; } = new List<RoomType>();
        public List<Country> Countries { get; set; } = new List<Country>();
        public List<State> States { get; set; } = new List<State>();
        public List<LoyaltyTier> LoyaltyTiers { get; set; } = new List<LoyaltyTier>();
        public List<Service> Services { get; set; } = new List<Service>();

        // New Room Type
        public string? NewRoomTypeName { get; set; }
        public string? NewRoomTypeDescription { get; set; }
        public int NewRoomTypeMaxOccupancy { get; set; }
        public decimal NewRoomTypeBasePrice { get; set; }

        // New Country
        public string? NewCountryName { get; set; }
        public string? NewCountryCode { get; set; }

        // New Loyalty Tier
        public string? NewLoyaltyTierName { get; set; }
        public int NewLoyaltyTierMinPoints { get; set; }
        public int NewLoyaltyTierMaxPoints { get; set; }
        public decimal NewLoyaltyTierDiscountPercentage { get; set; }

        // New Service
        public string? NewServiceName { get; set; }
        public string? NewServiceDescription { get; set; }
        public decimal NewServicePrice { get; set; }
        public string? NewServiceCategory { get; set; }
    }
}
