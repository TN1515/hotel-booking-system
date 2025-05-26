using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models
{
    public class State
    {
        public int StateID { get; set; }
        [StringLength(50)]
        public string? StateName { get; set; }
        public int CountryID { get; set; }
        public bool IsActive { get; set; }

        public Country? Country { get; set; }
    }
}
