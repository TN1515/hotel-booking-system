using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models
{
    public class Setting
    {
        public int SettingID { get; set; }
        
        [StringLength(100)]
        public string? SettingKey { get; set; }
        
        public string? SettingValue { get; set; }
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        [StringLength(50)]
        public string? Category { get; set; } // System, Hotel, Payment, Notification, etc.
        
        [StringLength(50)]
        public string? DataType { get; set; } // String, Integer, Boolean, Decimal, JSON
        
        public bool IsEncrypted { get; set; } = false;
        
        public bool IsReadOnly { get; set; } = false;
        
        public bool RequiresRestart { get; set; } = false;
        
        [StringLength(500)]
        public string? ValidationRules { get; set; } // JSON validation rules
        
        [StringLength(500)]
        public string? DefaultValue { get; set; }
        
        public int DisplayOrder { get; set; } = 0;
        
        public bool IsActive { get; set; } = true;
        
        [StringLength(100)]
        public string? CreatedBy { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        [StringLength(100)]
        public string? ModifiedBy { get; set; }
        
        public DateTime? ModifiedDate { get; set; }
        
        public DateTime? LastAccessDate { get; set; }
        
        [StringLength(100)]
        public string? LastAccessedBy { get; set; }
    }
}
