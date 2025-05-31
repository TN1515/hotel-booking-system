using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace HotelBooking.Models
{
    public class UserRole : IdentityUserRole<int>
    {
        public CustomUser? User { get; set; }
        public CustomRole? Role { get; set; }
    }
}
