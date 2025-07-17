using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Data;
using HotelBooking.Models;
using HotelBooking.Models.ViewModels;

namespace HotelBooking.Controllers
{
    [Authorize(Roles = "Admin,Staff")]
    public class PaymentProcessController : Controller
    {
        private readonly HotelBookingContext _context;

        public PaymentProcessController(HotelBookingContext context)
        {
            _context = context;
        }

        // GET: PaymentProcess
        public async Task<IActionResult> Index(string searchTerm, string statusFilter, DateTime? fromDate, DateTime? toDate, int page = 1, int pageSize = 10)
        {
            var query = _context.PaymentBatches
                .Include(pb => pb.User)
                .AsQueryable();

            // Search
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(pb => pb.User!.UserName!.Contains(searchTerm) ||
                                         pb.User.Email!.Contains(searchTerm));
            }

            // Date filter
            if (fromDate.HasValue)
            {
                query = query.Where(pb => pb.PaymentDate >= fromDate.Value);
            }
            if (toDate.HasValue)
            {
                query = query.Where(pb => pb.PaymentDate <= toDate.Value);
            }

            var totalPayments = await query.CountAsync();
            var totalAmount = await query.SumAsync(pb => pb.TotalAmount);

            var paymentBatches = await query
                .OrderByDescending(pb => pb.PaymentDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var payments = new List<PaymentProcessViewModel>();
            foreach (var batch in paymentBatches)
            {
                var batchPayments = await _context.Payments
                    .Include(p => p.Reservation!)
                        .ThenInclude(r => r.User)
                    .Include(p => p.Reservation!)
                        .ThenInclude(r => r.Room!)
                        .ThenInclude(r => r.RoomType)
                    .Where(p => p.PaymentBatchID == batch.PaymentBatchID)
                    .ToListAsync();

                foreach (var payment in batchPayments)
                {
                    payments.Add(new PaymentProcessViewModel
                    {
                        PaymentID = payment.PaymentID,
                        ReservationID = payment.ReservationID,
                        Amount = payment.Amount,
                        PaymentMethod = batch.PaymentMethod,
                        Status = "Completed",
                        PaymentDate = batch.PaymentDate,
                        GuestName = $"{payment.Reservation?.User?.UserName}",
                        RoomNumber = payment.Reservation?.Room?.RoomNumber,
                        RoomType = payment.Reservation?.Room?.RoomType?.TypeName,
                        CheckInDate = payment.Reservation?.CheckInDate ?? DateTime.MinValue,
                        CheckOutDate = payment.Reservation?.CheckOutDate ?? DateTime.MinValue,
                        NumberOfGuests = payment.Reservation?.NumberOfGuests ?? 0,
                        UserEmail = payment.Reservation?.User?.Email,
                        UserPhone = payment.Reservation?.User?.PhoneNumber
                    });
                }
            }

            var viewModel = new PaymentListViewModel
            {
                Payments = payments,
                TotalPayments = totalPayments,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)totalPayments / pageSize),
                SearchTerm = searchTerm,
                StatusFilter = statusFilter,
                FromDate = fromDate,
                ToDate = toDate,
                TotalAmount = totalAmount
            };

