using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models
{
    public class CustomRole : IdentityRole<int>
    {
        [StringLength(255)]
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
