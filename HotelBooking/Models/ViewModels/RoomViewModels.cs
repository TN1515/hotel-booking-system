using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models.ViewModels
{
    public class RoomViewModel
    {
        public int RoomID { get; set; }

        [Required]
        [Display(Name = "Room Number")]
        [StringLength(10)]
        public string RoomNumber { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Room Type")]
        public int RoomTypeID { get; set; }

        [Required]
        [Display(Name = "Price per Night")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Status")]
        public string Status { get; set; } = "Available";

        [Display(Name = "Description")]
        [StringLength(1000)]
        public string? Description { get; set; }

        [Display(Name = "Bed Type")]
        [StringLength(50)]
        public string? BedType { get; set; }

        [Display(Name = "View Type")]
        [StringLength(50)]
        public string? ViewType { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Room Images")]
        public List<IFormFile>? RoomImages { get; set; }

        [Display(Name = "Existing Images")]
        public List<string>? ExistingImagePaths { get; set; } = new();

        public List<RoomType> RoomTypes { get; set; } = new();
        public List<Amenity> Amenities { get; set; } = new();
        public List<int> SelectedAmenityIds { get; set; } = new();

        public List<string> StatusOptions { get; set; } = new()
        {
            "Available",
            "Occupied",
            "Maintenance",
            "Out of Order"
        };

        public List<string> BedTypeOptions { get; set; } = new()
        {
            "Single Bed",
            "Double Bed",
            "Queen Bed",
            "King Bed",
            "Twin Beds",
            "Sofa Bed"
        };

        public List<string> ViewTypeOptions { get; set; } = new()
        {
            "City View",
            "Ocean View",
            "Mountain View",
            "Garden View",
            "Pool View",
            "Courtyard View"
        };
    }

    public class RoomListViewModel
    {
        public List<Room> Rooms { get; set; } = new();
        public int TotalRooms { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string? SearchTerm { get; set; }
        public string? RoomTypeFilter { get; set; }
        public string? StatusFilter { get; set; }
        public List<RoomType> RoomTypes { get; set; } = new();
    }
}
