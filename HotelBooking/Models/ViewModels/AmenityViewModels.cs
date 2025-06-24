using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models.ViewModels
{
    public class AmenityViewModel
    {
        public int AmenityID { get; set; }

        [Required]
        [Display(Name = "Amenity Name")]
        [StringLength(100)]
        public string AmenityName { get; set; } = string.Empty;

        [Display(Name = "Description")]
        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Category")]
        public string Category { get; set; } = string.Empty;

        [Display(Name = "Icon")]
        [StringLength(50)]
        public string? Icon { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        public List<string> Categories { get; set; } = new();
    }

    public class AmenityListViewModel
    {
        public List<Amenity> Amenities { get; set; } = new();
        public int TotalAmenities { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string? SearchTerm { get; set; }
        public string? CategoryFilter { get; set; }
        public string? StatusFilter { get; set; }
        public List<string> Categories { get; set; } = new();
    }

    public class AssignAmenityViewModel
    {
        public int AmenityID { get; set; }
        public string? AmenityName { get; set; }
        public List<Room> Rooms { get; set; } = new();
        public List<int> AssignedRoomIds { get; set; } = new();
        public List<int>? SelectedRoomIds { get; set; }
    }
}
