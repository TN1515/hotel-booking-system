using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models
{
    public class SystemLog
    {
        public int SystemLogID { get; set; }
        
        [StringLength(50)]
        public string? LogLevel { get; set; } // Info, Warning, Error, Critical
        
        [StringLength(100)]
        public string? Category { get; set; } // Authentication, Booking, Payment, System
        
        [StringLength(500)]
        public string? Message { get; set; }
        
        public string? Details { get; set; } // JSON or detailed information
        
        [StringLength(100)]
        public string? Source { get; set; } // Controller/Action or Service name
        
        public int? UserID { get; set; }
        
        [StringLength(50)]
        public string? IPAddress { get; set; }
        
        [StringLength(500)]
        public string? UserAgent { get; set; }
        
        [StringLength(200)]
        public string? RequestUrl { get; set; }
        
        [StringLength(10)]
        public string? HttpMethod { get; set; }
        
        public int? StatusCode { get; set; }
        
        public long? ResponseTime { get; set; } // in milliseconds
        
        public string? Exception { get; set; } // Full exception details
        
        [StringLength(100)]
        public string? CorrelationId { get; set; }
        
        public DateTime LogDate { get; set; } = DateTime.Now;
        
        [StringLength(100)]
        public string? MachineName { get; set; }
        
        [StringLength(100)]
        public string? ApplicationVersion { get; set; }

        // Navigation properties
        public CustomUser? User { get; set; }
    }
}
