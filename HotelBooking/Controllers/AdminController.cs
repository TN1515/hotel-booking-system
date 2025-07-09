using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Data;
using HotelBooking.Models;
using Microsoft.AspNetCore.Authorization;

namespace HotelBooking.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly HotelBookingContext _context;

        public AdminController(HotelBookingContext context)
        {
            _context = context;
        }

        // GET: Admin/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var dashboardData = new AdminDashboardViewModel
            {
                TotalReservations = await _context.Reservations.CountAsync(),
                ConfirmedReservations = await _context.Reservations.CountAsync(r => r.Status == "Confirmed"),
                PendingReservations = await _context.Reservations.CountAsync(r => r.Status == "Pending"),
                TotalRevenue = await CalculateRevenue(DateTime.Now.AddMonths(-12), DateTime.Now),
                RecentReservations = await _context.Reservations
                    .Include(r => r.User)
                    .Include(r => r.Room)
                    .ThenInclude(room => room!.RoomType)
                    .OrderByDescending(r => r.CreatedDate)
                    .Take(10)
                    .ToListAsync()
            };

            return View(dashboardData);
        }

        // GET: Admin/Reservations
        public async Task<IActionResult> Reservations(string? status, string? search)
        {
            var query = _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Room)
                .ThenInclude(room => room!.RoomType)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status) && status != "All")
            {
                query = query.Where(r => r.Status == status);
            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(r =>
                    r.User!.UserName!.Contains(search) ||
                    r.User.Email!.Contains(search) ||
                    r.Room!.RoomNumber!.Contains(search));
            }

            var reservations = await query
                .OrderByDescending(r => r.CreatedDate)
                .ToListAsync();

            ViewBag.Status = status;
            ViewBag.Search = search;
            ViewBag.TotalReservations = await _context.Reservations.CountAsync();
            ViewBag.ConfirmedReservations = await _context.Reservations.CountAsync(r => r.Status == "Confirmed");
            ViewBag.PendingReservations = await _context.Reservations.CountAsync(r => r.Status == "Pending");
            ViewBag.TotalRevenue = await CalculateRevenue(DateTime.Now.AddMonths(-12), DateTime.Now);

            return View(reservations);
        }

        // POST: Admin/UpdateReservationStatus
        [HttpPost]
        public async Task<IActionResult> UpdateReservationStatus(int id, string status)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            reservation.Status = status;
            reservation.ModifiedDate = DateTime.Now;
            reservation.ModifiedBy = User.Identity?.Name ?? "Admin";

            await _context.SaveChangesAsync();

            TempData["Message"] = $"Reservation status updated to {status} successfully.";
            return RedirectToAction("Reservations");
        }

        // GET: Admin/ReservationDetails/5
        public async Task<IActionResult> ReservationDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Room)
                .ThenInclude(room => room!.RoomType)
                .Include(r => r.Payments)
                .FirstOrDefaultAsync(m => m.ReservationID == id);

            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Admin/Profile
        public async Task<IActionResult> Profile()
        {
            var currentUser = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == User.Identity!.Name);

            if (currentUser == null)
            {
                return NotFound();
            }

            return View(currentUser);
        }

        // GET: Admin/Reports
        public async Task<IActionResult> Reports()
        {
            var today = DateTime.Today;
            var thisMonth = new DateTime(today.Year, today.Month, 1);
            var lastMonth = thisMonth.AddMonths(-1);

            var reportData = new
            {
                // Revenue Reports
                TodayRevenue = await CalculateRevenue(today, today.AddDays(1)),
                ThisMonthRevenue = await CalculateRevenue(thisMonth, thisMonth.AddMonths(1)),
                LastMonthRevenue = await CalculateRevenue(lastMonth, thisMonth),

                // Booking Reports
                TodayBookings = await _context.Reservations.CountAsync(r => r.CreatedDate.Date == today),
                ThisMonthBookings = await _context.Reservations.CountAsync(r => r.CreatedDate >= thisMonth),
                LastMonthBookings = await _context.Reservations.CountAsync(r => r.CreatedDate >= lastMonth && r.CreatedDate < thisMonth),

                // Room Occupancy
                TotalRooms = await _context.Rooms.CountAsync(),
                OccupiedRooms = await _context.Reservations.CountAsync(r =>
                    r.Status == "Confirmed" &&
                    r.CheckInDate <= today &&
                    r.CheckOutDate > today),

                // Payment Reports
                TotalPayments = await _context.Payments.SumAsync(p => p.Amount),
                PendingPayments = await _context.QRPayments.Where(q => q.Status == "Pending").SumAsync(q => q.Amount),
                CompletedPayments = await _context.QRPayments.Where(q => q.Status == "Paid").SumAsync(q => q.Amount),

                // Top Customers
                TopCustomers = await GetTopCustomersData(),

                // Monthly Revenue Chart Data
                MonthlyRevenue = await GetMonthlyRevenueData()
            };

            return View(reportData);
        }

        // GET: Admin/SystemStats
        public async Task<IActionResult> SystemStats()
        {
            var stats = new SystemStatsViewModel
            {
                TotalUsers = await _context.Users.CountAsync(),
                ActiveUsers = await _context.Users.CountAsync(u => u.IsActive),
                TotalRooms = await _context.Rooms.CountAsync(),
                AvailableRooms = await _context.Rooms.CountAsync(r => r.Status == "Available"),
                TotalReservations = await _context.Reservations.CountAsync(),
                TodayReservations = await _context.Reservations.CountAsync(r => r.CreatedDate.Date == DateTime.Today),
                TotalRevenue = await CalculateRevenue(DateTime.Now.AddMonths(-12), DateTime.Now),
                MonthlyRevenue = await CalculateRevenue(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), DateTime.Now),
                TotalServices = await _context.Services.CountAsync(),
                ActiveServices = await _context.Services.CountAsync(s => s.IsActive),
                TotalNotifications = await _context.Notifications.CountAsync(),
                UnreadNotifications = await _context.Notifications.CountAsync(n => !n.IsRead),
                TotalPayments = await _context.QRPayments.SumAsync(p => p.Amount),
                PendingPayments = await _context.QRPayments.Where(p => p.Status == "Pending").SumAsync(p => p.Amount),
                CompletedPayments = await _context.QRPayments.Where(p => p.Status == "Paid").SumAsync(p => p.Amount)
            };

            return View(stats);
        }

        // GET: Admin/SystemStatsApi - For AJAX calls
        public async Task<IActionResult> SystemStatsApi()
        {
            var stats = new
            {
                TotalUsers = await _context.Users.CountAsync(),
                ActiveUsers = await _context.Users.CountAsync(u => u.IsActive),
                TotalRooms = await _context.Rooms.CountAsync(),
                AvailableRooms = await _context.Rooms.CountAsync(r => r.Status == "Available"),
                TotalReservations = await _context.Reservations.CountAsync(),
                TodayReservations = await _context.Reservations.CountAsync(r => r.CreatedDate.Date == DateTime.Today),
                TotalRevenue = await CalculateRevenue(DateTime.Now.AddMonths(-12), DateTime.Now),
                MonthlyRevenue = await CalculateRevenue(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), DateTime.Now)
            };

            return Json(stats);
        }

        // POST: Admin/DeleteReservation/5
        [HttpPost]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Reservation deleted successfully.";
            return RedirectToAction("Reservations");
        }

        // Helper method to calculate revenue for a date range
        // GET: Admin/GetSystemLogs
        public async Task<IActionResult> GetSystemLogs(string? level, string? category, DateTime? fromDate, DateTime? toDate, string? searchTerm, int page = 1)
        {
            // Mock data for system logs since we don't have a SystemLogs table
            var logs = new List<object>
            {
                new { id = 1, timestamp = DateTime.Now.AddHours(-1), level = "Info", category = "Authentication", message = "User logged in successfully", user = "admin@hotel.com", ipAddress = "192.168.1.1" },
                new { id = 2, timestamp = DateTime.Now.AddHours(-2), level = "Warning", category = "Booking", message = "Room availability check failed", user = "System", ipAddress = "127.0.0.1" },
                new { id = 3, timestamp = DateTime.Now.AddHours(-3), level = "Error", category = "Payment", message = "Payment processing failed", user = "customer@hotel.com", ipAddress = "192.168.1.2" },
                new { id = 4, timestamp = DateTime.Now.AddHours(-4), level = "Info", category = "Database", message = "Database backup completed", user = "System", ipAddress = "127.0.0.1" },
                new { id = 5, timestamp = DateTime.Now.AddHours(-5), level = "Critical", category = "System", message = "High memory usage detected", user = "System", ipAddress = "127.0.0.1" }
            };

            var statistics = new
            {
                infoCount = logs.Count(l => l.GetType().GetProperty("level")?.GetValue(l)?.ToString() == "Info"),
                warningCount = logs.Count(l => l.GetType().GetProperty("level")?.GetValue(l)?.ToString() == "Warning"),
                errorCount = logs.Count(l => l.GetType().GetProperty("level")?.GetValue(l)?.ToString() == "Error"),
                criticalCount = logs.Count(l => l.GetType().GetProperty("level")?.GetValue(l)?.ToString() == "Critical"),
                totalCount = logs.Count
            };

            var pagination = new
            {
                currentPage = page,
                totalPages = 1,
                totalItems = logs.Count
            };

            return Json(new { logs, statistics, pagination });
        }

        // GET: Admin/GetLogDetails
        public async Task<IActionResult> GetLogDetails(int id)
        {
            // Mock log details
            var logDetail = new
            {
                id = id,
                timestamp = DateTime.Now.AddHours(-id),
                level = "Info",
                category = "System",
                message = $"Detailed log message for log ID {id}",
                user = "admin@hotel.com",
                ipAddress = "192.168.1.1",
                userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
                exception = id == 3 ? "System.Exception: Sample exception details" : null
            };

            return Json(logDetail);
        }

        // GET: Admin/ExportLogs
        public async Task<IActionResult> ExportLogs(string? level, string? category, DateTime? fromDate, DateTime? toDate, string? searchTerm)
        {
            // Mock export functionality
            var csvContent = "Timestamp,Level,Category,Message,User,IP Address\n";
            csvContent += $"{DateTime.Now:yyyy-MM-dd HH:mm:ss},Info,Authentication,User logged in successfully,admin@hotel.com,192.168.1.1\n";
            csvContent += $"{DateTime.Now.AddHours(-1):yyyy-MM-dd HH:mm:ss},Warning,Booking,Room availability check failed,System,127.0.0.1\n";
            csvContent += $"{DateTime.Now.AddHours(-2):yyyy-MM-dd HH:mm:ss},Error,Payment,Payment processing failed,customer@hotel.com,192.168.1.2\n";

            var bytes = System.Text.Encoding.UTF8.GetBytes(csvContent);
            return File(bytes, "text/csv", $"system_logs_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
        }

        // GET: Admin/ExportReportPDF
        public async Task<IActionResult> ExportReportPDF()
        {
            try
            {
                var today = DateTime.Today;
                var thisMonth = new DateTime(today.Year, today.Month, 1);
                var lastMonth = thisMonth.AddMonths(-1);

                // Get report data
                var reportData = new
                {
                    TodayRevenue = await CalculateRevenue(today, today.AddDays(1)),
                    ThisMonthRevenue = await CalculateRevenue(thisMonth, thisMonth.AddMonths(1)),
                    LastMonthRevenue = await CalculateRevenue(lastMonth, thisMonth),
                    TodayBookings = await _context.Reservations.CountAsync(r => r.CreatedDate.Date == today),
                    ThisMonthBookings = await _context.Reservations.CountAsync(r => r.CreatedDate >= thisMonth),
                    TotalRooms = await _context.Rooms.CountAsync(),
                    OccupiedRooms = await _context.Reservations.CountAsync(r =>
                        r.Status == "Confirmed" &&
                        r.CheckInDate <= today &&
                        r.CheckOutDate > today),
                    TotalPayments = await _context.Payments.SumAsync(p => p.Amount),
                    PendingPayments = await _context.QRPayments.Where(q => q.Status == "Pending").SumAsync(q => q.Amount),
                    CompletedPayments = await _context.QRPayments.Where(q => q.Status == "Paid").SumAsync(q => q.Amount)
                };

                // Create simple HTML content for PDF
                var htmlContent = $@"
                <html>
                <head>
                    <title>Hotel Booking Report</title>
                    <style>
                        body {{ font-family: Arial, sans-serif; margin: 20px; }}
                        .header {{ text-align: center; margin-bottom: 30px; }}
                        .section {{ margin-bottom: 20px; }}
                        .metric {{ display: inline-block; margin: 10px; padding: 15px; border: 1px solid #ddd; border-radius: 5px; }}
                        .metric-value {{ font-size: 24px; font-weight: bold; color: #007bff; }}
                        .metric-label {{ font-size: 14px; color: #666; }}
                    </style>
                </head>
                <body>
                    <div class='header'>
                        <h1>Hotel Booking System Report</h1>
                        <p>Generated on: {DateTime.Now:yyyy-MM-dd HH:mm:ss}</p>
                    </div>

                    <div class='section'>
                        <h2>Revenue Overview</h2>
                        <div class='metric'>
                            <div class='metric-value'>${reportData.TodayRevenue:N2}</div>
                            <div class='metric-label'>Today's Revenue</div>
                        </div>
                        <div class='metric'>
                            <div class='metric-value'>${reportData.ThisMonthRevenue:N2}</div>
                            <div class='metric-label'>This Month's Revenue</div>
                        </div>
                        <div class='metric'>
                            <div class='metric-value'>${reportData.LastMonthRevenue:N2}</div>
                            <div class='metric-label'>Last Month's Revenue</div>
                        </div>
                    </div>

                    <div class='section'>
                        <h2>Booking Statistics</h2>
                        <div class='metric'>
                            <div class='metric-value'>{reportData.TodayBookings}</div>
                            <div class='metric-label'>Today's Bookings</div>
                        </div>
                        <div class='metric'>
                            <div class='metric-value'>{reportData.ThisMonthBookings}</div>
                            <div class='metric-label'>This Month's Bookings</div>
                        </div>
                    </div>

                    <div class='section'>
                        <h2>Room Occupancy</h2>
                        <div class='metric'>
                            <div class='metric-value'>{reportData.OccupiedRooms}/{reportData.TotalRooms}</div>
                            <div class='metric-label'>Occupied Rooms</div>
                        </div>
                        <div class='metric'>
                            <div class='metric-value'>{(reportData.TotalRooms > 0 ? (double)reportData.OccupiedRooms / reportData.TotalRooms * 100 : 0):F1}%</div>
                            <div class='metric-label'>Occupancy Rate</div>
                        </div>
                    </div>

                    <div class='section'>
                        <h2>Payment Summary</h2>
                        <div class='metric'>
                            <div class='metric-value'>${reportData.TotalPayments:N2}</div>
                            <div class='metric-label'>Total Payments</div>
                        </div>
                        <div class='metric'>
                            <div class='metric-value'>${reportData.PendingPayments:N2}</div>
                            <div class='metric-label'>Pending Payments</div>
                        </div>
                        <div class='metric'>
                            <div class='metric-value'>${reportData.CompletedPayments:N2}</div>
                            <div class='metric-label'>Completed Payments</div>
                        </div>
                    </div>
                </body>
                </html>";

                var bytes = System.Text.Encoding.UTF8.GetBytes(htmlContent);
                return File(bytes, "text/html", $"hotel_report_{DateTime.Now:yyyyMMdd_HHmmss}.html");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed to export PDF report: " + ex.Message;
                return RedirectToAction("Reports");
            }
        }

        // GET: Admin/ExportReportExcel
        public async Task<IActionResult> ExportReportExcel()
        {
            try
            {
                var today = DateTime.Today;
                var thisMonth = new DateTime(today.Year, today.Month, 1);

                // Get reservations data
                var reservations = await _context.Reservations
                    .Include(r => r.User)
                    .Include(r => r.Room)
                    .ThenInclude(room => room!.RoomType)
                    .Where(r => r.CreatedDate >= thisMonth)
                    .OrderByDescending(r => r.CreatedDate)
                    .ToListAsync();

                // Create CSV content
                var csvContent = "Reservation ID,Guest Name,Room Number,Room Type,Check-in Date,Check-out Date,Status,Booking Date,Number of Guests\n";

                foreach (var reservation in reservations)
                {
                    csvContent += $"{reservation.ReservationID}," +
                                 $"\"{reservation.User?.UserName ?? "Unknown"}\"," +
                                 $"{reservation.Room?.RoomNumber ?? "N/A"}," +
                                 $"\"{reservation.Room?.RoomType?.TypeName ?? "N/A"}\"," +
                                 $"{reservation.CheckInDate:yyyy-MM-dd}," +
                                 $"{reservation.CheckOutDate:yyyy-MM-dd}," +
                                 $"{reservation.Status}," +
                                 $"{reservation.BookingDate:yyyy-MM-dd}," +
                                 $"{reservation.NumberOfGuests}\n";
                }

                var bytes = System.Text.Encoding.UTF8.GetBytes(csvContent);
                return File(bytes, "text/csv", $"hotel_reservations_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed to export Excel report: " + ex.Message;
                return RedirectToAction("Reports");
            }
        }

        private async Task<decimal> CalculateRevenue(DateTime startDate, DateTime endDate)
        {
            try
            {
                var confirmedReservations = await _context.Reservations
                    .Where(r => r.Status == "Confirmed" && r.CreatedDate >= startDate && r.CreatedDate < endDate)
                    .Include(r => r.Room)
                    .ToListAsync();

                decimal totalRevenue = 0;
                foreach (var reservation in confirmedReservations)
                {
                    if (reservation.Room != null)
                    {
                        var days = (reservation.CheckOutDate - reservation.CheckInDate).Days;
                        if (days > 0)
                        {
                            totalRevenue += reservation.Room.Price * days;
                        }
                    }
                }

                return totalRevenue;
            }
            catch (Exception)
            {
                // Return 0 if there's an error calculating revenue
                return 0;
            }
        }

        // Helper method to get monthly revenue data for charts
        private async Task<object> GetMonthlyRevenueData()
        {
            var monthlyData = new List<object>();
            var currentDate = DateTime.Today.AddMonths(-11); // Last 12 months

            for (int i = 0; i < 12; i++)
            {
                var monthStart = new DateTime(currentDate.Year, currentDate.Month, 1);
                var monthEnd = monthStart.AddMonths(1);

                var revenue = await CalculateRevenue(monthStart, monthEnd);
                var bookings = await _context.Reservations.CountAsync(r =>
                    r.CreatedDate >= monthStart && r.CreatedDate < monthEnd);

                monthlyData.Add(new
                {
                    Month = monthStart.ToString("MMM yyyy"),
                    Revenue = revenue,
                    Bookings = bookings
                });

                currentDate = currentDate.AddMonths(1);
            }

            return monthlyData;
        }

        // Helper method to get top customers data
        private async Task<object> GetTopCustomersData()
        {
            try
            {
                var confirmedReservations = await _context.Reservations
                    .Include(r => r.User)
                    .Include(r => r.Room)
                    .Where(r => r.Status == "Confirmed")
                    .ToListAsync();

                var customerData = confirmedReservations
                    .GroupBy(r => r.UserID)
                    .Select(g => new {
                        CustomerName = g.First().User?.UserName ?? "Unknown",
                        TotalBookings = g.Count(),
                        TotalSpent = g.Sum(r => r.Room != null ? r.Room.Price * (r.CheckOutDate - r.CheckInDate).Days : 0)
                    })
                    .OrderByDescending(x => x.TotalSpent)
                    .Take(5)
                    .ToList();

                return customerData;
            }
            catch (Exception)
            {
                // Return empty list if there's an error
                return new List<object>();
            }
        }

        // GET: Admin/PaymentManagement
        public async Task<IActionResult> PaymentManagement()
        {
            var payments = await _context.QRPayments
                .Include(q => q.Reservation!)
                    .ThenInclude(r => r.User)
                .Include(q => q.Reservation!)
                    .ThenInclude(r => r.Room)
                .OrderByDescending(q => q.CreatedDate)
                .ToListAsync();

            var paymentStats = new
            {
                TotalPayments = payments.Sum(p => p.Amount),
                PendingPayments = payments.Where(p => p.Status == "Pending").Sum(p => p.Amount),
                CompletedPayments = payments.Where(p => p.Status == "Paid").Sum(p => p.Amount),
                TotalTransactions = payments.Count,
                PendingTransactions = payments.Count(p => p.Status == "Pending"),
                CompletedTransactions = payments.Count(p => p.Status == "Paid")
            };

            ViewBag.PaymentStats = paymentStats;
            return View(payments);
        }

        // POST: Admin/MarkPaymentAsPaid/5
        [HttpPost]
        public async Task<IActionResult> MarkPaymentAsPaid(int id)
        {
            try
            {
                var payment = await _context.QRPayments.FindAsync(id);
                if (payment == null)
                {
                    return Json(new { success = false, message = "Payment not found" });
                }

                payment.Status = "Paid";
                payment.PaidDate = DateTime.Now;

                _context.QRPayments.Update(payment);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Payment marked as paid successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error updating payment: " + ex.Message });
            }
        }

        // POST: Admin/CancelPayment/5
        [HttpPost]
        public async Task<IActionResult> CancelPayment(int id)
        {
            try
            {
                var payment = await _context.QRPayments.FindAsync(id);
                if (payment == null)
                {
                    return Json(new { success = false, message = "Payment not found" });
                }

                payment.Status = "Cancelled";

                _context.QRPayments.Update(payment);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Payment cancelled successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error cancelling payment: " + ex.Message });
            }
        }
    }
}
