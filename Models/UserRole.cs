using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace HotelBooking.Models
{
    public class UserRole : IdentityUserRole<int>
    {
        // Remove navigation properties to avoid shadow property conflicts
        // Navigation properties will be handled by Identity framework
    }
}
