using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models.ViewModels
{
    public class GuestProfileViewModel
    {
        public int GuestID { get; set; }
        public int UserID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }

        [Required]
        [Phone]
        [StringLength(15)]
        public string? Phone { get; set; }

        [Display(Name = "Age Group")]
        public string? AgeGroup { get; set; }

        [StringLength(255)]
        public string? Address { get; set; }

        [Required]
        [Display(Name = "Country")]
        public int CountryID { get; set; }

        [Required]
        [Display(Name = "State")]
        public int StateID { get; set; }

        public string? UserName { get; set; }
        public string? CountryName { get; set; }
        public string? StateName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        // Additional properties for profile view
        public DateTime? DateOfBirth { get; set; }
        public string? Nationality { get; set; }
        public string? PreferredRoomType { get; set; }
        public string? SpecialRequests { get; set; }
        public string? DietaryRestrictions { get; set; }
        public string? StaffNotes { get; set; }

        // Booking statistics
        public int TotalBookings { get; set; }
        public int CompletedBookings { get; set; }
        public int PendingBookings { get; set; }
        public int CancelledBookings { get; set; }
        public decimal TotalSpent { get; set; }
        public int LoyaltyPoints { get; set; }

        // Recent bookings
        public List<RecentBookingViewModel> RecentBookings { get; set; } = new();

        // For dropdowns
        public List<Country>? Countries { get; set; }
        public List<State>? States { get; set; }
    }

    public class GuestListViewModel
    {
        public List<GuestProfileViewModel>? Guests { get; set; }
        public int TotalGuests { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string? SearchTerm { get; set; }
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; }
    }

    public class RecentBookingViewModel
    {
        public int ReservationID { get; set; }
        public string? RoomNumber { get; set; }
        public string? RoomType { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Status { get; set; }
    }
}
