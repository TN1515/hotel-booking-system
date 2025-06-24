using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models
{
    public class Notification
    {
        public int NotificationID { get; set; }
        public int UserID { get; set; }
        [StringLength(200)]
        public string? Title { get; set; }
        [StringLength(1000)]
        public string? Message { get; set; }
        [StringLength(50)]
        public string? Type { get; set; } // Email, SMS, System
        [StringLength(50)]
        public string? Status { get; set; } // Pending, Sent, Failed
        public DateTime CreatedDate { get; set; }
        public DateTime? SentDate { get; set; }
        public bool IsRead { get; set; }
        [StringLength(100)]
        public string? CreatedBy { get; set; }

        public CustomUser? User { get; set; }
    }
}
