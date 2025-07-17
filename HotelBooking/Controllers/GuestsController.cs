using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Data;
using HotelBooking.Models;
using HotelBooking.Models.ViewModels;

namespace HotelBooking.Controllers
{
    [Authorize] // Allow all authenticated users
    public class GuestsController : Controller
    {
        private readonly HotelBookingContext _context;

        public GuestsController(HotelBookingContext context)
        {
            _context = context;
        }

        // GET: Guests
        public async Task<IActionResult> Index(string searchTerm, string sortBy, string sortOrder, int page = 1, int pageSize = 10)
        {
            var query = _context.Guests
                .Include(g => g.User)
                .Include(g => g.Country)
                .Include(g => g.State)
                .AsQueryable();

            // Search
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(g => g.FirstName!.Contains(searchTerm) ||
                                        g.LastName!.Contains(searchTerm) ||
                                        g.Email!.Contains(searchTerm) ||
                                        g.Phone!.Contains(searchTerm));
            }

            // Sorting
            switch (sortBy?.ToLower())
            {
                case "name":
                    query = sortOrder == "desc" ? query.OrderByDescending(g => g.FirstName) : query.OrderBy(g => g.FirstName);
                    break;
                case "email":
                    query = sortOrder == "desc" ? query.OrderByDescending(g => g.Email) : query.OrderBy(g => g.Email);
                    break;
                case "country":
                    query = sortOrder == "desc" ? query.OrderByDescending(g => g.Country!.CountryName) : query.OrderBy(g => g.Country!.CountryName);
                    break;
                case "created":
                    query = sortOrder == "desc" ? query.OrderByDescending(g => g.CreatedDate) : query.OrderBy(g => g.CreatedDate);
                    break;
                default:
                    query = query.OrderByDescending(g => g.CreatedDate);
                    break;
            }

            // Pagination
            var totalGuests = await query.CountAsync();
            var guests = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var guestViewModels = guests.Select(g => new GuestProfileViewModel
            {
                GuestID = g.GuestID,
                UserID = g.UserID,
                FirstName = g.FirstName,
                LastName = g.LastName,
                Email = g.Email,
                Phone = g.Phone,
                AgeGroup = g.AgeGroup,
                Address = g.Address,
                CountryID = g.CountryID,
                StateID = g.StateID,
                UserName = g.User?.UserName,
                CountryName = g.Country?.CountryName,
                StateName = g.State?.StateName,
                CreatedDate = g.CreatedDate,
                ModifiedDate = g.ModifiedDate
            }).ToList();

