using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models
{
    public class UserRole
    {
        public int RoleID { get; set; }
        [StringLength(50)]
        public string? RoleName { get; set; }
        public bool IsActive { get; set; }
        [StringLength(255)]
        public string? Description { get; set; }
    }
}
