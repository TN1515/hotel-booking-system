using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Models
{
    public class QRPayment
    {
        [Key]
        public int QRPaymentID { get; set; }
        
        [Required]
        public int ReservationID { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }
        
        [Required]
        [StringLength(100)]
        public string? BankCode { get; set; } // VietinBank
        
        [Required]
        [StringLength(50)]
        public string? AccountNumber { get; set; } // 1038766815877
        
        [Required]
        [StringLength(100)]
        public string? AccountName { get; set; } // LUU VAN HIEN
        
        [Required]
        [StringLength(500)]
        public string? QRCodeData { get; set; } // QR code content
        
        [StringLength(200)]
        public string? TransactionDescription { get; set; }
        
        [Required]
        public DateTime CreatedDate { get; set; }
        
        public DateTime? PaidDate { get; set; }
        
        [Required]
        [StringLength(20)]
        public string? Status { get; set; } // Pending, Paid, Expired, Cancelled
        
        [StringLength(100)]
        public string? TransactionReference { get; set; }
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        public int CreatedByUserID { get; set; }
        
        // Navigation properties
        public virtual Reservation? Reservation { get; set; }
        public virtual CustomUser? CreatedByUser { get; set; }
    }
}
