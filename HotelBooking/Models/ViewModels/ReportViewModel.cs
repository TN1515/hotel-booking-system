using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models.ViewModels
{
    public class ReportViewModel
    {
        [Display(Name = "Report Type")]
        public string? ReportType { get; set; }
        
        [Display(Name = "From Date")]
        public DateTime? FromDate { get; set; }
        
        [Display(Name = "To Date")]
        public DateTime? ToDate { get; set; }
        
        [Display(Name = "Room Type")]
        public int? RoomTypeID { get; set; }
        
        [Display(Name = "Status")]
        public string? Status { get; set; }
        
        public List<RoomType>? RoomTypes { get; set; }
        public int TotalRooms { get; set; }
        public int OccupiedRooms { get; set; }
        public List<MonthlyRevenueData> MonthlyRevenue { get; set; } = new();
        public List<TopCustomerData> TopCustomers { get; set; } = new();
        public decimal TodayRevenue { get; set; }
        public decimal ThisMonthRevenue { get; set; }
        public decimal LastMonthRevenue { get; set; }
        public int TodayBookings { get; set; }
        public int ThisMonthBookings { get; set; }
        public int LastMonthBookings { get; set; }
        public decimal CompletedPayments { get; set; }
        public decimal PendingPayments { get; set; }
        public decimal TotalPayments { get; set; }
    }

    public class BookingReportViewModel
    {
        public int TotalBookings { get; set; }
        public int ConfirmedBookings { get; set; }
        public int CancelledBookings { get; set; }
        public int PendingBookings { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal AverageBookingValue { get; set; }
        public double OccupancyRate { get; set; }
        
        public List<BookingReportData>? BookingData { get; set; }
        public List<RevenueReportData>? RevenueData { get; set; }
        public List<RoomTypeReportData>? RoomTypeData { get; set; }
        public List<MonthlyReportData>? MonthlyData { get; set; }
    }

    public class BookingReportData
    {
        public int ReservationID { get; set; }
        public string? GuestName { get; set; }
        public string? RoomNumber { get; set; }
        public string? RoomType { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfGuests { get; set; }
        public string? Status { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
    }

    public class RevenueReportData
    {
        public DateTime Date { get; set; }
        public decimal DailyRevenue { get; set; }
        public int BookingsCount { get; set; }
        public decimal AverageBookingValue { get; set; }
    }

    public class RoomTypeReportData
    {
        public string? RoomTypeName { get; set; }
        public int TotalBookings { get; set; }
        public int BookingCount { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal Revenue { get; set; }
        public double OccupancyRate { get; set; }
        public decimal AverageRate { get; set; }
    }

    public class MonthlyReportData
    {
        public string? Month { get; set; }
        public int Year { get; set; }
        public int TotalBookings { get; set; }
        public decimal TotalRevenue { get; set; }
        public double OccupancyRate { get; set; }
        public int NewGuests { get; set; }
    }

    public class PaymentReportViewModel
    {
        public decimal TotalRevenue { get; set; }
        public decimal TotalPayments { get; set; }
        public decimal TotalRefunds { get; set; }
        public decimal NetRevenue { get; set; }
        public int TotalTransactions { get; set; }
        public decimal AverageTransactionValue { get; set; }
        public decimal PendingPayments { get; set; }

        public List<PaymentMethodData>? PaymentMethodData { get; set; }
        public List<PaymentReportData>? RecentTransactions { get; set; }
        public List<MonthlyPaymentData>? MonthlyData { get; set; }
        public List<DailyPaymentData>? DailyData { get; set; }
    }

    public class PaymentMethodData
    {
        public string? PaymentMethod { get; set; }
        public int TransactionCount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal AverageAmount { get; set; }
        public double Percentage { get; set; }
    }

    public class MonthlyPaymentData
    {
        public string? Month { get; set; }
        public decimal Revenue { get; set; }
        public int TransactionCount { get; set; }
    }

    public class DailyPaymentData
    {
        public string? Date { get; set; }
        public decimal Revenue { get; set; }
        public int TransactionCount { get; set; }
    }

    public class PaymentReportData
    {
        public int PaymentBatchID { get; set; }
        public int PaymentID { get; set; }
        public string? GuestName { get; set; }
        public string? RoomNumber { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Status { get; set; }
    }

    public class GuestReportViewModel
    {
        public int TotalGuests { get; set; }
        public int NewGuests { get; set; }
        public int ReturningGuests { get; set; }
        public double AverageStayDuration { get; set; }

        public List<GuestReportData>? GuestData { get; set; }
        public List<AgeGroupData>? AgeGroupData { get; set; }
        public List<CountryData>? CountryData { get; set; }
        public List<LoyaltyData>? LoyaltyData { get; set; }
        public List<MonthlyGuestData>? MonthlyData { get; set; }
    }

    public class AgeGroupData
    {
        public string? AgeGroup { get; set; }
        public int GuestCount { get; set; }
        public double Percentage { get; set; }
        public double AverageBookings { get; set; }
    }

    public class CountryData
    {
        public string? CountryName { get; set; }
        public int GuestCount { get; set; }
        public double Percentage { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class LoyaltyData
    {
        public string? Segment { get; set; }
        public int GuestCount { get; set; }
        public double AverageBookings { get; set; }
        public decimal AverageSpend { get; set; }
        public decimal TotalRevenue { get; set; }
        public double Percentage { get; set; }
    }

    public class MonthlyGuestData
    {
        public string? Month { get; set; }
        public int NewGuests { get; set; }
        public int ReturningGuests { get; set; }
        public int TotalGuests { get; set; }
    }

    public class GuestReportData
    {
        public string? GuestName { get; set; }
        public string? Email { get; set; }
        public string? Country { get; set; }
        public int TotalBookings { get; set; }
        public decimal TotalSpent { get; set; }
        public DateTime LastBooking { get; set; }
        public DateTime FirstBooking { get; set; }
    }

    public class MonthlyRevenueData
    {
        public string month { get; set; }
        public decimal revenue { get; set; }
        public int bookings { get; set; }
    }

    public class TopCustomerData
    {
        public string CustomerName { get; set; }
        public int TotalBookings { get; set; }
        public decimal TotalSpent { get; set; }
    }
}
