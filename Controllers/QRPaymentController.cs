using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Data;
using HotelBooking.Models;
using HotelBooking.Services;
using HotelBooking.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace HotelBooking.Controllers
{
    [Authorize]
    public class QRPaymentController : Controller
    {
        private readonly HotelBookingContext _context;
        private readonly IQRPaymentService _qrPaymentService;

        public QRPaymentController(HotelBookingContext context, IQRPaymentService qrPaymentService)
        {
            _context = context;
            _qrPaymentService = qrPaymentService;
        }

        // GET: QRPayment/Generate
        public async Task<IActionResult> Generate(int reservationId, decimal amount)
        {
            var reservation = await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Room!)
                    .ThenInclude(r => r.RoomType)
                .FirstOrDefaultAsync(r => r.ReservationID == reservationId);

            if (reservation == null)
            {
                return NotFound();
            }

            // Check if user owns this reservation
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            if (reservation.UserID != currentUserId && !User.IsInRole("Admin") && !User.IsInRole("Staff"))
            {
                return Forbid();
            }

            var nights = (reservation.CheckOutDate - reservation.CheckInDate).Days;
            var calculatedAmount = (reservation.Room?.Price ?? 0) * nights;

            // Use calculated amount if provided amount is 0
            var finalAmount = amount > 0 ? amount : calculatedAmount;

            var description = $"Hotel booking payment - Room {reservation.Room?.RoomNumber} - {reservation.CheckInDate:dd/MM/yyyy} to {reservation.CheckOutDate:dd/MM/yyyy}";

            // Create QR Payment record
            var qrPayment = _qrPaymentService.CreateQRPayment(
                reservationId,
                finalAmount,
                description,
                currentUserId
            );

            // Save QR Payment to database
            _context.QRPayments.Add(qrPayment);
            await _context.SaveChangesAsync();

            var viewModel = new QRPaymentGenerateViewModel
            {
                QRPaymentID = qrPayment.QRPaymentID,
                ReservationID = reservationId,
                Amount = finalAmount,
                BankCode = qrPayment.BankCode ?? "VietinBank",
                AccountNumber = qrPayment.AccountNumber ?? "1038766815877",
                AccountName = qrPayment.AccountName ?? "LUU VAN HIEN",
                QRCodeData = qrPayment.QRCodeData ?? "",
                TransactionDescription = description,
                TransactionReference = qrPayment.TransactionReference ?? "",

                // Reservation details
                RoomNumber = reservation.Room?.RoomNumber ?? "N/A",
                RoomType = reservation.Room?.RoomType?.TypeName ?? "Standard",
                CheckInDate = reservation.CheckInDate,
                CheckOutDate = reservation.CheckOutDate,
                NumberOfNights = nights,
                GuestName = reservation.User?.UserName ?? "Guest"
            };

            return View(viewModel);
        }

        // GET: QRPayment/Create/5
        public async Task<IActionResult> Create(int id)
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
            var remainingAmount = totalAmount - paidAmount;

            if (remainingAmount <= 0)
            {
                TempData["ErrorMessage"] = "This reservation is already fully paid.";
                return RedirectToAction("Details", "Reservations", new { id = id });
            }

            var viewModel = new QRPaymentViewModel
            {
                ReservationID = reservation.ReservationID,
                GuestName = reservation.User?.UserName,
                RoomNumber = reservation.Room?.RoomNumber,
                RoomType = reservation.Room?.RoomType?.TypeName,
                CheckInDate = reservation.CheckInDate,
                CheckOutDate = reservation.CheckOutDate,
                TotalAmount = totalAmount,
                PaidAmount = paidAmount,
                RemainingAmount = remainingAmount,
                Amount = remainingAmount // Default to full remaining amount
            };

            return View(viewModel);
        }

        // POST: QRPayment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QRPaymentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var reservation = await _context.Reservations
                        .Include(r => r.User)
                        .Include(r => r.Room)
                        .FirstOrDefaultAsync(r => r.ReservationID == viewModel.ReservationID);

                    if (reservation == null)
                    {
                        return NotFound();
                    }

                    var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
                    var description = $"Hotel booking payment - Room {reservation.Room?.RoomNumber} - {reservation.User?.UserName}";

                    var qrPayment = _qrPaymentService.CreateQRPayment(
                        viewModel.ReservationID,
                        viewModel.Amount,
                        description,
                        userId
                    );

                    _context.QRPayments.Add(qrPayment);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Display", new { id = qrPayment.QRPaymentID });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while creating QR payment: " + ex.Message);
                }
            }

            // Reload data if validation fails
            var reservationData = await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Room!)
                    .ThenInclude(r => r.RoomType)
                .FirstOrDefaultAsync(r => r.ReservationID == viewModel.ReservationID);

            if (reservationData != null)
            {
                viewModel.GuestName = reservationData.User?.UserName;
                viewModel.RoomNumber = reservationData.Room?.RoomNumber;
                viewModel.RoomType = reservationData.Room?.RoomType?.TypeName;
            }

            return View(viewModel);
        }

        // GET: QRPayment/Display/5
        public async Task<IActionResult> Display(int id)
        {
            var qrPayment = await _context.QRPayments
                .Include(q => q.Reservation!)
                    .ThenInclude(r => r.User)
                .Include(q => q.Reservation!)
                    .ThenInclude(r => r.Room)
                .FirstOrDefaultAsync(q => q.QRPaymentID == id);

            if (qrPayment == null)
            {
                return NotFound();
            }

            return View(qrPayment);
        }

        // POST: QRPayment/ConfirmPayment
        [HttpPost]
        public async Task<IActionResult> ConfirmPayment([FromBody] ConfirmPaymentRequest request)
        {
            try
            {
                var qrPayment = await _context.QRPayments.FindAsync(request.QrPaymentId);
                if (qrPayment == null)
                {
                    return Json(new { success = false, message = "QR Payment not found" });
                }

                var reservation = await _context.Reservations.FindAsync(request.ReservationId);
                if (reservation == null)
                {
                    return Json(new { success = false, message = "Reservation not found" });
                }

                // Update QR Payment status
                qrPayment.Status = "Completed";
                qrPayment.PaidDate = DateTime.Now;

                // Update Reservation status
                reservation.Status = "Confirmed";
                reservation.ModifiedBy = User.Identity?.Name ?? "Customer";
                reservation.ModifiedDate = DateTime.Now;

                // Create Payment record
                var paymentBatch = new PaymentBatch
                {
                    UserID = reservation.UserID,
                    PaymentDate = DateTime.Now,
                    TotalAmount = qrPayment.Amount,
                    PaymentMethod = "QR Code - VietinBank"
                };

                _context.PaymentBatches.Add(paymentBatch);
                await _context.SaveChangesAsync();

                var payment = new Payment
                {
                    ReservationID = reservation.ReservationID,
                    Amount = qrPayment.Amount,
                    PaymentBatchID = paymentBatch.PaymentBatchID
                };

                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();

                // Create notification for payment confirmation
                var paymentNotification = new Notification
                {
                    UserID = reservation.UserID,
                    Title = "Payment Confirmed",
                    Message = $"Payment of {qrPayment.Amount:N0} VND for your reservation has been confirmed. Your booking is now complete!",
                    Type = "Payment",
                    Status = "Sent",
                    CreatedDate = DateTime.Now,
                    SentDate = DateTime.Now,
                    IsRead = false,
                    CreatedBy = "System"
                };
                _context.Notifications.Add(paymentNotification);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Payment confirmed successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private decimal CalculateTotalAmount(Reservation reservation)
        {
            if (reservation.Room == null) return 0;

            var nights = (reservation.CheckOutDate - reservation.CheckInDate).Days;
            return reservation.Room.Price * nights;
        }
    }

    public class ConfirmPaymentRequest
    {
        public int QrPaymentId { get; set; }
        public int ReservationId { get; set; }
    }
}