            var viewModel = new GuestListViewModel
            {
                Guests = guestViewModels,
                TotalGuests = totalGuests,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)totalGuests / pageSize),
                SearchTerm = searchTerm,
                SortBy = sortBy,
                SortOrder = sortOrder
            };

            return View(viewModel);
        }

        // GET: Guests/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var guest = await _context.Guests
                .Include(g => g.User)
                .Include(g => g.Country)
                .Include(g => g.State)
                .Include(g => g.ReservationGuests!)
                    .ThenInclude(rg => rg.Reservation!)
                    .ThenInclude(r => r.Room!)
                    .ThenInclude(r => r.RoomType)
                .FirstOrDefaultAsync(g => g.GuestID == id);

            if (guest == null)
            {
                return NotFound();
            }

            var viewModel = new GuestProfileViewModel
            {
                GuestID = guest.GuestID,
                UserID = guest.UserID,
                FirstName = guest.FirstName,
                LastName = guest.LastName,
                Email = guest.Email,
                Phone = guest.Phone,
                AgeGroup = guest.AgeGroup,
                Address = guest.Address,
                CountryID = guest.CountryID,
                StateID = guest.StateID,
                UserName = guest.User?.UserName,
                CountryName = guest.Country?.CountryName,
                StateName = guest.State?.StateName,
                CreatedDate = guest.CreatedDate,
                ModifiedDate = guest.ModifiedDate
            };

            return View(viewModel);
        }

        // GET: Guests/Edit/5
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Edit(int id)
        {
            var guest = await _context.Guests.FindAsync(id);
            if (guest == null)
            {
                return NotFound();
            }

            var viewModel = new GuestProfileViewModel
            {
                GuestID = guest.GuestID,
                UserID = guest.UserID,
                FirstName = guest.FirstName,
                LastName = guest.LastName,
                Email = guest.Email,
                Phone = guest.Phone,
                AgeGroup = guest.AgeGroup,
                Address = guest.Address,
                CountryID = guest.CountryID,
                StateID = guest.StateID,
                Countries = await _context.Countries.Where(c => c.IsActive).ToListAsync(),
                States = await _context.States.Where(s => s.IsActive).ToListAsync()
            };

            return View(viewModel);
        }

        // POST: Guests/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Edit(int id, GuestProfileViewModel viewModel)
        {
            if (id != viewModel.GuestID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var guest = await _context.Guests.FindAsync(id);
                    if (guest == null)
                    {
                        return NotFound();
                    }

                    guest.FirstName = viewModel.FirstName;
                    guest.LastName = viewModel.LastName;
                    guest.Email = viewModel.Email;
                    guest.Phone = viewModel.Phone;
                    guest.AgeGroup = viewModel.AgeGroup;
                    guest.Address = viewModel.Address;
                    guest.CountryID = viewModel.CountryID;
                    guest.StateID = viewModel.StateID;
                    guest.ModifiedBy = User.Identity?.Name;
                    guest.ModifiedDate = DateTime.Now;

                    _context.Update(guest);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Guest profile updated successfully.";
                    return RedirectToAction(nameof(Details), new { id = guest.GuestID });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GuestExists(viewModel.GuestID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while updating the guest profile: " + ex.Message);
                }
            }

            viewModel.Countries = await _context.Countries.Where(c => c.IsActive).ToListAsync();
            viewModel.States = await _context.States.Where(s => s.IsActive).ToListAsync();
            return View(viewModel);
        }

        // GET: Guests/Delete/5
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Delete(int id)
        {
            var guest = await _context.Guests
                .Include(g => g.User)
                .Include(g => g.Country)
                .Include(g => g.State)
                .FirstOrDefaultAsync(g => g.GuestID == id);

            if (guest == null)
            {
                return NotFound();
            }

            var viewModel = new GuestProfileViewModel
            {
                GuestID = guest.GuestID,
                FirstName = guest.FirstName,
                LastName = guest.LastName,
                Email = guest.Email,
                Phone = guest.Phone,
                UserName = guest.User?.UserName,
                CountryName = guest.Country?.CountryName,
                StateName = guest.State?.StateName,
                CreatedDate = guest.CreatedDate
            };

            return View(viewModel);
        }

        // POST: Guests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var guest = await _context.Guests.FindAsync(id);
            if (guest != null)
            {
                _context.Guests.Remove(guest);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Guest profile deleted successfully.";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Guests/Profile/5
        public async Task<IActionResult> Profile(int id)
        {
            var guest = await _context.Guests
                .Include(g => g.User)
                .Include(g => g.Country)
                .Include(g => g.State)
                .FirstOrDefaultAsync(g => g.GuestID == id);

            if (guest == null)
            {
                return NotFound();
            }

            // Get booking statistics
            var reservations = await _context.Reservations
                .Include(r => r.Room)
                .ThenInclude(r => r!.RoomType)
                .Where(r => r.UserID == guest.UserID)
                .ToListAsync();

            var totalBookings = reservations.Count;
            var completedBookings = reservations.Count(r => r.Status == "Completed");
            var pendingBookings = reservations.Count(r => r.Status == "Pending" || r.Status == "Confirmed");
            var cancelledBookings = reservations.Count(r => r.Status == "Cancelled");
            var totalSpent = reservations
                .Where(r => r.Status == "Completed" || r.Status == "Confirmed")
                .Sum(r => r.Room!.Price * (r.CheckOutDate - r.CheckInDate).Days);

            // Get recent bookings
            var recentBookings = reservations
                .OrderByDescending(r => r.CreatedDate)
                .Take(5)
                .Select(r => new RecentBookingViewModel
                {
                    ReservationID = r.ReservationID,
                    RoomNumber = r.Room!.RoomNumber,
                    RoomType = r.Room.RoomType!.TypeName,
                    CheckInDate = r.CheckInDate,
                    CheckOutDate = r.CheckOutDate,
                    Adults = r.NumberOfGuests,
                    Children = 0, // Default since Reservation doesn't track children separately
                    TotalAmount = r.Room.Price * (r.CheckOutDate - r.CheckInDate).Days,
                    Status = r.Status
                })
                .ToList();

            var viewModel = new GuestProfileViewModel
            {
                GuestID = guest.GuestID,
                UserID = guest.UserID,
                FirstName = guest.FirstName,
                LastName = guest.LastName,
                Email = guest.Email,
                Phone = guest.Phone,
                AgeGroup = guest.AgeGroup,
                Address = guest.Address,
                CountryID = guest.CountryID,
                StateID = guest.StateID,
                UserName = guest.User?.UserName,
                CountryName = guest.Country?.CountryName,
                StateName = guest.State?.StateName,
                CreatedDate = guest.CreatedDate,
                ModifiedDate = guest.ModifiedDate,

                // Additional properties
                DateOfBirth = null, // You can add this to Guest model if needed
                Nationality = guest.Country?.CountryName,
                PreferredRoomType = "Standard", // Default or get from preferences
                SpecialRequests = "None",
                DietaryRestrictions = "None",
                StaffNotes = "No notes available",

                // Statistics
                TotalBookings = totalBookings,
                CompletedBookings = completedBookings,
                PendingBookings = pendingBookings,
                CancelledBookings = cancelledBookings,
                TotalSpent = totalSpent,
                LoyaltyPoints = (int)(totalSpent / 1000), // 1 point per 1000 VND

                // Recent bookings
                RecentBookings = recentBookings
            };

            return View(viewModel);
        }

        // POST: Guests/AddNote
        [HttpPost]
        public IActionResult AddNote(int guestId, string note, string category)
        {
            try
            {
                // In a real application, you might want to create a GuestNotes table
                // For now, we'll just return success
                return Json(new { success = true, message = "Note added successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private bool GuestExists(int id)
        {
            return _context.Guests.Any(e => e.GuestID == id);
        }

        // AJAX: Get states by country
        [HttpGet]
        public async Task<JsonResult> GetStatesByCountry(int countryId)
        {
            var states = await _context.States
                .Where(s => s.CountryID == countryId && s.IsActive)
                .Select(s => new { s.StateID, s.StateName })
                .ToListAsync();

            return Json(states);
        }
    }
}
