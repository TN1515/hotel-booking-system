using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Data;
using HotelBooking.Models;
using Microsoft.AspNetCore.Authorization;

namespace HotelBooking.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {
        private readonly HotelBookingContext _context;

        public ReservationsController(HotelBookingContext context)
        {
            _context = context;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            
            var reservations = await _context.Reservations
                .Include(r => r.Room)
                    .ThenInclude(rm => rm!.RoomType)
                .Include(r => r.User)
                .Where(r => r.UserID == userId)
                .OrderByDescending(r => r.CreatedDate)
                .ToListAsync();

            return View(reservations);
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Room)
                    .ThenInclude(rm => rm!.RoomType)
                .Include(r => r.User)
                .Include(r => r.Payments)
                .FirstOrDefaultAsync(m => m.ReservationID == id);

            if (reservation == null)
            {
                return NotFound();
            }

            // Check if user owns this reservation
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (reservation.UserID != userId)
            {
                return Forbid();
            }

            return View(reservation);
        }

        // GET: Reservations/Cancel/5
        public async Task<IActionResult> Cancel(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Room)
                    .ThenInclude(rm => rm!.RoomType)
                .FirstOrDefaultAsync(m => m.ReservationID == id);

            if (reservation == null)
            {
                return NotFound();
            }

            // Check if user owns this reservation
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (reservation.UserID != userId)
            {
                return Forbid();
            }

            // Check if reservation can be cancelled (e.g., not within 24 hours of check-in)
            if (reservation.CheckInDate <= DateTime.Today.AddDays(1))
            {
                TempData["Error"] = "Cannot cancel reservation within 24 hours of check-in date.";
                return RedirectToAction("Details", new { id = reservation.ReservationID });
            }

            return View(reservation);
        }

        // POST: Reservations/Cancel/5
        [HttpPost, ActionName("Cancel")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelConfirmed(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            
            if (reservation == null)
            {
                return NotFound();
            }

            // Check if user owns this reservation
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (reservation.UserID != userId)
            {
                return Forbid();
            }

            // Check if reservation can be cancelled
            if (reservation.CheckInDate <= DateTime.Today.AddDays(1))
            {
                TempData["Error"] = "Cannot cancel reservation within 24 hours of check-in date.";
                return RedirectToAction("Details", new { id = reservation.ReservationID });
            }

            // Create cancellation record
            var cancellation = new Cancellation
            {
                ReservationID = reservation.ReservationID,
                CancellationDate = DateTime.Now,
                Reason = "Cancelled by customer",
                RefundAmount = 0, // Calculate refund based on cancellation policy
                CreatedBy = User.Identity?.Name ?? "System",
                CreatedDate = DateTime.Now
            };

            _context.Cancellations.Add(cancellation);

            // Update reservation status
            reservation.Status = "Cancelled";
            reservation.ModifiedBy = User.Identity?.Name ?? "System";
            reservation.ModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();

            TempData["Message"] = "Reservation cancelled successfully.";
            return RedirectToAction("Index");
        }

        // GET: Reservations/Modify/5
        public async Task<IActionResult> Modify(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Room)
                    .ThenInclude(rm => rm!.RoomType)
                .FirstOrDefaultAsync(m => m.ReservationID == id);

            if (reservation == null)
            {
                return NotFound();
            }

            // Check if user owns this reservation
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (reservation.UserID != userId)
            {
                return Forbid();
            }

            // Check if reservation can be modified (e.g., not within 48 hours of check-in)
            if (reservation.CheckInDate <= DateTime.Today.AddDays(2))
            {
                TempData["Error"] = "Cannot modify reservation within 48 hours of check-in date.";
                return RedirectToAction("Details", new { id = reservation.ReservationID });
            }

            var modifyViewModel = new ModifyReservationViewModel
            {
                ReservationID = reservation.ReservationID,
                RoomID = reservation.RoomID,
                Room = reservation.Room,
                CheckInDate = reservation.CheckInDate,
                CheckOutDate = reservation.CheckOutDate,
                NumberOfGuests = reservation.NumberOfGuests,
                OriginalCheckInDate = reservation.CheckInDate,
                OriginalCheckOutDate = reservation.CheckOutDate
            };

            return View(modifyViewModel);
        }

        // POST: Reservations/Modify/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Modify(ModifyReservationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var reservation = await _context.Reservations.FindAsync(model.ReservationID);
                
                if (reservation == null)
                {
                    return NotFound();
                }

                // Check if user owns this reservation
                var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
                if (reservation.UserID != userId)
                {
                    return Forbid();
                }

                // Check if new dates are available
                var isAvailable = !await _context.Reservations
                    .AnyAsync(res => res.RoomID == reservation.RoomID &&
                                   res.ReservationID != reservation.ReservationID &&
                                   res.CheckInDate < model.CheckOutDate &&
                                   res.CheckOutDate > model.CheckInDate);

                if (!isAvailable)
                {
                    ModelState.AddModelError("", "The room is not available for the selected dates.");
                    model.Room = await _context.Rooms
                        .Include(r => r.RoomType)
                        .FirstOrDefaultAsync(r => r.RoomID == model.RoomID);
                    return View(model);
                }

                // Update reservation
                reservation.CheckInDate = model.CheckInDate;
                reservation.CheckOutDate = model.CheckOutDate;
                reservation.NumberOfGuests = model.NumberOfGuests;
                reservation.ModifiedBy = User.Identity?.Name ?? "System";
                reservation.ModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync();

                TempData["Message"] = "Reservation modified successfully.";
                return RedirectToAction("Details", new { id = reservation.ReservationID });
            }

            model.Room = await _context.Rooms
                .Include(r => r.RoomType)
                .FirstOrDefaultAsync(r => r.RoomID == model.RoomID);
            return View(model);
        }
    }
}
