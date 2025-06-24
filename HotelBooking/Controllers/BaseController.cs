using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HotelBooking.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Set ViewBag properties for layout detection
            ViewBag.IsAdmin = User.IsInRole("Admin");
            ViewBag.IsStaff = User.IsInRole("Staff");
            ViewBag.IsCustomer = User.IsInRole("Customer");
            
            base.OnActionExecuting(context);
        }
    }
}
