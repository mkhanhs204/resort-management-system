using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace PhucNguyenResort.Web.Controllers
{
    [Authorize(Roles = "System Admin,Business Manager,Receptionist,F&B Staff,Kitchen Staff,Tour Guide,Housekeeping Staff,Maintenance Staff")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
