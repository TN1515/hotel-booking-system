using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Data;
using HotelBooking.Models;
using HotelBooking.Models.ViewModels;

namespace HotelBooking.Controllers
{
    [Authorize(Roles = "Admin,Staff")]
    public class AmenitiesController : Controller
    {
        private readonly HotelBookingContext _context;

        public AmenitiesController(HotelBookingContext context)
        {
            _context = context;
        }

        // GET: Amenities
        public async Task<IActionResult> Index(string searchTerm, string categoryFilter, string statusFilter, int page = 1, int pageSize = 10)
        {
            var query = _context.Amenities.AsQueryable();

            // Search
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(a => a.AmenityName!.Contains(searchTerm) ||
                                        a.Description!.Contains(searchTerm));
            }

            // Category filter
            if (!string.IsNullOrEmpty(categoryFilter) && categoryFilter != "All")
            {
                query = query.Where(a => a.Category == categoryFilter);
            }

            // Status filter
            if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "All")
            {
                bool isActive = statusFilter == "Active";
                query = query.Where(a => a.IsActive == isActive);
            }

            var totalAmenities = await query.CountAsync();

            var amenities = await query
                .OrderBy(a => a.AmenityName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var viewModel = new AmenityListViewModel
            {
                Amenities = amenities,
                TotalAmenities = totalAmenities,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)totalAmenities / pageSize),
                SearchTerm = searchTerm,
                CategoryFilter = categoryFilter,
                StatusFilter = statusFilter,
                Categories = await _context.Amenities
                    .Where(a => !string.IsNullOrEmpty(a.Category))
                    .Select(a => a.Category!)
                    .Distinct()
                    .ToListAsync()
            };

            return View(viewModel);
        }

        // GET: Amenities/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var amenity = await _context.Amenities
                .Include(a => a.RoomAmenities!)
                    .ThenInclude(ra => ra.Room)
                .FirstOrDefaultAsync(a => a.AmenityID == id);

            if (amenity == null)
            {
                return NotFound();
            }

            return View(amenity);
        }

        // GET: Amenities/Create
        public IActionResult Create()
        {
            var viewModel = new AmenityViewModel
            {
                IsActive = true,
                Categories = GetAmenityCategories()
            };

            return View(viewModel);
        }

        // POST: Amenities/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AmenityViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var amenity = new Amenity
                {
                    AmenityName = viewModel.AmenityName,
                    Description = viewModel.Description,
                    Category = viewModel.Category,
                    Icon = viewModel.Icon,
                    IsActive = viewModel.IsActive,
                    CreatedBy = User.Identity?.Name ?? "System",
                    CreatedDate = DateTime.Now
                };

                _context.Amenities.Add(amenity);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Amenity created successfully.";
                return RedirectToAction(nameof(Index));
            }

            viewModel.Categories = GetAmenityCategories();
            return View(viewModel);
        }

        // GET: Amenities/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var amenity = await _context.Amenities.FindAsync(id);
            if (amenity == null)
            {
                return NotFound();
            }

            var viewModel = new AmenityViewModel
            {
                AmenityID = amenity.AmenityID,
                AmenityName = amenity.AmenityName ?? "",
                Description = amenity.Description ?? "",
                Category = amenity.Category ?? "",
                Icon = amenity.Icon ?? "",
                IsActive = amenity.IsActive,
                Categories = GetAmenityCategories()
            };

            return View(viewModel);
        }

        // POST: Amenities/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AmenityViewModel viewModel)
        {
            if (id != viewModel.AmenityID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var amenity = await _context.Amenities.FindAsync(id);
                    if (amenity == null)
                    {
                        return NotFound();
                    }

                    amenity.AmenityName = viewModel.AmenityName;
                    amenity.Description = viewModel.Description;
                    amenity.Category = viewModel.Category;
                    amenity.Icon = viewModel.Icon;
                    amenity.IsActive = viewModel.IsActive;
                    amenity.ModifiedBy = User.Identity?.Name ?? "System";
                    amenity.ModifiedDate = DateTime.Now;

                    _context.Update(amenity);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Amenity updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AmenityExists(viewModel.AmenityID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            viewModel.Categories = GetAmenityCategories();
            return View(viewModel);
        }

        // GET: Amenities/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var amenity = await _context.Amenities
                .Include(a => a.RoomAmenities!)
                    .ThenInclude(ra => ra.Room)
                .FirstOrDefaultAsync(a => a.AmenityID == id);

            if (amenity == null)
            {
                return NotFound();
            }

            return View(amenity);
        }

        // POST: Amenities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var amenity = await _context.Amenities.FindAsync(id);
            if (amenity != null)
            {
                // Check if amenity is used by any rooms
                var isUsed = await _context.RoomAmenities.AnyAsync(ra => ra.AmenityID == id);
                if (isUsed)
                {
                    TempData["ErrorMessage"] = "Cannot delete amenity because it is being used by rooms.";
                    return RedirectToAction(nameof(Index));
                }

                _context.Amenities.Remove(amenity);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Amenity deleted successfully.";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Amenities/ToggleStatus/5
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var amenity = await _context.Amenities.FindAsync(id);
            if (amenity == null)
            {
                return Json(new { success = false, message = "Amenity not found" });
            }

            amenity.IsActive = !amenity.IsActive;
            amenity.ModifiedBy = User.Identity?.Name ?? "System";
            amenity.ModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();

            return Json(new { success = true, isActive = amenity.IsActive });
        }

        // GET: Amenities/AssignToRoom/5
        public async Task<IActionResult> AssignToRoom(int id)
        {
            var amenity = await _context.Amenities.FindAsync(id);
            if (amenity == null)
            {
                return NotFound();
            }

            var assignedRoomIds = await _context.RoomAmenities
                .Where(ra => ra.AmenityID == id)
                .Select(ra => ra.RoomID)
                .ToListAsync();

            var viewModel = new AssignAmenityViewModel
            {
                AmenityID = amenity.AmenityID,
                AmenityName = amenity.AmenityName,
                Rooms = await _context.Rooms
                    .Include(r => r.RoomType)
                    .Where(r => r.IsActive)
                    .ToListAsync(),
                AssignedRoomIds = assignedRoomIds
            };

            return View(viewModel);
        }

        // POST: Amenities/AssignToRoom/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignToRoom(int id, AssignAmenityViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Remove existing assignments
                    var existingAssignments = await _context.RoomAmenities
                        .Where(ra => ra.AmenityID == id)
                        .ToListAsync();
                    _context.RoomAmenities.RemoveRange(existingAssignments);

                    // Add new assignments
                    if (viewModel.SelectedRoomIds != null)
                    {
                        foreach (var roomId in viewModel.SelectedRoomIds)
                        {
                            _context.RoomAmenities.Add(new RoomAmenity
                            {
                                RoomID = roomId,
                                AmenityID = id
                            });
                        }
                    }

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Amenity assignments updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred: " + ex.Message);
                }
            }

            var amenity = await _context.Amenities.FindAsync(id);
            viewModel.AmenityName = amenity?.AmenityName;
            viewModel.Rooms = await _context.Rooms
                .Include(r => r.RoomType)
                .Where(r => r.IsActive)
                .ToListAsync();

            return View(viewModel);
        }

        private bool AmenityExists(int id)
        {
            return _context.Amenities.Any(e => e.AmenityID == id);
        }

        private List<string> GetAmenityCategories()
        {
            return new List<string>
            {
                "Basic",
                "Entertainment",
                "Business",
                "Wellness",
                "Kitchen",
                "Bathroom",
                "Connectivity",
                "Safety",
                "Accessibility",
                "Outdoor"
            };
        }
    }
}
