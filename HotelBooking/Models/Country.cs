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
        [StringLength(100)]
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        [StringLength(100)]
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public ICollection<State>? States { get; set; }
    }
}
