using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Data;
using HotelBooking.Models;
using HotelBooking.Models.ViewModels;

namespace HotelBooking.Controllers
{
    [Authorize(Roles = "Admin,Staff")]
    public class ReportController : Controller
    {
        private readonly HotelBookingContext _context;

        public ReportController(HotelBookingContext context)
        {
            _context = context;
        }

        // GET: Report
        public async Task<IActionResult> Index()
        {
            var viewModel = new ReportViewModel
            {
                FromDate = DateTime.Now.AddMonths(-1),
                ToDate = DateTime.Now,
                RoomTypes = await _context.RoomTypes.Where(rt => rt.IsActive).ToListAsync()
            };

            // Tổng quan doanh thu và booking
            var today = DateTime.Today;
            var thisMonth = new DateTime(today.Year, today.Month, 1);
            var lastMonth = thisMonth.AddMonths(-1);

            // Today
            var todayBookings = await _context.Reservations.CountAsync(r => r.Status == "Confirmed" && r.BookingDate.Date == today);
            var todayReservations = await _context.Reservations
                .Where(r => r.Status == "Confirmed" && r.BookingDate.Date == today)
                .Include(r => r.Payments)
                .ToListAsync();
            var todayRevenue = todayReservations.Sum(r => r.Payments?.Sum(p => (decimal?)p.Amount) ?? 0);

            // This month
            var thisMonthBookings = await _context.Reservations.CountAsync(r => r.Status == "Confirmed" && r.BookingDate >= thisMonth);
            var thisMonthReservations = await _context.Reservations
                .Where(r => r.Status == "Confirmed" && r.BookingDate >= thisMonth)
                .Include(r => r.Payments)
                .ToListAsync();
            var thisMonthRevenue = thisMonthReservations.Sum(r => r.Payments?.Sum(p => (decimal?)p.Amount) ?? 0);

            // Last month
            var lastMonthBookings = await _context.Reservations.CountAsync(r => r.Status == "Confirmed" && r.BookingDate >= lastMonth && r.BookingDate < thisMonth);
            var lastMonthReservations = await _context.Reservations
                .Where(r => r.Status == "Confirmed" && r.BookingDate >= lastMonth && r.BookingDate < thisMonth)
                .Include(r => r.Payments)
                .ToListAsync();
            var lastMonthRevenue = lastMonthReservations.Sum(r => r.Payments?.Sum(p => (decimal?)p.Amount) ?? 0);

            // Monthly revenue trend (12 tháng gần nhất)
            var monthlyReservations = await _context.Reservations
                .Where(r => r.Status == "Confirmed" && r.BookingDate >= today.AddMonths(-11))
                .Include(r => r.Payments)
                .ToListAsync();
            var monthlyRevenue = monthlyReservations
                .GroupBy(r => new { r.BookingDate.Year, r.BookingDate.Month })
                .Select(g => new MonthlyRevenueData
                {
                    month = g.Key.Year + "-" + g.Key.Month.ToString("D2"),
                    revenue = g.Sum(r => r.Payments?.Sum(p => (decimal?)p.Amount) ?? 0),
                    bookings = g.Count()
                })
                .OrderBy(g => g.month)
                .ToList();

            // Top customers (top 5)
            var topCustomerReservations = await _context.Reservations
                .Where(r => r.Status == "Confirmed")
                .Include(r => r.User)
                .Include(r => r.Payments)
                .ToListAsync();
            var topCustomers = topCustomerReservations
                .GroupBy(r => r.User?.UserName)
                .Select(g => new TopCustomerData
                {
                    CustomerName = g.Key ?? "Unknown",
                    TotalBookings = g.Count(),
                    TotalSpent = g.Sum(r => r.Payments?.Sum(p => (decimal?)p.Amount) ?? 0)
                })
                .OrderByDescending(c => c.TotalSpent)
                .Take(5)
                .ToList();

            // Room stats
            viewModel.TotalRooms = await _context.Rooms.CountAsync();
            viewModel.OccupiedRooms = await _context.Rooms.CountAsync(r => r.Status == "Occupied");

            // Payment stats
            viewModel.CompletedPayments = 0; // Không có property Status, nên không phân loại được
            viewModel.PendingPayments = 0;
            viewModel.TotalPayments = await _context.PaymentBatches.SumAsync(p => (decimal?)p.TotalAmount) ?? 0;

            // Gán các biến tổng quan
            viewModel.TodayRevenue = todayRevenue;
            viewModel.TodayBookings = todayBookings;
            viewModel.ThisMonthRevenue = thisMonthRevenue;
            viewModel.ThisMonthBookings = thisMonthBookings;
            viewModel.LastMonthRevenue = lastMonthRevenue;
            viewModel.LastMonthBookings = lastMonthBookings;
            viewModel.MonthlyRevenue = monthlyRevenue;
            viewModel.TopCustomers = topCustomers;

            return View(viewModel);
        }

