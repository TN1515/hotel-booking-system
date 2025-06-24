using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models
{
    public class PaymentMethod
    {
        public int PaymentMethodID { get; set; }
        
        [StringLength(100)]
        public string? MethodName { get; set; }
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        [StringLength(50)]
        public string? MethodType { get; set; } // Cash, Card, Bank Transfer, QR Code, E-Wallet
        
        [StringLength(100)]
        public string? Provider { get; set; } // VietinBank, Vietcombank, MoMo, ZaloPay, etc.
        
        [StringLength(50)]
        public string? Icon { get; set; }
        
        public bool RequiresVerification { get; set; } = false;
        
        public bool IsOnline { get; set; } = true;
        
        public bool IsActive { get; set; } = true;
        
        public int DisplayOrder { get; set; } = 0;
        
        [StringLength(500)]
        public string? Instructions { get; set; }
        
        [StringLength(100)]
        public string? CreatedBy { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        [StringLength(100)]
        public string? ModifiedBy { get; set; }
        
        public DateTime? ModifiedDate { get; set; }

        // Navigation properties
        public ICollection<Payment>? Payments { get; set; }
    }
}
