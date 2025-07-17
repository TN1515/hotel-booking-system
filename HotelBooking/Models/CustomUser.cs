using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Models
{
    public class CustomUser : IdentityUser<int>
    {
        [Column("CustomRoleId")]
        public int? CustomRoleId { get; set; }
        [Column("CustomUserId")]
        public int? CustomUserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastLogin { get; set; }
        public bool IsActive { get; set; } = true;
        [StringLength(100)]
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [StringLength(100)]
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        [ForeignKey("CustomRoleId")]
        public CustomRole? CustomRole { get; set; }
        public ICollection<UserRole>? UserRoles { get; set; }
        public ICollection<Reservation>? Reservations { get; set; }
    }
}