        // POST: Report/Generate
        [HttpPost]
        public async Task<IActionResult> Generate(ReportViewModel model)
        {
            switch (model.ReportType)
            {
                case "booking":
                    return await GenerateBookingReport(model);
                case "payment":
                    return await GeneratePaymentReport(model);
                case "guest":
                    return await GenerateGuestReport(model);
                default:
                    return RedirectToAction(nameof(Index));
            }
        }

        private async Task<IActionResult> GenerateBookingReport(ReportViewModel model)
        {
            var query = _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Room!)
                    .ThenInclude(r => r.RoomType)
                .Include(r => r.Payments!)
                    .ThenInclude(p => p.PaymentBatch)
                .AsQueryable();

            // Apply filters
            if (model.FromDate.HasValue)
                query = query.Where(r => r.BookingDate >= model.FromDate.Value);
            if (model.ToDate.HasValue)
                query = query.Where(r => r.BookingDate <= model.ToDate.Value);
            if (model.RoomTypeID.HasValue)
                query = query.Where(r => r.Room!.RoomTypeID == model.RoomTypeID.Value);
            if (!string.IsNullOrEmpty(model.Status))
                query = query.Where(r => r.Status == model.Status);

            var reservations = await query.ToListAsync();

            // Calculate statistics
            var totalBookings = reservations.Count;
            var confirmedBookings = reservations.Count(r => r.Status == "Confirmed");
            var cancelledBookings = reservations.Count(r => r.Status == "Cancelled");
            var pendingBookings = reservations.Count(r => r.Status == "Pending");

            var totalRevenue = reservations
                .Where(r => r.Status == "Confirmed")
                .Sum(r => r.Payments?.Sum(p => p.Amount) ?? 0);

            var averageBookingValue = totalBookings > 0 ? totalRevenue / totalBookings : 0;

            // Calculate occupancy rate
            var totalRooms = await _context.Rooms.CountAsync();
            var totalDays = model.ToDate.HasValue && model.FromDate.HasValue 
                ? (model.ToDate.Value - model.FromDate.Value).Days 
                : 30;
            var totalRoomNights = totalRooms * totalDays;
            var bookedRoomNights = reservations
                .Where(r => r.Status == "Confirmed")
                .Sum(r => (r.CheckOutDate - r.CheckInDate).Days);
            var occupancyRate = totalRoomNights > 0 ? (double)bookedRoomNights / totalRoomNights * 100 : 0;

            // Booking data
            var bookingData = reservations.Select(r => new BookingReportData
            {
                ReservationID = r.ReservationID,
                GuestName = r.User?.UserName ?? "Unknown",
                RoomNumber = r.Room?.RoomNumber ?? "Unknown",
                RoomType = r.Room?.RoomType?.TypeName ?? "Unknown",
                BookingDate = r.BookingDate,
                CheckInDate = r.CheckInDate,
                CheckOutDate = r.CheckOutDate,
                NumberOfGuests = r.NumberOfGuests,
                Status = r.Status ?? "Unknown",
                TotalAmount = CalculateTotalAmount(r),
                PaidAmount = r.Payments?.Sum(p => p.Amount) ?? 0
            }).ToList();

            // Revenue data by day
            var revenueData = reservations
                .Where(r => r.Status == "Confirmed" && r.Payments != null)
                .SelectMany(r => r.Payments!)
                .GroupBy(p => p.PaymentBatch!.PaymentDate.Date)
                .Select(g => new RevenueReportData
                {
                    Date = g.Key,
                    DailyRevenue = g.Sum(p => p.Amount),
                    BookingsCount = g.Count(),
                    AverageBookingValue = g.Average(p => p.Amount)
                })
                .OrderBy(r => r.Date)
                .ToList();

            // Room type data
            var roomTypeData = reservations
                .Where(r => r.Status == "Confirmed")
                .GroupBy(r => r.Room!.RoomType!.TypeName)
                .Select(g => new RoomTypeReportData
                {
                    RoomTypeName = g.Key,
                    TotalBookings = g.Count(),
                    TotalRevenue = g.Sum(r => r.Payments?.Sum(p => p.Amount) ?? 0),
                    OccupancyRate = 0, // Calculate based on room type capacity
                    AverageRate = g.Average(r => r.Room!.Price)
                })
                .ToList();

