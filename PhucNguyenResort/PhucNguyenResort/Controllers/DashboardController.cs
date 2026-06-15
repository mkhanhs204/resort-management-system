using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace PhucNguyenResort.Web.Controllers
{
    [Authorize(Roles = "System Admin,Business Manager,Receptionist,F&B Staff,Kitchen Staff,Tour Guide,Housekeeping Staff,Maintenance Staff")]
    public class DashboardController : Controller
    {
        // Trang chính: Tự động chuyển hướng đến dashboard phù hợp với role của người dùng
        public IActionResult Index()
        {
            if (User.IsInRole("System Admin"))
                return RedirectToAction("Admin");
            if (User.IsInRole("Business Manager"))
                return RedirectToAction("Manager");
            if (User.IsInRole("Receptionist"))
                return RedirectToAction("Receptionist");
            if (User.IsInRole("F&B Staff"))
                return RedirectToAction("FnB");
            if (User.IsInRole("Kitchen Staff"))
                return RedirectToAction("Kitchen");
            if (User.IsInRole("Tour Guide"))
                return RedirectToAction("TourGuide");
            if (User.IsInRole("Housekeeping Staff"))
                return RedirectToAction("Housekeeping");
            if (User.IsInRole("Maintenance Staff"))
                return RedirectToAction("Maintenance");

            return View();
        }

        // 1. System Admin Dashboard
        [Authorize(Roles = "System Admin")]
        public IActionResult Admin()
        {
            return View();
        }

        // 2. Business Manager Dashboard
        [Authorize(Roles = "Business Manager")]
        public IActionResult Manager()
        {
            return View();
        }

        // 3. Receptionist (Front Desk) Dashboard
        [Authorize(Roles = "Receptionist")]
        public IActionResult Receptionist()
        {
            return View();
        }

        // 4. F&B Staff Dashboard
        [Authorize(Roles = "F&B Staff")]
        public IActionResult FnB()
        {
            return View();
        }

        // 5. Kitchen Staff Dashboard
        [Authorize(Roles = "Kitchen Staff")]
        public IActionResult Kitchen()
        {
            return View();
        }

        // 6. Tour Guide Dashboard
        [Authorize(Roles = "Tour Guide")]
        public IActionResult TourGuide()
        {
            return View();
        }

        // 7. Housekeeping Staff Dashboard
        [Authorize(Roles = "Housekeeping Staff")]
        public IActionResult Housekeeping()
        {
            return View();
        }

        // 8. Maintenance Staff Dashboard
        [Authorize(Roles = "Maintenance Staff")]
        public IActionResult Maintenance()
        {
            return View();
        }
    }
}
