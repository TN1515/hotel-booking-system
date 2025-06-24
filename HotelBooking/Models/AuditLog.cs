using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models
{
    public class AuditLog
    {
        public int AuditLogID { get; set; }
        
        [StringLength(100)]
        public string? TableName { get; set; }
        
        [StringLength(50)]
        public string? Operation { get; set; } // INSERT, UPDATE, DELETE
        
        [StringLength(100)]
        public string? PrimaryKeyValue { get; set; }
        
        public string? OldValues { get; set; } // JSON of old values
        
        public string? NewValues { get; set; } // JSON of new values
        
        public string? ChangedColumns { get; set; } // JSON array of changed column names
        
        public int? UserID { get; set; }
        
        [StringLength(100)]
        public string? UserName { get; set; }
        
        public DateTime AuditDate { get; set; } = DateTime.Now;
        
        [StringLength(50)]
        public string? IPAddress { get; set; }
        
        [StringLength(500)]
        public string? UserAgent { get; set; }
        
        [StringLength(200)]
        public string? RequestUrl { get; set; }
        
        [StringLength(100)]
        public string? SessionId { get; set; }
        
        [StringLength(100)]
        public string? CorrelationId { get; set; }
        
        [StringLength(500)]
        public string? Reason { get; set; } // Business reason for the change
        
        [StringLength(100)]
        public string? ApplicationName { get; set; }
        
        [StringLength(100)]
        public string? ApplicationVersion { get; set; }

        // Navigation properties
        public CustomUser? User { get; set; }
    }
}
