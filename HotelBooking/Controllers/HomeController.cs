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
            // Get featured rooms and room types for the home page
            var featuredRooms = await _context.Rooms
                .Include(r => r.RoomType)
                .Where(r => r.IsActive)
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
