using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models.ViewModels
{
    public class FeedbackResponseViewModel
    {
        public int FeedbackID { get; set; }
        
        [Required]
        [Display(Name = "Response")]
        [StringLength(1000, MinimumLength = 10)]
        public string Response { get; set; } = string.Empty;
        
        [Display(Name = "Response Category")]
        public string? ResponseCategory { get; set; }
        
        [Display(Name = "Priority Level")]
        public string Priority { get; set; } = "Medium";
        
        [Display(Name = "Requires Follow-up")]
        public bool RequiresFollowUp { get; set; }
        
        [Display(Name = "Follow-up Date")]
        [DataType(DataType.DateTime)]
        public DateTime? FollowUpDate { get; set; }
        
        [Display(Name = "Internal Notes")]
        [StringLength(500)]
        public string? InternalNotes { get; set; }
        
        // Original feedback details for display
        public string CustomerName { get; set; } = string.Empty;
        public string RoomNumber { get; set; } = string.Empty;
        public string RoomType { get; set; } = string.Empty;
        public DateTime FeedbackDate { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
    }
}
