using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Data;
using HotelBooking.Models;
using HotelBooking.Models.ViewModels;

namespace HotelBooking.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SettingsController : Controller
    {
        private readonly HotelBookingContext _context;

        public SettingsController(HotelBookingContext context)
        {
            _context = context;
        }

        // GET: Settings
        public async Task<IActionResult> Index()
        {
            var viewModel = new SettingsViewModel
            {
                // System Settings
                TotalUsers = await _context.Users.CountAsync(),
                TotalRooms = await _context.Rooms.CountAsync(),
                TotalReservations = await _context.Reservations.CountAsync(),
                TotalRevenue = (await _context.Reservations
                    .Where(r => r.Status == "Confirmed")
                    .Include(r => r.Room)
                    .ToListAsync())
                    .Sum(r => (r.Room?.Price ?? 0) * (decimal)(r.CheckOutDate - r.CheckInDate).Days),

                // Room Types
                RoomTypes = await _context.RoomTypes.Where(rt => rt.IsActive).ToListAsync(),

                // Countries and States
                Countries = await _context.Countries.Where(c => c.IsActive).ToListAsync(),
                States = await _context.States.Where(s => s.IsActive).ToListAsync(),

                // Loyalty Tiers
                LoyaltyTiers = await _context.LoyaltyTiers.Where(lt => lt.IsActive).ToListAsync(),

                // Services
                Services = await _context.Services.Where(s => s.IsActive).ToListAsync()
            };

            return View(viewModel);
        }

        // GET: Settings/RoomTypes
        public async Task<IActionResult> RoomTypes()
        {
            var roomTypes = await _context.RoomTypes.ToListAsync();
            return View(roomTypes);
        }

        // POST: Settings/CreateRoomType
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRoomType(string typeName, string description, int maxOccupancy, string accessibilityFeatures)
        {
            try
            {
                if (!string.IsNullOrEmpty(typeName))
                {
                    var roomType = new RoomType
                    {
                        TypeName = typeName,
                        Description = description,
                        MaxOccupancy = maxOccupancy,
                        AccessibilityFeatures = accessibilityFeatures,
                        IsActive = true,
                        CreatedBy = User.Identity?.Name ?? "System",
                        CreatedDate = DateTime.Now
                    };

                    _context.RoomTypes.Add(roomType);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = "Room type created successfully." });
                }
                return Json(new { success = false, message = "Type name is required." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Settings/UpdateRoomType
        [HttpPost]
        public async Task<IActionResult> UpdateRoomType(int roomTypeID, string typeName, string description, int maxOccupancy, string accessibilityFeatures)
        {
            try
            {
                var roomType = await _context.RoomTypes.FindAsync(roomTypeID);
                if (roomType == null)
                {
                    return Json(new { success = false, message = "Room type not found." });
                }

                roomType.TypeName = typeName;
                roomType.Description = description;
                roomType.MaxOccupancy = maxOccupancy;
                roomType.AccessibilityFeatures = accessibilityFeatures;
                roomType.ModifiedBy = User.Identity?.Name ?? "System";
                roomType.ModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Room type updated successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Settings/ToggleRoomTypeStatus
        [HttpPost]
        public async Task<IActionResult> ToggleRoomTypeStatus(int id)
        {
            try
            {
                var roomType = await _context.RoomTypes.FindAsync(id);
                if (roomType == null)
                {
                    return Json(new { success = false, message = "Room type not found." });
                }

                roomType.IsActive = !roomType.IsActive;
                roomType.ModifiedBy = User.Identity?.Name ?? "System";
                roomType.ModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Status updated successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: Settings/Countries
        public async Task<IActionResult> Countries()
        {
            var countries = await _context.Countries.ToListAsync();
            return View(countries);
        }

        // POST: Settings/CreateCountry
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCountry(string countryName, string countryCode)
        {
            if (!string.IsNullOrEmpty(countryName))
            {
                var country = new Country
                {
                    CountryName = countryName,
                    CountryCode = countryCode,
                    IsActive = true,
                    CreatedBy = User.Identity?.Name ?? "System",
                    CreatedDate = DateTime.Now
                };

                _context.Countries.Add(country);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Country created successfully.";
            }

            return RedirectToAction(nameof(Countries));
        }

        // GET: Settings/LoyaltyTiers
        public async Task<IActionResult> LoyaltyTiers()
        {
            var loyaltyTiers = await _context.LoyaltyTiers.ToListAsync();
            return View(loyaltyTiers);
        }

        // POST: Settings/CreateLoyaltyTier
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLoyaltyTier(string tierName, int minPoints, int maxPoints, decimal discountPercentage)
        {
            if (!string.IsNullOrEmpty(tierName))
            {
                var loyaltyTier = new LoyaltyTier
                {
                    TierName = tierName,
                    MinPoints = minPoints,
                    MaxPoints = maxPoints,
                    DiscountPercentage = discountPercentage,
                    IsActive = true,
                    CreatedBy = User.Identity?.Name ?? "System",
                    CreatedDate = DateTime.Now
                };

                _context.LoyaltyTiers.Add(loyaltyTier);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Loyalty tier created successfully.";
            }

            return RedirectToAction(nameof(LoyaltyTiers));
        }

        // GET: Settings/Services
        public async Task<IActionResult> Services()
        {
            var services = await _context.Services.ToListAsync();
            return View(services);
        }

        // POST: Settings/CreateService
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateService(string serviceName, string description, string category)
        {
            if (!string.IsNullOrEmpty(serviceName))
            {
                var service = new Service
                {
                    ServiceName = serviceName,
                    Description = description,
                    Category = category,
                    IsActive = true,
                    CreatedBy = User.Identity?.Name ?? "System",
                    CreatedDate = DateTime.Now
                };

                _context.Services.Add(service);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Service created successfully.";
            }

            return RedirectToAction(nameof(Services));
        }

        // GET: Settings/EditService/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            return View(service);
        }

        // POST: Settings/EditService/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditService(int id, Service model)
        {
            if (id != model.ServiceID)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var service = await _context.Services.FindAsync(id);
                if (service == null)
                {
                    return NotFound();
                }
                service.ServiceName = model.ServiceName;
                service.Description = model.Description;
                service.Category = model.Category;
                service.IsActive = model.IsActive;
                service.ModifiedBy = User.Identity?.Name ?? "System";
                service.ModifiedDate = DateTime.Now;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Service updated successfully.";
                return RedirectToAction(nameof(Services));
            }
            return View(model);
        }

        // POST: Settings/ToggleStatus
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(string entityType, int id)
        {
            try
            {
                switch (entityType.ToLower())
                {
                    case "roomtype":
                        var roomType = await _context.RoomTypes.FindAsync(id);
                        if (roomType != null)
                        {
                            roomType.IsActive = !roomType.IsActive;
                            roomType.ModifiedBy = User.Identity?.Name;
                            roomType.ModifiedDate = DateTime.Now;
                        }
                        break;

                    case "country":
                        var country = await _context.Countries.FindAsync(id);
                        if (country != null)
                        {
                            country.IsActive = !country.IsActive;
                            country.ModifiedBy = User.Identity?.Name;
                            country.ModifiedDate = DateTime.Now;
                        }
                        break;

                    case "loyaltytier":
                        var loyaltyTier = await _context.LoyaltyTiers.FindAsync(id);
                        if (loyaltyTier != null)
                        {
                            loyaltyTier.IsActive = !loyaltyTier.IsActive;
                        }
                        break;

                    case "service":
                        var service = await _context.Services.FindAsync(id);
                        if (service != null)
                        {
                            service.IsActive = !service.IsActive;
                            service.ModifiedBy = User.Identity?.Name;
                            service.ModifiedDate = DateTime.Now;
                        }
                        break;
                }

                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: Settings/SystemInfo
        public async Task<IActionResult> SystemInfo()
        {
            var confirmedReservations = await _context.Reservations
                .Where(r => r.Status == "Confirmed")
                .Join(_context.Rooms, r => r.RoomID, room => room.RoomID, (r, room) => new { r, room })
                .ToListAsync();

            var totalRevenue = confirmedReservations
                .Sum(x => x.room.Price * (x.r.CheckOutDate - x.r.CheckInDate).Days);

            var systemInfo = new
            {
                DatabaseTables = new
                {
                    Users = await _context.Users.CountAsync(),
                    Rooms = await _context.Rooms.CountAsync(),
                    Reservations = await _context.Reservations.CountAsync(),
                    Payments = await _context.Payments.CountAsync(),
                    Amenities = await _context.Amenities.CountAsync(),
                    RoomTypes = await _context.RoomTypes.CountAsync(),
                    Countries = await _context.Countries.CountAsync(),
                    States = await _context.States.CountAsync(),
                    Services = await _context.Services.CountAsync(),
                    LoyaltyTiers = await _context.LoyaltyTiers.CountAsync()
                },
                SystemStats = new
                {
                    TotalRevenue = totalRevenue,
                    ActiveUsers = await _context.Users.CountAsync(u => u.IsActive),
                    AvailableRooms = await _context.Rooms.CountAsync(r => r.Status == "Available"),
                    PendingReservations = await _context.Reservations.CountAsync(r => r.Status == "Pending")
                }
            };

            return View(systemInfo);
        }
    }
}
