using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models.ViewModels
{
    public class NotificationViewModel
    {
        public int NotificationID { get; set; }
        public int UserID { get; set; }

        [Required]
        [StringLength(200)]
        public string? Title { get; set; }

        [Required]
        [StringLength(1000)]
        public string? Message { get; set; }

        [Required]
        [Display(Name = "Notification Type")]
        public string? Type { get; set; }

        public string? Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? SentDate { get; set; }
        public bool IsRead { get; set; }

        // User details
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
    }

    public class SendNotificationViewModel
    {
        [Required]
        [StringLength(200)]
        public string? Title { get; set; }

        [Required]
        [StringLength(1000)]
        public string? Message { get; set; }

        [Required]
        [Display(Name = "Notification Type")]
        public string? Type { get; set; }

        [Display(Name = "Send To")]
        public string? SendTo { get; set; } // All, Specific, Role

        [Display(Name = "Specific Users")]
        public List<int>? UserIDs { get; set; }

        [Display(Name = "Role")]
        public int? RoleID { get; set; }

        [Display(Name = "Role Name")]
        public string? RoleName { get; set; }

        [Display(Name = "Send Immediately")]
        public bool SendImmediately { get; set; } = true;

        [Display(Name = "Schedule Date")]
        public DateTime? ScheduleDate { get; set; }

        // For dropdowns
        public List<CustomUser>? Users { get; set; }
        public List<CustomRole>? Roles { get; set; }
    }

    public class NotificationListViewModel
    {
        public List<NotificationViewModel>? Notifications { get; set; }
        public int TotalNotifications { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string? SearchTerm { get; set; }
        public string? TypeFilter { get; set; }
        public string? StatusFilter { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class BulkNotificationViewModel
    {
        [Required]
        [StringLength(200)]
        public string? Title { get; set; }

        [Required]
        [StringLength(1000)]
        public string? Message { get; set; }

        [Required]
        [Display(Name = "Notification Type")]
        public string? Type { get; set; }

        [Display(Name = "Notification Type")]
        public string? NotificationType { get; set; }

        [Display(Name = "Priority")]
        public string? Priority { get; set; }

        [Required]
        [Display(Name = "Target Audience")]
        public string? TargetAudience { get; set; } // AllUsers, AllGuests, AllStaff, SpecificRole

        [Display(Name = "Specific Role")]
        public int? RoleID { get; set; }

        [Display(Name = "Selected Role IDs")]
        public List<int>? SelectedRoleIds { get; set; }

        [Display(Name = "Include Inactive Users")]
        public bool IncludeInactiveUsers { get; set; }

        [Display(Name = "Send Email")]
        public bool SendEmail { get; set; }

        [Display(Name = "Send SMS")]
        public bool SendSMS { get; set; }

        // For dropdown
        public List<CustomRole>? Roles { get; set; }
    }

    public class NotificationTemplateViewModel
    {
        public string? TemplateName { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; }
        public string? Type { get; set; }
        public Dictionary<string, string>? Variables { get; set; }
    }

    public class MyNotificationsViewModel
    {
        public List<NotificationViewModel>? Notifications { get; set; }
        public int TotalNotifications { get; set; }
        public int UnreadCount { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
