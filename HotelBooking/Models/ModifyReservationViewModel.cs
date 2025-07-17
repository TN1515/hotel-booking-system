using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models
{
    public class ModifyReservationViewModel
    {
        public int ReservationID { get; set; }
        public int RoomID { get; set; }
        public Room? Room { get; set; }

        [Required(ErrorMessage = "Check-in date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "New Check-in Date")]
        public DateTime CheckInDate { get; set; }

        [Required(ErrorMessage = "Check-out date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "New Check-out Date")]
        public DateTime CheckOutDate { get; set; }

        [Required(ErrorMessage = "Number of guests is required")]
        [Range(1, 10, ErrorMessage = "Number of guests must be between 1 and 10")]
        [Display(Name = "Number of Guests")]
        public int NumberOfGuests { get; set; }

        // Original dates for comparison
        public DateTime OriginalCheckInDate { get; set; }
        public DateTime OriginalCheckOutDate { get; set; }

        public decimal NewTotalPrice
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

        public decimal OriginalTotalPrice
        {
            get
            {
                if (Room != null && OriginalCheckOutDate > OriginalCheckInDate)
                {
                    var nights = (OriginalCheckOutDate - OriginalCheckInDate).Days;
                    return Room.Price * nights;
                }
                return 0;
            }
        }

        public decimal PriceDifference
        {
            get
            {
                return NewTotalPrice - OriginalTotalPrice;
            }
        }
    }
}