            return View(viewModel);
        }

        // GET: PaymentProcess/Process/5
        public async Task<IActionResult> Process(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Room!)
                    .ThenInclude(r => r.RoomType)
                .Include(r => r.Payments!)
                    .ThenInclude(p => p.PaymentBatch)
                .FirstOrDefaultAsync(r => r.ReservationID == id);

            if (reservation == null)
            {
                return NotFound();
            }

            var totalAmount = CalculateTotalAmount(reservation);
            var paidAmount = reservation.Payments?.Sum(p => p.Amount) ?? 0;

            var viewModel = new ProcessPaymentViewModel
            {
                ReservationID = reservation.ReservationID,
                GuestName = reservation.User?.UserName,
                RoomNumber = reservation.Room?.RoomNumber,
                TotalAmount = totalAmount,
                PaidAmount = paidAmount,
                RemainingAmount = totalAmount - paidAmount
            };

            return View(viewModel);
        }

        // POST: PaymentProcess/Process
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Process(ProcessPaymentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var reservation = await _context.Reservations.FindAsync(viewModel.ReservationID);
                    if (reservation == null)
                    {
                        return NotFound();
                    }

                    // Create payment batch
                    var paymentBatch = new PaymentBatch
                    {
                        UserID = reservation.UserID,
                        PaymentDate = DateTime.Now,
                        TotalAmount = viewModel.Amount,
                        PaymentMethod = viewModel.PaymentMethod
                    };

                    _context.PaymentBatches.Add(paymentBatch);
                    await _context.SaveChangesAsync();

                    // Create payment
                    var payment = new Payment
                    {
                        ReservationID = viewModel.ReservationID,
                        Amount = viewModel.Amount,
                        PaymentBatchID = paymentBatch.PaymentBatchID
                    };

                    _context.Payments.Add(payment);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Payment processed successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while processing the payment: " + ex.Message);
                }
            }

            // Reload reservation data for display
            var reservation2 = await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Room!)
                    .ThenInclude(r => r.RoomType)
                .Include(r => r.Payments!)
                    .ThenInclude(p => p.PaymentBatch)
                .FirstOrDefaultAsync(r => r.ReservationID == viewModel.ReservationID);

            if (reservation2 != null)
            {
                var totalAmount = CalculateTotalAmount(reservation2);
                var paidAmount = reservation2.Payments?.Sum(p => p.Amount) ?? 0;

                viewModel.GuestName = reservation2.User?.UserName;
                viewModel.RoomNumber = reservation2.Room?.RoomNumber;
                viewModel.TotalAmount = totalAmount;
                viewModel.PaidAmount = paidAmount;
                viewModel.RemainingAmount = totalAmount - paidAmount;
            }

            return View(viewModel);
        }

        // GET: PaymentProcess/Refund/5
        public async Task<IActionResult> Refund(int id)
        {
            var payment = await _context.Payments
                .Include(p => p.Reservation!)
                    .ThenInclude(r => r.User)
                .Include(p => p.Reservation!)
                    .ThenInclude(r => r.Room)
                .Include(p => p.PaymentBatch)
                .FirstOrDefaultAsync(p => p.PaymentID == id);

            if (payment == null)
            {
                return NotFound();
            }

            var viewModel = new RefundProcessViewModel
            {
                PaymentID = payment.PaymentID,
                OriginalAmount = payment.Amount,
                RefundAmount = payment.Amount,
                GuestName = payment.Reservation?.User?.UserName,
                RoomNumber = payment.Reservation?.Room?.RoomNumber,
                PaymentDate = payment.PaymentBatch?.PaymentDate ?? DateTime.MinValue,
                RefundMethods = await _context.RefundMethods.Where(rm => rm.IsActive).ToListAsync()
            };

            return View(viewModel);
        }

        // POST: PaymentProcess/Refund
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Refund(RefundProcessViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var payment = await _context.Payments.FindAsync(viewModel.PaymentID);
                    if (payment == null)
                    {
                        return NotFound();
                    }

                    var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity!.Name);
                    if (currentUser == null)
                    {
                        ModelState.AddModelError("", "Unable to identify current user.");
                        viewModel.RefundMethods = await _context.RefundMethods.Where(rm => rm.IsActive).ToListAsync();
                        return View(viewModel);
                    }

                    var refund = new Refund
                    {
                        PaymentID = viewModel.PaymentID,
                        RefundAmount = viewModel.RefundAmount,
                        RefundDate = DateTime.Now,
                        RefundReason = viewModel.RefundReason,
                        RefundMethodID = viewModel.RefundMethodID,
                        ProcessedByUserID = currentUser.Id,
                        RefundStatus = "Processed"
                    };

                    _context.Refunds.Add(refund);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Refund processed successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while processing the refund: " + ex.Message);
                }
            }

            viewModel.RefundMethods = await _context.RefundMethods.Where(rm => rm.IsActive).ToListAsync();
            return View(viewModel);
        }

        // GET: PaymentProcess/Success
        public async Task<IActionResult> Success(int? reservationId, string? transactionId)
        {
            if (reservationId == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Room!)
                    .ThenInclude(r => r.RoomType)
                .Include(r => r.Payments!)
                    .ThenInclude(p => p.PaymentBatch)
                .FirstOrDefaultAsync(r => r.ReservationID == reservationId);

            if (reservation == null)
            {
                return NotFound();
            }

            var latestPayment = reservation.Payments?.OrderByDescending(p => p.PaymentBatch!.PaymentDate).FirstOrDefault();

            var viewModel = new PaymentSuccessViewModel
            {
                TransactionId = transactionId ?? $"TXN{DateTime.Now:yyyyMMddHHmmss}",
                Amount = latestPayment?.Amount ?? CalculateTotalAmount(reservation),
                PaymentMethod = latestPayment?.PaymentBatch?.PaymentMethod ?? "Credit Card",
                PaymentDate = latestPayment?.PaymentBatch?.PaymentDate ?? DateTime.Now,
                Status = "Completed",

                // Booking information
                ReservationId = reservation.ReservationID,
                GuestName = reservation.User?.UserName ?? "Guest",
                RoomNumber = reservation.Room?.RoomNumber ?? "N/A",
                RoomType = reservation.Room?.RoomType?.TypeName ?? "Standard",
                CheckInDate = reservation.CheckInDate,
                CheckOutDate = reservation.CheckOutDate,

                // Additional information
                ConfirmationCode = $"CONF{reservation.ReservationID:D6}",
                Notes = "Payment processed successfully. Thank you for choosing our hotel!"
            };

            return View(viewModel);
        }

        private decimal CalculateTotalAmount(Reservation reservation)
        {
            if (reservation.Room?.Price == null) return 0;

            var nights = (reservation.CheckOutDate - reservation.CheckInDate).Days;
            return reservation.Room.Price * nights;
        }
    }
}
