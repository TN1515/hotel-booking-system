using Microsoft.AspNetCore.Mvc;
using HotelBooking.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Controllers
{
    public class TestController : Controller
    {
        private readonly HotelBookingContext _context;
        private readonly IConfiguration _configuration;

        public TestController(HotelBookingContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IActionResult> DatabaseInfo()
        {
            try
            {
                // Get connection string
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                
                // Test database connection
                var canConnect = await _context.Database.CanConnectAsync();
                
                // Get database name
                var databaseName = _context.Database.GetDbConnection().Database;
                
                // Get server name
                var serverName = _context.Database.GetDbConnection().DataSource;
                
                // Count some tables to verify data
                var roomCount = await _context.Rooms.CountAsync();
                var userCount = await _context.Users.CountAsync();
                var roleCount = await _context.Roles.CountAsync();

                ViewBag.ConnectionString = connectionString;
                ViewBag.CanConnect = canConnect;
                ViewBag.DatabaseName = databaseName;
                ViewBag.ServerName = serverName;
                ViewBag.RoomCount = roomCount;
                ViewBag.UserCount = userCount;
                ViewBag.RoleCount = roleCount;

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }
    }
}
