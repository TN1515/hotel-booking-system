using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models
{
    public class State
    {
        [Key]
        public int StateID { get; set; }
        [StringLength(50)]
        public string? StateName { get; set; }
        [StringLength(10)]
        public string? StateCode { get; set; }
        public int CountryID { get; set; }
        public bool IsActive { get; set; }
        [StringLength(100)]
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        [StringLength(100)]
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public Country? Country { get; set; }
    }
}
