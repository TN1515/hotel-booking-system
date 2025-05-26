using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models
{
    public class Guest
    {
        public int GuestID { get; set; }
        public int UserID { get; set; }
        [StringLength(50)]
        public string? FirstName { get; set; }
        [StringLength(50)]
        public string? LastName { get; set; }
        [StringLength(100)]
        public string? Email { get; set; }
        [StringLength(15)]
        public string? Phone { get; set; }
        public string? AgeGroup { get; set; }
        [StringLength(255)]
        public string? Address { get; set; }
        public int CountryID { get; set; }
        public int StateID { get; set; }
        [StringLength(100)]
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        [StringLength(100)]
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public CustomUser? User { get; set; }
        public Country? Country { get; set; }
        public State? State { get; set; }
    }

}
