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
                TotalRevenue = await _context.Reservations
                    .Where(r => r.Status == "Confirmed")
                    .Join(_context.Rooms, r => r.RoomID, room => room.RoomID, (r, room) => new { r, room })
                    .SumAsync(x => x.room.Price * (x.r.CheckOutDate - x.r.CheckInDate).Days),
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
            ViewBag.TotalRevenue = await _context.Reservations
                .Where(r => r.Status == "Confirmed")
                .Join(_context.Rooms, r => r.RoomID, room => room.RoomID, (r, room) => new { r, room })
                .SumAsync(x => x.room.Price * (x.r.CheckOutDate - x.r.CheckInDate).Days);

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
    }
}