            // Monthly data
            var monthlyData = reservations
                .Where(r => r.Status == "Confirmed")
                .GroupBy(r => new { r.BookingDate.Year, r.BookingDate.Month })
                .Select(g => new MonthlyReportData
                {
                    Month = $"{g.Key.Year}-{g.Key.Month:D2}",
                    Year = g.Key.Year,
                    TotalBookings = g.Count(),
                    TotalRevenue = g.Sum(r => r.Payments?.Sum(p => p.Amount) ?? 0),
                    OccupancyRate = 0, // Calculate based on monthly capacity
                    NewGuests = g.Select(r => r.UserID).Distinct().Count()
                })
                .OrderBy(m => m.Month)
                .ToList();

            var reportViewModel = new BookingReportViewModel
            {
                TotalBookings = totalBookings,
                ConfirmedBookings = confirmedBookings,
                CancelledBookings = cancelledBookings,
                PendingBookings = pendingBookings,
                TotalRevenue = totalRevenue,
                AverageBookingValue = averageBookingValue,
                OccupancyRate = occupancyRate,
                BookingData = bookingData,
                RevenueData = revenueData,
                RoomTypeData = roomTypeData,
                MonthlyData = monthlyData
            };

            return View("BookingReport", reportViewModel);
        }

        private async Task<IActionResult> GeneratePaymentReport(ReportViewModel model)
        {
            var query = _context.PaymentBatches
                .Include(pb => pb.User)
                .AsQueryable();

            // Apply filters
            if (model.FromDate.HasValue)
                query = query.Where(pb => pb.PaymentDate >= model.FromDate.Value);
            if (model.ToDate.HasValue)
                query = query.Where(pb => pb.PaymentDate <= model.ToDate.Value);

            var paymentBatches = await query.ToListAsync();

            var totalPayments = paymentBatches.Sum(pb => pb.TotalAmount);
            var totalRefunds = await _context.Refunds
                .Where(r => model.FromDate == null || r.RefundDate >= model.FromDate.Value)
                .Where(r => model.ToDate == null || r.RefundDate <= model.ToDate.Value)
                .SumAsync(r => r.RefundAmount);

            var netRevenue = totalPayments - totalRefunds;
            var totalTransactions = paymentBatches.Count;

            // Payment method breakdown
            var paymentMethodBreakdown = paymentBatches
                .GroupBy(pb => pb.PaymentMethod ?? "Unknown")
                .ToDictionary(g => g.Key, g => g.Sum(pb => pb.TotalAmount));

            // Payment data
            var paymentData = new List<PaymentReportData>();
            foreach (var batch in paymentBatches)
            {
                var payments = await _context.Payments
                    .Include(p => p.Reservation!)
                        .ThenInclude(r => r.User)
                    .Include(p => p.Reservation!)
                        .ThenInclude(r => r.Room)
                    .Where(p => p.PaymentBatchID == batch.PaymentBatchID)
                    .ToListAsync();

                foreach (var payment in payments)
                {
                    paymentData.Add(new PaymentReportData
                    {
                        PaymentBatchID = batch.PaymentBatchID,
                        PaymentID = payment.PaymentID,
                        GuestName = payment.Reservation?.User?.UserName ?? "Unknown",
                        RoomNumber = payment.Reservation?.Room?.RoomNumber ?? "Unknown",
                        PaymentDate = batch.PaymentDate,
                        Amount = payment.Amount,
                        PaymentMethod = batch.PaymentMethod ?? "Unknown",
                        Status = "Completed"
                    });
                }
            }

            // Payment method data
            var paymentMethodData = paymentBatches
                .GroupBy(pb => pb.PaymentMethod ?? "Unknown")
                .Select(g => new PaymentMethodData
                {
                    PaymentMethod = g.Key,
                    TransactionCount = g.Count(),
                    TotalAmount = g.Sum(pb => pb.TotalAmount),
                    AverageAmount = g.Average(pb => pb.TotalAmount),
                    Percentage = totalPayments > 0 ? (double)(g.Sum(pb => pb.TotalAmount) / totalPayments) * 100 : 0
                }).ToList();

            // Recent transactions
            var recentTransactions = paymentData.OrderByDescending(p => p.PaymentDate).Take(10).ToList();

            var reportViewModel = new PaymentReportViewModel
            {
                TotalRevenue = totalPayments,
                TotalPayments = totalPayments,
                TotalRefunds = totalRefunds,
                NetRevenue = netRevenue,
                TotalTransactions = totalTransactions,
                AverageTransactionValue = totalTransactions > 0 ? totalPayments / totalTransactions : 0,
                PendingPayments = 0, // Calculate if needed
                PaymentMethodData = paymentMethodData,
                RecentTransactions = recentTransactions,
                MonthlyData = new List<MonthlyPaymentData>(), // Add implementation if needed
                DailyData = new List<DailyPaymentData>() // Add implementation if needed
            };

            return View("PaymentReport", reportViewModel);
        }

