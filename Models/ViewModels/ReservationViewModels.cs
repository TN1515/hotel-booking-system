using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models.ViewModels
{
    public class CreateReservationViewModel
    {
        [Required]
        [Display(Name = "Room")]
        public int RoomID { get; set; }

        [Required]
        [Display(Name = "Customer")]
        public int UserID { get; set; }

        [Required]
        [Display(Name = "Check-in Date")]
        [DataType(DataType.Date)]
        public DateTime CheckInDate { get; set; } = DateTime.Today.AddDays(1);

        [Required]
        [Display(Name = "Check-out Date")]
        [DataType(DataType.Date)]
        public DateTime CheckOutDate { get; set; } = DateTime.Today.AddDays(2);

        [Required]
        [Range(1, 10)]
        [Display(Name = "Adults")]
        public int Adults { get; set; } = 1;

        [Range(0, 10)]
        [Display(Name = "Children")]
        public int Children { get; set; } = 0;

        [Required]
        [Display(Name = "Status")]
        public string Status { get; set; } = "Pending";

        [Display(Name = "Special Requests")]
        [StringLength(1000)]
        public string? SpecialRequests { get; set; }

        // Navigation properties for dropdowns
        public List<Room> AvailableRooms { get; set; } = new();
        public List<CustomUser> Customers { get; set; } = new();
    }

    public class ReservationListViewModel
    {
        public List<Reservation> Reservations { get; set; } = new();
        public int TotalReservations { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string? SearchTerm { get; set; }
        public string? StatusFilter { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; }
    }

    public class ReservationDetailsViewModel
    {
        public Reservation Reservation { get; set; } = new();
        public List<Guest> Guests { get; set; } = new();
        public List<BookingServiceUsage> Services { get; set; } = new();
        public List<Payment> Payments { get; set; } = new();
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public bool CanModify { get; set; }
        public bool CanCancel { get; set; }
        public bool CanCheckIn { get; set; }
        public bool CanCheckOut { get; set; }
    }

    public class ReservationSearchViewModel
    {
        [Display(Name = "Check-in Date")]
        [DataType(DataType.Date)]
        public DateTime? CheckInDate { get; set; }

        [Display(Name = "Check-out Date")]
        [DataType(DataType.Date)]
        public DateTime? CheckOutDate { get; set; }

        [Display(Name = "Room Type")]
        public int? RoomTypeID { get; set; }

        [Display(Name = "Number of Guests")]
        [Range(1, 20)]
        public int Guests { get; set; } = 1;

        [Display(Name = "Price Range (Min)")]
        [DataType(DataType.Currency)]
        public decimal? MinPrice { get; set; }

        [Display(Name = "Price Range (Max)")]
        [DataType(DataType.Currency)]
        public decimal? MaxPrice { get; set; }

        [Display(Name = "Amenities")]
        public List<int> SelectedAmenityIds { get; set; } = new();

        // Available options
        public List<RoomType> RoomTypes { get; set; } = new();
        public List<Amenity> Amenities { get; set; } = new();

        // Search results
        public List<Room> AvailableRooms { get; set; } = new();
        public bool HasSearched { get; set; }
    }
}
