using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Data;
using HotelBooking.Models;
using Microsoft.AspNetCore.Authorization;

namespace HotelBooking.Controllers
{
    public class RoomsController : Controller
    {
        private readonly HotelBookingContext _context;

        public RoomsController(HotelBookingContext context)
        {
            _context = context;
        }

        // GET: Rooms
        public async Task<IActionResult> Index()
        {
            var rooms = await _context.Rooms
                .Include(r => r.RoomType)
                .Where(r => r.IsActive)
                .ToListAsync();
            return View(rooms);
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .Include(r => r.RoomType)
                .Include(r => r.RoomAmenities)
                    .ThenInclude(ra => ra.Amenity)
                .FirstOrDefaultAsync(m => m.RoomID == id);

            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // GET: Rooms/Available
        public async Task<IActionResult> Available(DateTime? checkIn, DateTime? checkOut, int? guests)
        {
            var query = _context.Rooms
                .Include(r => r.RoomType)
                .Where(r => r.IsActive && r.Status == "Available");

            if (checkIn.HasValue && checkOut.HasValue)
            {
                // Check for rooms that are not booked during the specified period
                var bookedRoomIds = await _context.Reservations
                    .Where(res => res.CheckInDate < checkOut && res.CheckOutDate > checkIn)
                    .Select(res => res.RoomID)
                    .ToListAsync();

                query = query.Where(r => !bookedRoomIds.Contains(r.RoomID));
            }

            if (guests.HasValue)
            {
                query = query.Where(r => r.RoomType!.MaxOccupancy >= guests);
            }

            var availableRooms = await query.ToListAsync();

            ViewBag.CheckIn = checkIn?.ToString("yyyy-MM-dd");
            ViewBag.CheckOut = checkOut?.ToString("yyyy-MM-dd");
            ViewBag.Guests = guests;

            return View(availableRooms);
        }

        // GET: Rooms/Search
        public IActionResult Search()
        {
            ViewBag.RoomTypes = _context.RoomTypes.Where(rt => rt.IsActive).ToList();
            return View();
        }

        // POST: Rooms/Search
        [HttpPost]
        public async Task<IActionResult> Search(DateTime checkIn, DateTime checkOut, int guests, int? roomTypeId, decimal? maxPrice)
        {
            var query = _context.Rooms
                .Include(r => r.RoomType)
                .Where(r => r.IsActive && r.Status == "Available");

            // Check availability
            var bookedRoomIds = await _context.Reservations
                .Where(res => res.CheckInDate < checkOut && res.CheckOutDate > checkIn)
                .Select(res => res.RoomID)
                .ToListAsync();

            query = query.Where(r => !bookedRoomIds.Contains(r.RoomID));

            // Filter by guest capacity
            query = query.Where(r => r.RoomType!.MaxOccupancy >= guests);

            // Filter by room type
            if (roomTypeId.HasValue)
            {
                query = query.Where(r => r.RoomTypeID == roomTypeId);
            }

            // Filter by max price
            if (maxPrice.HasValue)
            {
                query = query.Where(r => r.Price <= maxPrice);
            }

            var searchResults = await query.ToListAsync();

            ViewBag.CheckIn = checkIn.ToString("yyyy-MM-dd");
            ViewBag.CheckOut = checkOut.ToString("yyyy-MM-dd");
            ViewBag.Guests = guests;
            ViewBag.RoomTypeId = roomTypeId;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.RoomTypes = _context.RoomTypes.Where(rt => rt.IsActive).ToList();

            return View("SearchResults", searchResults);
        }

        // GET: Rooms/Book/5
        [Authorize]
        public async Task<IActionResult> Book(int? id, DateTime? checkIn, DateTime? checkOut, int? guests)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .Include(r => r.RoomType)
                .FirstOrDefaultAsync(r => r.RoomID == id);

            if (room == null)
            {
                return NotFound();
            }

            var bookingViewModel = new BookingViewModel
            {
                RoomID = room.RoomID,
                Room = room,
                CheckInDate = checkIn ?? DateTime.Today.AddDays(1),
                CheckOutDate = checkOut ?? DateTime.Today.AddDays(2),
                NumberOfGuests = guests ?? 1
            };

            return View(bookingViewModel);
        }

        // POST: Rooms/Book
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book(BookingViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if room is still available
                var isAvailable = !await _context.Reservations
                    .AnyAsync(res => res.RoomID == model.RoomID &&
                                   res.CheckInDate < model.CheckOutDate &&
                                   res.CheckOutDate > model.CheckInDate);

                if (!isAvailable)
                {
                    ModelState.AddModelError("", "Sorry, this room is no longer available for the selected dates.");
                    model.Room = await _context.Rooms
                        .Include(r => r.RoomType)
                        .FirstOrDefaultAsync(r => r.RoomID == model.RoomID);
                    return View(model);
                }

                var reservation = new Reservation
                {
                    UserID = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0"),
                    RoomID = model.RoomID,
                    BookingDate = DateTime.Now,
                    CheckInDate = model.CheckInDate,
                    CheckOutDate = model.CheckOutDate,
                    NumberOfGuests = model.NumberOfGuests,
                    Status = "Confirmed",
                    CreatedBy = User.Identity?.Name ?? "System",
                    CreatedDate = DateTime.Now
                };

                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Room booked successfully!";
                return RedirectToAction("Details", "Reservations", new { id = reservation.ReservationID });
            }

            model.Room = await _context.Rooms
                .Include(r => r.RoomType)
                .FirstOrDefaultAsync(r => r.RoomID == model.RoomID);
            return View(model);
        }
    }
}