        private async Task<IActionResult> GenerateGuestReport(ReportViewModel model)
        {
            var query = _context.Guests
                .Include(g => g.User)
                .Include(g => g.Country)
                .Include(g => g.ReservationGuests!)
                    .ThenInclude(rg => rg.Reservation!)
                    .ThenInclude(r => r.Payments!)
                    .ThenInclude(p => p.PaymentBatch)
                .AsQueryable();

            // Apply date filter based on guest creation date
            if (model.FromDate.HasValue)
                query = query.Where(g => g.CreatedDate >= model.FromDate.Value);
            if (model.ToDate.HasValue)
                query = query.Where(g => g.CreatedDate <= model.ToDate.Value);

            var guests = await query.ToListAsync();

            var totalGuests = guests.Count;
            var newGuestsThisPeriod = guests.Count;

            // Calculate returning guests (guests with more than one reservation)
            var returningGuests = guests.Count(g => 
                g.ReservationGuests != null && g.ReservationGuests.Count > 1);

            // Guest data
            var guestData = guests.Select(g => new GuestReportData
            {
                GuestName = $"{g.FirstName} {g.LastName}",
                Email = g.Email ?? "Unknown",
                Country = g.Country?.CountryName ?? "Unknown",
                TotalBookings = g.ReservationGuests?.Count ?? 0,
                TotalSpent = g.ReservationGuests?
                    .SelectMany(rg => rg.Reservation?.Payments ?? new List<Payment>())
                    .Sum(p => p.Amount) ?? 0,
                LastBooking = g.ReservationGuests?
                    .Max(rg => rg.Reservation?.BookingDate) ?? DateTime.MinValue,
                FirstBooking = g.ReservationGuests?
                    .Min(rg => rg.Reservation?.BookingDate) ?? DateTime.MinValue
            }).ToList();

            // Country distribution
            var countryDistribution = guests
                .GroupBy(g => g.Country?.CountryName ?? "Unknown")
                .ToDictionary(g => g.Key, g => g.Count());

            // Age group distribution
            var ageGroupDistribution = guests
                .GroupBy(g => g.AgeGroup ?? "Unknown")
                .ToDictionary(g => g.Key, g => g.Count());

            // Age group data
            var ageGroupData = guests
                .GroupBy(g => g.AgeGroup ?? "Unknown")
                .Select(g => new AgeGroupData
                {
                    AgeGroup = g.Key,
                    GuestCount = g.Count(),
                    Percentage = totalGuests > 0 ? (double)g.Count() / totalGuests * 100 : 0,
                    AverageBookings = g.Average(guest => guest.ReservationGuests?.Count ?? 0)
                }).ToList();

            // Country data
            var countryData = guests
                .GroupBy(g => g.Country?.CountryName ?? "Unknown")
                .Select(g => new CountryData
                {
                    CountryName = g.Key,
                    GuestCount = g.Count(),
                    Percentage = totalGuests > 0 ? (double)g.Count() / totalGuests * 100 : 0,
                    TotalRevenue = g.Sum(guest => guest.ReservationGuests?
                        .SelectMany(rg => rg.Reservation?.Payments ?? new List<Payment>())
                        .Sum(p => p.Amount) ?? 0)
                }).ToList();

            // Loyalty data (simplified)
            var loyaltyData = new List<LoyaltyData>
            {
                new LoyaltyData { Segment = "VIP", GuestCount = guests.Count(g => (g.ReservationGuests?.Count ?? 0) >= 5), AverageBookings = 5, AverageSpend = 1000, TotalRevenue = 5000, Percentage = 10 },
                new LoyaltyData { Segment = "Loyal", GuestCount = guests.Count(g => (g.ReservationGuests?.Count ?? 0) >= 3), AverageBookings = 3, AverageSpend = 500, TotalRevenue = 1500, Percentage = 20 },
                new LoyaltyData { Segment = "Regular", GuestCount = guests.Count(g => (g.ReservationGuests?.Count ?? 0) >= 1), AverageBookings = 1, AverageSpend = 200, TotalRevenue = 200, Percentage = 70 }
            };

            var reportViewModel = new GuestReportViewModel
            {
                TotalGuests = totalGuests,
                NewGuests = newGuestsThisPeriod,
                ReturningGuests = returningGuests,
                AverageStayDuration = 2.5, // Calculate if needed
                GuestData = guestData,
                AgeGroupData = ageGroupData,
                CountryData = countryData,
                LoyaltyData = loyaltyData,
                MonthlyData = new List<MonthlyGuestData>() // Add implementation if needed
            };

            return View("GuestReport", reportViewModel);
        }

        private decimal CalculateTotalAmount(Reservation reservation)
        {
            if (reservation.Room?.Price == null) return 0;
            
            var nights = (reservation.CheckOutDate - reservation.CheckInDate).Days;
            return reservation.Room.Price * nights;
        }
    }
}
