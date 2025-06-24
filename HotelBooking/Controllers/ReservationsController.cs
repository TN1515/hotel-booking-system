using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Data;
using HotelBooking.Models;
using HotelBooking.Models.ViewModels;
using HotelBooking.Services;
using Microsoft.AspNetCore.Authorization;

namespace HotelBooking.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {
        private readonly HotelBookingContext _context;
        private readonly IEmailService _emailService;
        private readonly ILoyaltyService _loyaltyService;
        private readonly IPdfService _pdfService;

        public ReservationsController(HotelBookingContext context, IEmailService emailService, ILoyaltyService loyaltyService, IPdfService pdfService)
        {
            _context = context;
            _emailService = emailService;
            _loyaltyService = loyaltyService;
            _pdfService = pdfService;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Forbid();
            }
            
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
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId) || reservation.UserID != userId)
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
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId) || reservation.UserID != userId)
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
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId) || reservation.UserID != userId)
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
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId) || reservation.UserID != userId)
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
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out var userId) || reservation.UserID != userId)
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

        // GET: Reservations/ChangeRoom/5
        public async Task<IActionResult> ChangeRoom(int? id)
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
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId) || reservation.UserID != userId)
            {
                return Forbid();
            }

            // Check if reservation is active and can be changed
            if (reservation.Status != "Confirmed" || reservation.CheckOutDate <= DateTime.Today)
            {
                TempData["Error"] = "Room change is not available for this reservation.";
                return RedirectToAction("Details", new { id = reservation.ReservationID });
            }

            // Get available rooms for the remaining period
            var changeDate = DateTime.Today > reservation.CheckInDate ? DateTime.Today : reservation.CheckInDate;
            var availableRooms = await GetAvailableRooms(changeDate, reservation.CheckOutDate, reservation.RoomID);

            var viewModel = new RoomChangeViewModel
            {
                ReservationID = reservation.ReservationID,
                CurrentRoomID = reservation.RoomID,
                CurrentRoomNumber = reservation.Room?.RoomNumber,
                CurrentRoomType = reservation.Room?.RoomType?.TypeName,
                CurrentRoomPrice = reservation.Room?.Price ?? 0,
                CheckInDate = reservation.CheckInDate,
                CheckOutDate = reservation.CheckOutDate,
                ChangeDate = changeDate,
                AvailableRooms = availableRooms
            };

            return View(viewModel);
        }

        // POST: Reservations/ChangeRoom
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeRoom(RoomChangeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var reservation = await _context.Reservations
                    .Include(r => r.Room)
                    .FirstOrDefaultAsync(r => r.ReservationID == model.ReservationID);

                if (reservation == null)
                {
                    return NotFound();
                }

                // Check if user owns this reservation
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out var userId) || reservation.UserID != userId)
                {
                    return Forbid();
                }

                // Get new room details
                var newRoom = await _context.Rooms
                    .Include(r => r.RoomType)
                    .FirstOrDefaultAsync(r => r.RoomID == model.NewRoomID);

                if (newRoom == null)
                {
                    ModelState.AddModelError("", "Selected room not found.");
                    model.AvailableRooms = await GetAvailableRooms(model.ChangeDate, model.CheckOutDate, model.CurrentRoomID);
                    return View(model);
                }

                // Calculate price difference
                var remainingNights = (model.CheckOutDate - model.ChangeDate).Days;
                var oldRoomCost = (reservation.Room?.Price ?? 0) * remainingNights;
                var newRoomCost = newRoom.Price * remainingNights;
                var priceDifference = newRoomCost - oldRoomCost;

                // Create room change history record
                var roomChange = new RoomChangeHistory
                {
                    ReservationID = reservation.ReservationID,
                    OldRoomID = reservation.RoomID,
                    NewRoomID = model.NewRoomID,
                    ChangeDate = model.ChangeDate,
                    Reason = model.Reason,
                    OldRoomPrice = reservation.Room?.Price ?? 0,
                    NewRoomPrice = newRoom.Price,
                    PriceDifference = priceDifference,
                    Status = "Approved", // Auto-approve for customers
                    CreatedBy = User.Identity?.Name ?? "Customer",
                    CreatedDate = DateTime.Now,
                    ApprovedBy = User.Identity?.Name ?? "Customer",
                    ApprovedDate = DateTime.Now
                };

                _context.RoomChangeHistories.Add(roomChange);

                // Update reservation
                reservation.RoomID = model.NewRoomID;
                reservation.ModifiedBy = User.Identity?.Name ?? "Customer";
                reservation.ModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync();

                // Send email notification
                var user = await _context.Users.FindAsync(reservation.UserID);
                if (user != null)
                {
                    await _emailService.SendRoomChangeNotificationAsync(
                        user.Email ?? "",
                        user.UserName ?? "",
                        reservation.Room?.RoomNumber ?? "",
                        newRoom.RoomNumber ?? "",
                        model.ChangeDate,
                        priceDifference);
                }

                TempData["Message"] = $"Room changed successfully! {(priceDifference > 0 ? $"Additional cost: ${priceDifference:F2}" : priceDifference < 0 ? $"Refund: ${Math.Abs(priceDifference):F2}" : "No price difference")}";
                return RedirectToAction("Details", new { id = reservation.ReservationID });
            }

            model.AvailableRooms = await GetAvailableRooms(model.ChangeDate, model.CheckOutDate, model.CurrentRoomID);
            return View(model);
        }

        // GET: Reservations/AddService/5
        public async Task<IActionResult> AddService(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Room)
                .FirstOrDefaultAsync(m => m.ReservationID == id);

            if (reservation == null)
            {
                return NotFound();
            }

            // Check if user owns this reservation
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId) || reservation.UserID != userId)
            {
                return Forbid();
            }

            // Check if reservation is active
            if (reservation.Status != "Confirmed" || reservation.CheckOutDate <= DateTime.Today)
            {
                TempData["Error"] = "Cannot add services to this reservation.";
                return RedirectToAction("Details", new { id = reservation.ReservationID });
            }

            var availableServices = await _context.Services
                .Where(s => s.IsActive)
                .OrderBy(s => s.Category)
                .ThenBy(s => s.ServiceName)
                .ToListAsync();

            var viewModel = new AddServiceViewModel
            {
                ReservationID = reservation.ReservationID,
                RoomNumber = reservation.Room?.RoomNumber,
                CheckInDate = reservation.CheckInDate,
                CheckOutDate = reservation.CheckOutDate,
                AvailableServices = availableServices
            };

            return View(viewModel);
        }

        // POST: Reservations/AddService
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddService(AddServiceViewModel model)
        {
            if (ModelState.IsValid)
            {
                var reservation = await _context.Reservations.FindAsync(model.ReservationID);
                var service = await _context.Services.FindAsync(model.ServiceID);

                if (reservation == null || service == null)
                {
                    return NotFound();
                }

                // Check if user owns this reservation
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out var userId) || reservation.UserID != userId)
                {
                    return Forbid();
                }

                var totalPrice = service.UnitPrice * model.Quantity;

                var serviceUsage = new BookingServiceUsage
                {
                    ReservationID = model.ReservationID,
                    ServiceID = model.ServiceID,
                    Quantity = model.Quantity,
                    UnitPrice = service.UnitPrice,
                    TotalPrice = totalPrice,
                    UsageDate = DateTime.Now,
                    Note = model.Note,
                    Status = "Ordered",
                    CreatedBy = User.Identity?.Name ?? "Customer",
                    CreatedDate = DateTime.Now
                };

                _context.BookingServiceUsages.Add(serviceUsage);
                await _context.SaveChangesAsync();

                // Add loyalty points for service usage
                var points = await _loyaltyService.CalculatePointsForService(totalPrice);
                await _loyaltyService.AddPointsAsync(
                    reservation.UserID,
                    points,
                    "Service",
                    totalPrice,
                    $"Points for {service.ServiceName}",
                    reservation.ReservationID,
                    serviceUsage.BookingServiceUsageID);

                // Send email notification
                var user = await _context.Users.FindAsync(reservation.UserID);
                if (user != null)
                {
                    await _emailService.SendServiceAddedNotificationAsync(
                        user.Email ?? "",
                        user.UserName ?? "",
                        service.ServiceName ?? "",
                        model.Quantity,
                        totalPrice);
                }

                // Update inventory
                await UpdateServiceInventoryAsync(service.ServiceID, model.Quantity);

                TempData["Message"] = $"Service '{service.ServiceName}' added successfully! Total cost: ${totalPrice:F2}. You earned {points} loyalty points!";
                return RedirectToAction("Details", new { id = reservation.ReservationID });
            }

            model.AvailableServices = await _context.Services
                .Where(s => s.IsActive)
                .OrderBy(s => s.Category)
                .ThenBy(s => s.ServiceName)
                .ToListAsync();

            return View(model);
        }

        // GET: Reservations/Checkout/5
        public async Task<IActionResult> Checkout(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Room)
                    .ThenInclude(rm => rm!.RoomType)
                .Include(r => r.Payments)
                .FirstOrDefaultAsync(m => m.ReservationID == id);

            if (reservation == null)
            {
                return NotFound();
            }

            // Check if user owns this reservation
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId) || reservation.UserID != userId)
            {
                return Forbid();
            }

            // Get services used
            var servicesUsed = await _context.BookingServiceUsages
                .Include(s => s.Service)
                .Where(s => s.ReservationID == id)
                .ToListAsync();

            // Get room changes
            var roomChanges = await _context.RoomChangeHistories
                .Include(rc => rc.OldRoom)
                .Include(rc => rc.NewRoom)
                .Where(rc => rc.ReservationID == id)
                .ToListAsync();

            // Calculate costs
            var numberOfNights = (reservation.CheckOutDate - reservation.CheckInDate).Days;
            var roomCost = (reservation.Room?.Price ?? 0) * numberOfNights;
            var serviceCost = servicesUsed.Sum(s => s.TotalPrice);
            var roomChangeCost = roomChanges.Sum(rc => rc.PriceDifference);
            var taxAmount = (roomCost + serviceCost + roomChangeCost) * 0.1m; // 10% tax
            var totalCost = roomCost + serviceCost + roomChangeCost + taxAmount;

            var paidAmount = reservation.Payments?.Sum(p => p.Amount) ?? 0;

            var viewModel = new CheckoutViewModel
            {
                ReservationID = reservation.ReservationID,
                RoomNumber = reservation.Room?.RoomNumber,
                RoomType = reservation.Room?.RoomType?.TypeName,
                CheckInDate = reservation.CheckInDate,
                CheckOutDate = reservation.CheckOutDate,
                NumberOfNights = numberOfNights,
                RoomCost = roomCost + roomChangeCost,
                ServiceCost = serviceCost,
                TaxAmount = taxAmount,
                TotalCost = totalCost,
                ServicesUsed = servicesUsed,
                RoomChanges = roomChanges,
                PaymentStatus = reservation.Status,
                PaidAmount = paidAmount,
                RemainingAmount = totalCost - paidAmount
            };

            return View(viewModel);
        }

        // POST: Reservations/Checkout/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckoutConfirmed(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Room)
                    .ThenInclude(r => r!.RoomType)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.ReservationID == id);

            if (reservation == null)
            {
                return NotFound();
            }

            // Check if user owns this reservation
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId) || reservation.UserID != userId)
            {
                return Forbid();
            }

            // Calculate total amount for loyalty points
            var servicesUsed = await _context.BookingServiceUsages
                .Where(s => s.ReservationID == id)
                .ToListAsync();

            var roomChanges = await _context.RoomChangeHistories
                .Where(rc => rc.ReservationID == id)
                .ToListAsync();

            var numberOfNights = (reservation.CheckOutDate - reservation.CheckInDate).Days;
            var roomCost = (reservation.Room?.Price ?? 0) * numberOfNights;
            var serviceCost = servicesUsed.Sum(s => s.TotalPrice);
            var roomChangeCost = roomChanges.Sum(rc => rc.PriceDifference);
            var totalAmount = roomCost + serviceCost + roomChangeCost;

            // Add loyalty points for room booking
            var roomPoints = await _loyaltyService.CalculatePointsForRoomBooking(roomCost);
            await _loyaltyService.AddPointsAsync(
                reservation.UserID,
                roomPoints,
                "Room",
                roomCost,
                $"Points for room booking - {reservation.Room?.RoomNumber}",
                reservation.ReservationID);

            // Update reservation status
            reservation.Status = "Checked Out";
            reservation.ModifiedBy = User.Identity?.Name ?? "Customer";
            reservation.ModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();

            // Generate and send receipt
            var checkoutData = new CheckoutViewModel
            {
                ReservationID = reservation.ReservationID,
                RoomNumber = reservation.Room?.RoomNumber,
                RoomType = reservation.Room?.RoomType?.TypeName,
                CheckInDate = reservation.CheckInDate,
                CheckOutDate = reservation.CheckOutDate,
                NumberOfNights = numberOfNights,
                RoomCost = roomCost + roomChangeCost,
                ServiceCost = serviceCost,
                TaxAmount = totalAmount * 0.1m,
                TotalCost = totalAmount * 1.1m,
                ServicesUsed = servicesUsed,
                RoomChanges = roomChanges
            };

            var receiptHtml = await _pdfService.GenerateInvoiceHtmlAsync(checkoutData);

            if (reservation.User != null)
            {
                await _emailService.SendCheckoutReceiptAsync(
                    reservation.User.Email ?? "",
                    reservation.User.UserName ?? "",
                    receiptHtml,
                    checkoutData.TotalCost);
            }

            TempData["Message"] = $"Checkout completed successfully! You earned {roomPoints} loyalty points. Receipt sent to your email.";
            return RedirectToAction("Index");
        }

        // Helper method to get available rooms
        private async Task<List<Room>> GetAvailableRooms(DateTime startDate, DateTime endDate, int excludeRoomId)
        {
            var bookedRoomIds = await _context.Reservations
                .Where(res => res.CheckInDate < endDate && res.CheckOutDate > startDate && res.Status == "Confirmed")
                .Select(res => res.RoomID)
                .ToListAsync();

            return await _context.Rooms
                .Include(r => r.RoomType)
                .Where(r => r.IsActive && r.Status == "Available" &&
                           !bookedRoomIds.Contains(r.RoomID) && r.RoomID != excludeRoomId)
                .OrderBy(r => r.RoomType!.TypeName)
                .ThenBy(r => r.Price)
                .ToListAsync();
        }

        // Helper method to update service inventory
        private async Task UpdateServiceInventoryAsync(int serviceId, int quantityUsed)
        {
            var inventory = await _context.ServiceInventories
                .FirstOrDefaultAsync(si => si.ServiceID == serviceId);

            if (inventory != null)
            {
                var previousStock = inventory.CurrentStock;
                inventory.CurrentStock -= quantityUsed;
                inventory.ModifiedBy = User.Identity?.Name ?? "System";
                inventory.ModifiedDate = DateTime.Now;

                // Update status based on stock level
                if (inventory.CurrentStock <= 0)
                {
                    inventory.Status = "Out of Stock";
                }
                else if (inventory.CurrentStock <= inventory.ReorderLevel)
                {
                    inventory.Status = "Low Stock";
                }
                else
                {
                    inventory.Status = "In Stock";
                }

                // Create inventory transaction record
                var transaction = new InventoryTransaction
                {
                    ServiceInventoryID = inventory.ServiceInventoryID,
                    TransactionType = "Out",
                    Quantity = quantityUsed,
                    PreviousStock = previousStock,
                    NewStock = inventory.CurrentStock,
                    Reason = "Service usage by customer",
                    Reference = $"Booking Service Usage",
                    TransactionDate = DateTime.Now,
                    CreatedBy = User.Identity?.Name ?? "System",
                    CreatedDate = DateTime.Now
                };

                _context.InventoryTransactions.Add(transaction);
                await _context.SaveChangesAsync();
            }
        }
    }
}
