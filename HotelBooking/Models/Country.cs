using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models
{
    public class Country
    {
        public int CountryID { get; set; }
        [StringLength(50)]
        public string? CountryName { get; set; }
        [StringLength(10)]
        public string? CountryCode { get; set; }
        public bool IsActive { get; set; }
    }
}
