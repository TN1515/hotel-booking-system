using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models
{
    public class CustomRole : IdentityRole<int>
    {
        public int RoleID { get; set; }
        [StringLength(100)]
        public string? RoleName { get; set; }
        [StringLength(255)]
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        [StringLength(100)]
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        [StringLength(100)]
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public ICollection<UserRole>? UserRoles { get; set; }
    }
}
