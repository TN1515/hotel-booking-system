using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Data;
using HotelBooking.Models;
using HotelBooking.Models.ViewModels;
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
                .Include(r => r.RoomImages)
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
                .Include(r => r.RoomAmenities!)
                    .ThenInclude(ra => ra.Amenity)
                .Include(r => r.RoomImages!)
                .FirstOrDefaultAsync(m => m.RoomID == id);

            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // GET: Rooms/Available
        public async Task<IActionResult> Available(DateTime? checkIn, DateTime? checkOut, int? guests, int? roomTypeId)
        {
            var query = _context.Rooms
                .Include(r => r.RoomType)
                .Include(r => r.RoomImages)
                .Where(r => r.IsActive && r.Status == "Available");

            if (checkIn.HasValue && checkOut.HasValue)
            {
                // Check for rooms that are not booked during the specified period
                // Only consider confirmed reservations that haven't been cancelled
                var bookedRoomIds = await _context.Reservations
                    .Where(res => res.CheckInDate < checkOut &&
                                 res.CheckOutDate > checkIn &&
                                 res.Status != "Cancelled" &&
                                 res.Status != "Pending")
                    .Select(res => res.RoomID)
                    .ToListAsync();

                query = query.Where(r => !bookedRoomIds.Contains(r.RoomID));
            }

            if (guests.HasValue)
            {
                query = query.Where(r => r.RoomType!.MaxOccupancy >= guests);
            }

            if (roomTypeId.HasValue)
            {
                query = query.Where(r => r.RoomTypeID == roomTypeId.Value);
            }

            var availableRooms = await query.ToListAsync();

            ViewBag.CheckIn = checkIn?.ToString("yyyy-MM-dd");
            ViewBag.CheckOut = checkOut?.ToString("yyyy-MM-dd");
            ViewBag.Guests = guests;
            ViewBag.RoomTypeId = roomTypeId;

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
                .Include(r => r.RoomImages)
                .Where(r => r.IsActive && r.Status == "Available");

            // Check availability - only consider confirmed reservations that haven't been cancelled
            var bookedRoomIds = await _context.Reservations
                .Where(res => res.CheckInDate < checkOut &&
                             res.CheckOutDate > checkIn &&
                             res.Status != "Cancelled" &&
                             res.Status != "Pending")
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

            // Check if room is available for the specified dates
            if (checkIn.HasValue && checkOut.HasValue)
            {
                var isAvailable = !await _context.Reservations
                    .AnyAsync(res => res.RoomID == id &&
                                   res.CheckInDate < checkOut &&
                                   res.CheckOutDate > checkIn &&
                                   res.Status != "Cancelled" &&
                                   res.Status != "Pending");

                if (!isAvailable)
                {
                    TempData["Error"] = $"Sorry, Room {room.RoomNumber} is no longer available for the selected dates ({checkIn:MMM dd} - {checkOut:MMM dd}). Please choose different dates or another room.";
                    return RedirectToAction("Available", new { checkIn, checkOut, guests });
                }
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
        [Authorize(Roles = "Customer,Admin,Staff")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book(BookingViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Get current user ID
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    return RedirectToAction("Login", "Account");
                }

                // Verify user exists in database
                var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
                if (!userExists)
                {
                    return RedirectToAction("Login", "Account");
                }

                // Check if room is still available - only consider confirmed reservations
                var isAvailable = !await _context.Reservations
                    .AnyAsync(res => res.RoomID == model.RoomID &&
                                   res.CheckInDate < model.CheckOutDate &&
                                   res.CheckOutDate > model.CheckInDate &&
                                   res.Status != "Cancelled" &&
                                   res.Status != "Pending");

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
                    UserID = userId,
                    RoomID = model.RoomID,
                    BookingDate = DateTime.Now,
                    CheckInDate = model.CheckInDate,
                    CheckOutDate = model.CheckOutDate,
                    NumberOfGuests = model.NumberOfGuests,
                    Status = "Pending Payment", // Changed to require payment first
                    CreatedBy = User.Identity?.Name ?? "System",
                    CreatedDate = DateTime.Now
                };

                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();

                // Calculate total amount
                var room = await _context.Rooms.FindAsync(model.RoomID);
                var nights = (model.CheckOutDate - model.CheckInDate).Days;
                var totalAmount = (room?.Price ?? 0) * nights;

                // Create notification for booking
                var bookingNotification = new Notification
                {
                    UserID = userId,
                    Title = "Booking Created",
                    Message = $"Your reservation for {room?.RoomNumber} from {model.CheckInDate:MMM dd} to {model.CheckOutDate:MMM dd} has been created. Please complete payment to confirm.",
                    Type = "Booking",
                    Status = "Sent",
                    CreatedDate = DateTime.Now,
                    SentDate = DateTime.Now,
                    IsRead = false,
                    CreatedBy = "System"
                };
                _context.Notifications.Add(bookingNotification);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Reservation created! Please complete payment to confirm your booking.";
                return RedirectToAction("Generate", "QRPayment", new { reservationId = reservation.ReservationID, amount = totalAmount });
            }

            model.Room = await _context.Rooms
                .Include(r => r.RoomType)
                .FirstOrDefaultAsync(r => r.RoomID == model.RoomID);
            return View(model);
        }

        // POST: Rooms/BookMultiple
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookMultiple(List<int> roomIds)
        {
            if (roomIds == null || !roomIds.Any())
            {
                TempData["Error"] = "No rooms selected for booking.";
                return RedirectToAction("Index");
            }

            // Get selected rooms
            var rooms = await _context.Rooms
                .Include(r => r.RoomType)
                .Include(r => r.RoomImages)
                .Where(r => roomIds.Contains(r.RoomID) && r.IsActive && r.Status == "Available")
                .ToListAsync();

            if (!rooms.Any())
            {
                TempData["Error"] = "Selected rooms are not available.";
                return RedirectToAction("Index");
            }

            // Create view model for multiple booking
            var viewModel = new MultipleBookingViewModel
            {
                SelectedRooms = rooms,
                CheckInDate = DateTime.Today.AddDays(1),
                CheckOutDate = DateTime.Today.AddDays(2),
                NumberOfGuests = 2
            };

            return View(viewModel);
        }

        // POST: Rooms/ConfirmMultipleBooking
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmMultipleBooking(MultipleBookingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
                var createdReservations = new List<int>();

                foreach (var roomId in model.SelectedRoomIds)
                {
                    // Check if room is still available - only consider confirmed reservations
                    var isAvailable = !await _context.Reservations
                        .AnyAsync(res => res.RoomID == roomId &&
                                       res.CheckInDate < model.CheckOutDate &&
                                       res.CheckOutDate > model.CheckInDate &&
                                       res.Status != "Cancelled" &&
                                       res.Status != "Pending");

                    if (isAvailable)
                    {
                        var reservation = new Reservation
                        {
                            UserID = userId,
                            RoomID = roomId,
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
                        createdReservations.Add(reservation.ReservationID);
                    }
                }

                if (createdReservations.Any())
                {
                    TempData["Message"] = $"Successfully booked {createdReservations.Count} room(s)!";
                    return RedirectToAction("Index", "Reservations");
                }
                else
                {
                    TempData["Error"] = "No rooms could be booked. They may no longer be available.";
                }
            }

            // Reload rooms if validation failed
            model.SelectedRooms = await _context.Rooms
                .Include(r => r.RoomType)
                .Where(r => model.SelectedRoomIds.Contains(r.RoomID))
                .ToListAsync();

            return View("BookMultiple", model);
        }

        // GET: Rooms/Create
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Create()
        {
            var viewModel = new RoomViewModel
            {
                IsActive = true,
                Status = "Available",
                RoomTypes = await _context.RoomTypes.Where(rt => rt.IsActive).ToListAsync(),
                Amenities = await _context.Amenities.Where(a => a.IsActive).ToListAsync()
            };

            return View(viewModel);
        }

        // POST: Rooms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Create(RoomViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var room = new Room
                {
                    RoomNumber = viewModel.RoomNumber,
                    RoomTypeID = viewModel.RoomTypeID,
                    Price = viewModel.Price,
                    Status = viewModel.Status,
                    Description = viewModel.Description,
                    BedType = viewModel.BedType,
                    ViewType = viewModel.ViewType,
                    IsActive = viewModel.IsActive,
                    CreatedBy = User.Identity?.Name ?? "System",
                    CreatedDate = DateTime.Now
                };

                _context.Rooms.Add(room);
                await _context.SaveChangesAsync();

                // Handle image uploads
                if (viewModel.RoomImages != null && viewModel.RoomImages.Any())
                {
                    await SaveRoomImages(room.RoomID, viewModel.RoomImages);
                }

                // Add amenities
                if (viewModel.SelectedAmenityIds != null)
                {
                    foreach (var amenityId in viewModel.SelectedAmenityIds)
                    {
                        _context.RoomAmenities.Add(new RoomAmenity
                        {
                            RoomID = room.RoomID,
                            AmenityID = amenityId
                        });
                    }
                    await _context.SaveChangesAsync();
                }

                TempData["SuccessMessage"] = "Room created successfully.";
                return RedirectToAction(nameof(Index));
            }

            viewModel.RoomTypes = await _context.RoomTypes.Where(rt => rt.IsActive).ToListAsync();
            viewModel.Amenities = await _context.Amenities.Where(a => a.IsActive).ToListAsync();
            return View(viewModel);
        }

        private async Task SaveRoomImages(int roomId, List<IFormFile> images)
        {
            int displayOrder = 1;
            foreach (var image in images)
            {
                if (image.Length > 0)
                {
                    // Convert image to byte array
                    using (var memoryStream = new MemoryStream())
                    {
                        await image.CopyToAsync(memoryStream);
                        var imageData = memoryStream.ToArray();

                        // Save to database with binary data
                        var roomImage = new RoomImage
                        {
                            RoomID = roomId,
                            ImageName = image.FileName,
                            ImageData = imageData, // Store binary data
                            IsPrimary = displayOrder == 1, // First image is primary
                            DisplayOrder = displayOrder,
                            IsActive = true,
                            CreatedBy = User.Identity?.Name ?? "System",
                            CreatedDate = DateTime.Now
                        };

                        _context.RoomImages.Add(roomImage);
                        displayOrder++;
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        // GET: Rooms/Edit/5
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Edit(int id)
        {
            var room = await _context.Rooms
                .Include(r => r.RoomAmenities!)
                .Include(r => r.RoomImages!)
                .FirstOrDefaultAsync(r => r.RoomID == id);

            if (room == null)
            {
                return NotFound();
            }

            var viewModel = new RoomViewModel
            {
                RoomID = room.RoomID,
                RoomNumber = room.RoomNumber ?? "",
                RoomTypeID = room.RoomTypeID,
                Price = room.Price,
                Status = room.Status ?? "",
                Description = room.Description ?? "",
                BedType = room.BedType ?? "",
                ViewType = room.ViewType ?? "",
                IsActive = room.IsActive,
                RoomTypes = await _context.RoomTypes.Where(rt => rt.IsActive).ToListAsync(),
                Amenities = await _context.Amenities.Where(a => a.IsActive).ToListAsync(),
                SelectedAmenityIds = room.RoomAmenities?.Select(ra => ra.AmenityID).ToList() ?? new List<int>(),
                ExistingImagePaths = room.RoomImages?.Where(ri => ri.IsActive)
                    .OrderBy(ri => ri.DisplayOrder)
                    .Select(ri => ri.ImagePath ?? "")
                    .Where(path => !string.IsNullOrEmpty(path))
                    .ToList() ?? new List<string>()
            };

            return View(viewModel);
        }

        // POST: Rooms/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Edit(int id, RoomViewModel viewModel, List<string>? RemovedImages)
        {
            if (id != viewModel.RoomID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var room = await _context.Rooms
                        .Include(r => r.RoomAmenities!)
                        .Include(r => r.RoomImages!)
                        .FirstOrDefaultAsync(r => r.RoomID == id);

                    if (room == null)
                    {
                        return NotFound();
                    }

                    room.RoomNumber = viewModel.RoomNumber;
                    room.RoomTypeID = viewModel.RoomTypeID;
                    room.Price = viewModel.Price;
                    room.Status = viewModel.Status;
                    room.Description = viewModel.Description;
                    room.BedType = viewModel.BedType;
                    room.ViewType = viewModel.ViewType;
                    room.IsActive = viewModel.IsActive;
                    room.ModifiedBy = User.Identity?.Name ?? "System";
                    room.ModifiedDate = DateTime.Now;

                    // Handle removed images
                    if (RemovedImages != null && RemovedImages.Any())
                    {
                        await RemoveRoomImages(room.RoomID, RemovedImages);
                    }

                    // Handle new image uploads
                    if (viewModel.RoomImages != null && viewModel.RoomImages.Any())
                    {
                        await SaveRoomImages(room.RoomID, viewModel.RoomImages);
                    }

                    // Update amenities
                    _context.RoomAmenities.RemoveRange(room.RoomAmenities!);
                    if (viewModel.SelectedAmenityIds != null)
                    {
                        foreach (var amenityId in viewModel.SelectedAmenityIds)
                        {
                            _context.RoomAmenities.Add(new RoomAmenity
                            {
                                RoomID = room.RoomID,
                                AmenityID = amenityId
                            });
                        }
                    }

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Room updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(viewModel.RoomID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            viewModel.RoomTypes = await _context.RoomTypes.Where(rt => rt.IsActive).ToListAsync();
            viewModel.Amenities = await _context.Amenities.Where(a => a.IsActive).ToListAsync();
            return View(viewModel);
        }

        private async Task RemoveRoomImages(int roomId, List<string> imagePaths)
        {
            foreach (var imagePath in imagePaths)
            {
                // Remove from database
                var roomImage = await _context.RoomImages
                    .FirstOrDefaultAsync(ri => ri.RoomID == roomId && ri.ImagePath == imagePath);

                if (roomImage != null)
                {
                    _context.RoomImages.Remove(roomImage);

                    // Remove physical file
                    var physicalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath.TrimStart('/'));
                    if (System.IO.File.Exists(physicalPath))
                    {
                        System.IO.File.Delete(physicalPath);
                    }
                }
            }
        }

        // GET: Rooms/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var room = await _context.Rooms
                .Include(r => r.RoomType)
                .Include(r => r.RoomAmenities!)
                    .ThenInclude(ra => ra.Amenity)
                .FirstOrDefaultAsync(r => r.RoomID == id);

            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room != null)
            {
                // Check if room has reservations
                var hasReservations = await _context.Reservations.AnyAsync(r => r.RoomID == id);
                if (hasReservations)
                {
                    TempData["ErrorMessage"] = "Cannot delete room because it has reservations.";
                    return RedirectToAction(nameof(Index));
                }

                // Remove room amenities first
                var roomAmenities = await _context.RoomAmenities.Where(ra => ra.RoomID == id).ToListAsync();
                _context.RoomAmenities.RemoveRange(roomAmenities);

                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Room deleted successfully.";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Rooms/ToggleStatus/5
        [HttpPost]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return Json(new { success = false, message = "Room not found" });
            }

            room.IsActive = !room.IsActive;
            room.ModifiedBy = User.Identity?.Name ?? "System";
            room.ModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();

            return Json(new { success = true, isActive = room.IsActive });
        }

        private bool RoomExists(int id)
        {
            return _context.Rooms.Any(e => e.RoomID == id);
        }

        // AJAX endpoint to check room availability
        [HttpPost]
        public async Task<IActionResult> CheckAvailability([FromBody] AvailabilityCheckRequest request)
        {
            try
            {
                if (request.CheckInDate >= request.CheckOutDate)
                {
                    return Json(new {
                        isAvailable = false,
                        message = "Check-out date must be after check-in date."
                    });
                }

                var isAvailable = !await _context.Reservations
                    .AnyAsync(res => res.RoomID == request.RoomId &&
                                   res.CheckInDate < request.CheckOutDate &&
                                   res.CheckOutDate > request.CheckInDate &&
                                   res.Status != "Cancelled" &&
                                   res.Status != "Pending");

                if (!isAvailable)
                {
                    // Get the conflicting reservation details
                    var conflictingReservation = await _context.Reservations
                        .Where(res => res.RoomID == request.RoomId &&
                                     res.CheckInDate < request.CheckOutDate &&
                                     res.CheckOutDate > request.CheckInDate &&
                                     res.Status != "Cancelled" &&
                                     res.Status != "Pending")
                        .Select(res => new { res.CheckInDate, res.CheckOutDate })
                        .FirstOrDefaultAsync();

                    return Json(new {
                        isAvailable = false,
                        message = $"This room is already booked from {conflictingReservation?.CheckInDate:MMM dd, yyyy} to {conflictingReservation?.CheckOutDate:MMM dd, yyyy}. Please choose different dates.",
                        conflictingDates = new {
                            checkIn = conflictingReservation?.CheckInDate,
                            checkOut = conflictingReservation?.CheckOutDate
                        }
                    });
                }

                return Json(new {
                    isAvailable = true,
                    message = "Room is available for the selected dates!"
                });
            }
            catch (Exception ex)
            {
                return Json(new {
                    isAvailable = false,
                    message = "An error occurred while checking availability. Please try again."
                });
            }
        }

        public class AvailabilityCheckRequest
        {
            public int RoomId { get; set; }
            public DateTime CheckInDate { get; set; }
            public DateTime CheckOutDate { get; set; }
        }
    }
}
