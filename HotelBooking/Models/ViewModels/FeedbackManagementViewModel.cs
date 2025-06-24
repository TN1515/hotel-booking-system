using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models.ViewModels
{
    public class FeedbackManagementViewModel
    {
        public int FeedbackID { get; set; }
        public int ReservationID { get; set; }
        public int GuestID { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime FeedbackDate { get; set; }

        // Guest details
        public string? GuestName { get; set; }
        public string? GuestEmail { get; set; }

        // Reservation details
        public string? RoomNumber { get; set; }
        public string? RoomType { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }

        // Response details
        public string? AdminResponse { get; set; }
        public DateTime? ResponseDate { get; set; }
        public string? ResponseBy { get; set; }
    }

    public class FeedbackListViewModel
    {
        public List<FeedbackManagementViewModel>? Feedbacks { get; set; }
        public int TotalFeedbacks { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string? SearchTerm { get; set; }
        public int? RatingFilter { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public double AverageRating { get; set; }
        public Dictionary<int, int>? RatingDistribution { get; set; }
    }



    public class FeedbackStatisticsViewModel
    {
        public double AverageRating { get; set; }
        public int TotalFeedbacks { get; set; }
        public Dictionary<int, int>? RatingDistribution { get; set; }
        public List<FeedbackTrendData>? MonthlyTrends { get; set; }
        public List<FeedbackManagementViewModel>? RecentFeedbacks { get; set; }
        public List<FeedbackManagementViewModel>? TopRatedExperiences { get; set; }
        public List<FeedbackManagementViewModel>? LowRatedExperiences { get; set; }
    }

    public class FeedbackTrendData
    {
        public string? Month { get; set; }
        public double AverageRating { get; set; }
        public int FeedbackCount { get; set; }
    }


}
