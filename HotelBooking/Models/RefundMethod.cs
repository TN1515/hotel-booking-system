using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models
{
    public class RefundMethod
    {
        public int MethodID { get; set; }
        [StringLength(50)]
        public string? MethodName { get; set; }
        public bool IsActive { get; set; }
    }
}
