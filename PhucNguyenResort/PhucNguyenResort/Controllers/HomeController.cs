using Microsoft.AspNetCore.Mvc;
using PhucNguyenResort.ViewModels;
using System.Diagnostics;
using PhucNguyenResort.Services.Home;
using PhucNguyenResort.Services.Tours;
using PhucNguyenResort.Models;

namespace PhucNguyenResort.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeService _homeService;
        private readonly ITourService _tourService;

        public HomeController(ILogger<HomeController> logger, IHomeService homeService, ITourService tourService)
        {
            _logger = logger;
            _homeService = homeService;
            _tourService = tourService;
        }

        public async Task<IActionResult> Index()
        {
            var roomCategories = await _homeService.GetRoomCategoriesForHomeAsync();
            ViewBag.Tours = await _tourService.GetAllToursAsync();
            return View(roomCategories);
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public async Task<IActionResult> RoomDetails(int id)
        {
            var roomCategory = await _homeService.GetRoomDetailsAsync(id);

            if (roomCategory == null)
            {
                return NotFound();
            }

            return View(roomCategory);
        }

        [HttpGet]
        public async Task<IActionResult> SearchRooms(string checkIn, string checkOut, int guestCount = 2, int roomCount = 1)
        {
            bool isFiltered = !string.IsNullOrEmpty(checkIn) && !string.IsNullOrEmpty(checkOut);
            IEnumerable<RoomCategory> availableCategories;

            if (!isFiltered)
            {
                availableCategories = await _homeService.GetRoomCategoriesForHomeAsync();
                
                ViewBag.CheckIn = "";
                ViewBag.CheckOut = "";
                ViewBag.GuestCount = guestCount;
                ViewBag.RoomCount = roomCount;
                ViewBag.IsFiltered = false;

                return View(availableCategories);
            }

            string[] formats = { "dd/MM/yyyy", "dd MM, yyyy", "d MMMM, yyyy", "dd MMMM, yyyy", "MM/dd/yyyy", "yyyy-MM-dd" };
            
            if (!DateTime.TryParseExact(checkIn, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime checkInDate))
            {
                checkInDate = DateTime.Today;
            }

            if (!DateTime.TryParseExact(checkOut, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime checkOutDate))
            {
                checkOutDate = checkInDate.AddDays(1);
            }

            if (checkOutDate <= checkInDate)
            {
                checkOutDate = checkInDate.AddDays(1);
            }

            if (checkInDate < DateTime.Today)
            {
                checkInDate = DateTime.Today;
                if (checkOutDate <= checkInDate) checkOutDate = checkInDate.AddDays(1);
            }

            availableCategories = await _homeService.SearchAvailableCategoriesAsync(checkInDate, checkOutDate, guestCount, roomCount);

            ViewBag.CheckIn = checkInDate.ToString("dd/MM/yyyy");
            ViewBag.CheckOut = checkOutDate.ToString("dd/MM/yyyy");
            ViewBag.GuestCount = guestCount;
            ViewBag.RoomCount = roomCount;
            ViewBag.IsFiltered = true;

            return View(availableCategories);
        }

        [HttpGet]
        public async Task<IActionResult> CheckRoomAvailability(int categoryId, string checkIn, string checkOut)
        {
            string[] formats = { "dd/MM/yyyy", "dd MM, yyyy", "d MMMM, yyyy", "dd MMMM, yyyy", "MM/dd/yyyy", "yyyy-MM-dd" };
            if (!DateTime.TryParseExact(checkIn, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime checkInDate) ||
                !DateTime.TryParseExact(checkOut, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime checkOutDate))
            {
                // Fallback to standard TryParse if exact formats fail
                if (!DateTime.TryParse(checkIn, out checkInDate) || !DateTime.TryParse(checkOut, out checkOutDate))
                {
                    return Ok(new { success = false, message = "Ngày không hợp lệ. Vui lòng chọn lại." });
                }
            }

            if (checkInDate.Date >= checkOutDate.Date)
            {
                return Ok(new { success = false, message = "Ngày trả phòng phải sau ngày nhận phòng." });
            }

            if (checkInDate.Date < DateTime.Today)
            {
                return Ok(new { success = false, message = "Không thể đặt phòng trong quá khứ." });
            }

            var availableRooms = await _homeService.CheckAvailabilityAsync(categoryId, checkInDate, checkOutDate);

            if (availableRooms > 0)
            {
                return Ok(new { success = true, availableRooms = availableRooms, message = $"Tuyệt vời! Còn {availableRooms} phòng trống." });
            }
            
            return Ok(new { success = true, availableRooms = 0, message = "Rất tiếc, loại phòng này đã hết trong khoảng thời gian bạn chọn." });
        }

        public IActionResult Privacy()
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
