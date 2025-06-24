using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Data;
using HotelBooking.Models;
using HotelBooking.Models.ViewModels;

namespace HotelBooking.Controllers
{
    [Authorize(Roles = "Admin,Staff")]
    public class GuestProfileController : Controller
    {
        private readonly HotelBookingContext _context;

        public GuestProfileController(HotelBookingContext context)
        {
            _context = context;
        }

        // GET: GuestProfile
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

            // Sort
            query = sortBy switch
            {
                "name" => sortOrder == "desc" ? query.OrderByDescending(g => g.FirstName) : query.OrderBy(g => g.FirstName),
                "email" => sortOrder == "desc" ? query.OrderByDescending(g => g.Email) : query.OrderBy(g => g.Email),
                "country" => sortOrder == "desc" ? query.OrderByDescending(g => g.Country!.CountryName) : query.OrderBy(g => g.Country!.CountryName),
                "created" => sortOrder == "desc" ? query.OrderByDescending(g => g.CreatedDate) : query.OrderBy(g => g.CreatedDate),
                _ => query.OrderBy(g => g.FirstName)
            };

            var totalGuests = await query.CountAsync();
            var guests = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(g => new GuestProfileViewModel
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
                    UserName = g.User!.UserName,
                    CountryName = g.Country!.CountryName,
                    StateName = g.State!.StateName,
                    CreatedDate = g.CreatedDate,
                    ModifiedDate = g.ModifiedDate
                })
                .ToListAsync();

            var viewModel = new GuestListViewModel
            {
                Guests = guests,
                TotalGuests = totalGuests,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)totalGuests / pageSize),
                SearchTerm = searchTerm,
                SortBy = sortBy,
                SortOrder = sortOrder
            };

            return View(viewModel);
        }

        // GET: GuestProfile/Details/5
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

        // GET: GuestProfile/Edit/5
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

        // POST: GuestProfile/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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
            }

            viewModel.Countries = await _context.Countries.Where(c => c.IsActive).ToListAsync();
            viewModel.States = await _context.States.Where(s => s.IsActive).ToListAsync();
            return View(viewModel);
        }

        // GET: GuestProfile/Delete/5
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

        // POST: GuestProfile/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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
