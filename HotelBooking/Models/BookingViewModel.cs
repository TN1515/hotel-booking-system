using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models
{
    public class BookingViewModel
    {
        public int RoomID { get; set; }
        public Room? Room { get; set; }

        [Required(ErrorMessage = "Check-in date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Check-in Date")]
        public DateTime CheckInDate { get; set; }

        [Required(ErrorMessage = "Check-out date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Check-out Date")]
        public DateTime CheckOutDate { get; set; }

        [Required(ErrorMessage = "Number of guests is required")]
        [Range(1, 10, ErrorMessage = "Number of guests must be between 1 and 10")]
        [Display(Name = "Number of Guests")]
        public int NumberOfGuests { get; set; }

        [StringLength(500, ErrorMessage = "Special requests cannot exceed 500 characters")]
        [Display(Name = "Special Requests")]
        public string? SpecialRequests { get; set; }

        public decimal TotalPrice
        {
            get
            {
                if (Room != null && CheckOutDate > CheckInDate)
                {
                    var nights = (CheckOutDate - CheckInDate).Days;
                    return Room.Price * nights;
                }
                return 0;
            }
        }

        public int NumberOfNights
        {
            get
            {
                return CheckOutDate > CheckInDate ? (CheckOutDate - CheckInDate).Days : 0;
            }
        }
    }
}
