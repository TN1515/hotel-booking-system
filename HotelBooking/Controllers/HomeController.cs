using System.Diagnostics;
using HotelBooking.Models;
using HotelBooking.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HotelBookingContext _context;

        public HomeController(ILogger<HomeController> logger, HotelBookingContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Always show home page with role-specific content
            var featuredRooms = await _context.Rooms
                .Include(r => r.RoomType)
                .Include(r => r.RoomImages!.Where(ri => ri.IsActive))
                .Where(r => r.IsActive && r.Status == "Available")
                .OrderBy(r => r.RoomNumber)
                .Take(6)
                .ToListAsync();

            var roomTypes = await _context.RoomTypes
                .Where(rt => rt.IsActive)
                .ToListAsync();

            var homeViewModel = new HomeViewModel
            {
                FeaturedRooms = featuredRooms,
                RoomTypes = roomTypes
            };

            // Set ViewBag for role-specific content
            ViewBag.IsAdmin = User.IsInRole("Admin");
            ViewBag.IsStaff = User.IsInRole("Staff");
            ViewBag.IsCustomer = User.IsInRole("Customer");
            ViewBag.IsAuthenticated = User.Identity!.IsAuthenticated;

            return View(homeViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult TestSummary()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
