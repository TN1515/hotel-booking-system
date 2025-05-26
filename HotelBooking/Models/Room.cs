using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models
{
    public class Room
    {
        public int RoomID { get; set; }
        [StringLength(10)]
        public string? RoomNumber { get; set; }
        public int RoomTypeID { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }
        [StringLength(50)]
        public string? BedType { get; set; }
        [StringLength(50)]
        public string? ViewType { get; set; }
        public string? Status { get; set; }
        public bool IsActive { get; set; }
        [StringLength(100)]
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        [StringLength(100)]
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public RoomType? RoomType { get; set; }
    }
}
