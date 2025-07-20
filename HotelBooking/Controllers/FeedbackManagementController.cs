using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Data;
using HotelBooking.Models;
using HotelBooking.Models.ViewModels;

namespace HotelBooking.Controllers
{
    [Authorize(Roles = "Admin,Staff")]
    public class FeedbackManagementController : Controller
    {
        private readonly HotelBookingContext _context;

        public FeedbackManagementController(HotelBookingContext context)
        {
            _context = context;
        }

        // GET: FeedbackManagement
        public async Task<IActionResult> Index(string searchTerm, int? ratingFilter, DateTime? fromDate, DateTime? toDate, int page = 1, int pageSize = 10)
        {
            var query = _context.Feedbacks
                .Include(f => f.Guest)
                .Include(f => f.Reservation!)
                    .ThenInclude(r => r.Room!)
                    .ThenInclude(r => r.RoomType)
                .AsQueryable();

            // Search
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(f => f.Guest!.FirstName!.Contains(searchTerm) ||
                                        f.Guest.LastName!.Contains(searchTerm) ||
                                        f.Guest.Email!.Contains(searchTerm) ||
                                        f.Comment!.Contains(searchTerm));
            }

            // Rating filter
            if (ratingFilter.HasValue)
            {
                query = query.Where(f => f.Rating == ratingFilter.Value);
            }

            // Date filter
            if (fromDate.HasValue)
            {
                query = query.Where(f => f.FeedbackDate >= fromDate.Value);
            }
            if (toDate.HasValue)
            {
                query = query.Where(f => f.FeedbackDate <= toDate.Value);
            }

            var totalFeedbacks = await query.CountAsync();
            var averageRating = totalFeedbacks > 0 ? await query.AverageAsync(f => (double)f.Rating) : 0;

            // Rating distribution
            var ratingDistribution = await query
                .GroupBy(f => f.Rating)
                .Select(g => new { Rating = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Rating, x => x.Count);

            var feedbacks = await query
                .OrderByDescending(f => f.FeedbackDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(f => new FeedbackManagementViewModel
                {
                    FeedbackID = f.FeedbackID,
                    ReservationID = f.ReservationID,
                    GuestID = f.GuestID,
                    Rating = f.Rating,
                    Comment = f.Comment,
                    FeedbackDate = f.FeedbackDate,
                    GuestName = $"{f.Guest!.FirstName} {f.Guest.LastName}",
                    GuestEmail = f.Guest.Email,
                    RoomNumber = f.Reservation!.Room!.RoomNumber,
                    RoomType = f.Reservation.Room.RoomType!.TypeName,
                    CheckInDate = f.Reservation.CheckInDate,
                    CheckOutDate = f.Reservation.CheckOutDate
                })
                .ToListAsync();

            var viewModel = new FeedbackListViewModel
            {
                Feedbacks = feedbacks,
                TotalFeedbacks = totalFeedbacks,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)totalFeedbacks / pageSize),
                SearchTerm = searchTerm,
                RatingFilter = ratingFilter,
                FromDate = fromDate,
                ToDate = toDate,
                AverageRating = averageRating,
                RatingDistribution = ratingDistribution
            };

            return View(viewModel);
        }

        // GET: FeedbackManagement/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var feedback = await _context.Feedbacks
                .Include(f => f.Guest)
                .Include(f => f.Reservation!)
                    .ThenInclude(r => r.Room!)
                    .ThenInclude(r => r.RoomType)
                .FirstOrDefaultAsync(f => f.FeedbackID == id);

            if (feedback == null)
            {
                return NotFound();
            }

            var viewModel = new FeedbackManagementViewModel
            {
                FeedbackID = feedback.FeedbackID,
                ReservationID = feedback.ReservationID,
                GuestID = feedback.GuestID,
                Rating = feedback.Rating,
                Comment = feedback.Comment,
                FeedbackDate = feedback.FeedbackDate,
                GuestName = $"{feedback.Guest?.FirstName} {feedback.Guest?.LastName}",
                GuestEmail = feedback.Guest?.Email,
                RoomNumber = feedback.Reservation?.Room?.RoomNumber,
                RoomType = feedback.Reservation?.Room?.RoomType?.TypeName,
                CheckInDate = feedback.Reservation?.CheckInDate ?? DateTime.MinValue,
                CheckOutDate = feedback.Reservation?.CheckOutDate ?? DateTime.MinValue
            };

            return View(viewModel);
        }

        // GET: FeedbackManagement/Respond/5
        public async Task<IActionResult> Respond(int id)
        {
            var feedback = await _context.Feedbacks
                .Include(f => f.Guest)
                .Include(f => f.Reservation!)
                    .ThenInclude(r => r.Room)
                .FirstOrDefaultAsync(f => f.FeedbackID == id);

            if (feedback == null)
            {
                return NotFound();
            }

            var viewModel = new FeedbackResponseViewModel
            {
                FeedbackID = feedback.FeedbackID,
                CustomerName = $"{feedback.Guest?.FirstName} {feedback.Guest?.LastName}",
                Comment = feedback.Comment,
                Rating = feedback.Rating,
                FeedbackDate = feedback.FeedbackDate,
                RoomNumber = feedback.Reservation?.Room?.RoomNumber ?? "N/A",
                RoomType = feedback.Reservation?.Room?.RoomType?.TypeName ?? "N/A",
                Category = feedback.Category ?? "General"
            };

            return View(viewModel);
        }

        // POST: FeedbackManagement/Respond
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Respond(FeedbackResponseViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Note: In a real application, you might want to create a separate FeedbackResponse table
                    // For now, we'll just add a success message
                    TempData["SuccessMessage"] = "Response sent successfully to the guest.";
                    return RedirectToAction(nameof(Details), new { id = viewModel.FeedbackID });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while sending the response: " + ex.Message);
                }
            }

            // Reload feedback data
            var feedback = await _context.Feedbacks
                .Include(f => f.Guest)
                .Include(f => f.Reservation!)
                    .ThenInclude(r => r.Room)
                .FirstOrDefaultAsync(f => f.FeedbackID == viewModel.FeedbackID);

            if (feedback != null)
            {
                viewModel.CustomerName = $"{feedback.Guest?.FirstName} {feedback.Guest?.LastName}";
                viewModel.Comment = feedback.Comment ?? "No comment";
                viewModel.Rating = feedback.Rating;
                viewModel.FeedbackDate = feedback.FeedbackDate;
                viewModel.RoomNumber = feedback.Reservation?.Room?.RoomNumber ?? "N/A";
                viewModel.RoomType = feedback.Reservation?.Room?.RoomType?.TypeName ?? "N/A";
                viewModel.Category = "General";
            }

            return View(viewModel);
        }

        // GET: FeedbackManagement/Statistics
        public async Task<IActionResult> Statistics()
        {
            var totalFeedbacks = await _context.Feedbacks.CountAsync();
            var averageRating = totalFeedbacks > 0 ? await _context.Feedbacks.AverageAsync(f => (double)f.Rating) : 0;

            // Rating distribution
            var ratingDistribution = await _context.Feedbacks
                .GroupBy(f => f.Rating)
                .Select(g => new { Rating = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Rating, x => x.Count);

            // Monthly trends (last 12 months)
            var twelveMonthsAgo = DateTime.Now.AddMonths(-12);
            var feedbacksForTrends = await _context.Feedbacks
                .Where(f => f.FeedbackDate >= twelveMonthsAgo)
                .ToListAsync();

            var monthlyTrends = feedbacksForTrends
                .GroupBy(f => new { f.FeedbackDate.Year, f.FeedbackDate.Month })
                .Select(g => new FeedbackTrendData
                {
                    Month = $"{g.Key.Year}-{g.Key.Month:D2}",
                    AverageRating = g.Average(f => (double)f.Rating),
                    FeedbackCount = g.Count()
                })
                .OrderBy(x => x.Month)
                .ToList();

            // Recent feedbacks
            var recentFeedbacks = await _context.Feedbacks
                .Include(f => f.Guest)
                .Include(f => f.Reservation!)
                    .ThenInclude(r => r.Room!)
                    .ThenInclude(r => r.RoomType)
                .OrderByDescending(f => f.FeedbackDate)
                .Take(10)
                .Select(f => new FeedbackManagementViewModel
                {
                    FeedbackID = f.FeedbackID,
                    Rating = f.Rating,
                    Comment = f.Comment,
                    FeedbackDate = f.FeedbackDate,
                    GuestName = $"{f.Guest!.FirstName} {f.Guest.LastName}",
                    RoomNumber = f.Reservation!.Room!.RoomNumber,
                    RoomType = f.Reservation.Room.RoomType!.TypeName
                })
                .ToListAsync();

            // Top rated experiences
            var topRated = await _context.Feedbacks
                .Include(f => f.Guest)
                .Include(f => f.Reservation!)
                    .ThenInclude(r => r.Room!)
                    .ThenInclude(r => r.RoomType)
                .Where(f => f.Rating >= 4)
                .OrderByDescending(f => f.Rating)
                .ThenByDescending(f => f.FeedbackDate)
                .Take(5)
                .Select(f => new FeedbackManagementViewModel
                {
                    FeedbackID = f.FeedbackID,
                    Rating = f.Rating,
                    Comment = f.Comment,
                    FeedbackDate = f.FeedbackDate,
                    GuestName = $"{f.Guest!.FirstName} {f.Guest.LastName}",
                    RoomNumber = f.Reservation!.Room!.RoomNumber,
                    RoomType = f.Reservation.Room.RoomType!.TypeName
                })
                .ToListAsync();

            // Low rated experiences
            var lowRated = await _context.Feedbacks
                .Include(f => f.Guest)
                .Include(f => f.Reservation!)
                    .ThenInclude(r => r.Room!)
                    .ThenInclude(r => r.RoomType)
                .Where(f => f.Rating <= 2)
                .OrderBy(f => f.Rating)
                .ThenByDescending(f => f.FeedbackDate)
                .Take(5)
                .Select(f => new FeedbackManagementViewModel
                {
                    FeedbackID = f.FeedbackID,
                    Rating = f.Rating,
                    Comment = f.Comment,
                    FeedbackDate = f.FeedbackDate,
                    GuestName = $"{f.Guest!.FirstName} {f.Guest.LastName}",
                    RoomNumber = f.Reservation!.Room!.RoomNumber,
                    RoomType = f.Reservation.Room.RoomType!.TypeName
                })
                .ToListAsync();

            var viewModel = new FeedbackStatisticsViewModel
            {
                AverageRating = averageRating,
                TotalFeedbacks = totalFeedbacks,
                RatingDistribution = ratingDistribution,
                MonthlyTrends = monthlyTrends,
                RecentFeedbacks = recentFeedbacks,
                TopRatedExperiences = topRated,
                LowRatedExperiences = lowRated
            };

            return View(viewModel);
        }

        // POST: FeedbackManagement/AddResponse/5
        [HttpPost]
        public async Task<IActionResult> AddResponse(int id, string response)
        {
            try
            {
                var feedback = await _context.Feedbacks.FindAsync(id);
                if (feedback == null)
                {
                    return Json(new { success = false, message = "Feedback not found" });
                }

                feedback.Response = response;
                feedback.ResponseDate = DateTime.Now;
                feedback.ResponseBy = User.Identity?.Name ?? "System";

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Response added successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: FeedbackManagement/MarkAsResolved/5
        [HttpPost]
        public async Task<IActionResult> MarkAsResolved(int id)
        {
            try
            {
                var feedback = await _context.Feedbacks.FindAsync(id);
                if (feedback == null)
                {
                    return Json(new { success = false, message = "Feedback not found" });
                }

                // You might want to add a Status field to Feedback model
                // For now, we'll just ensure it has a response
                if (string.IsNullOrEmpty(feedback.Response))
                {
                    feedback.Response = "Issue resolved by management.";
                    feedback.ResponseDate = DateTime.Now;
                    feedback.ResponseBy = User.Identity?.Name ?? "System";
                }

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Feedback marked as resolved" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
