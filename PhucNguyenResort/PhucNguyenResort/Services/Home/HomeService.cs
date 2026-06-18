using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhucNguyenResort.Services.Home
{
    public class HomeService : IHomeService
    {
        private readonly ResortDbContext _context;

        public HomeService(ResortDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RoomCategory>> GetRoomCategoriesForHomeAsync()
        {
            return await _context.RoomCategories.ToListAsync();
        }

        public async Task<RoomCategory?> GetRoomDetailsAsync(int id)
        {
            return await _context.RoomCategories
                .Include(r => r.Images)
                .FirstOrDefaultAsync(r => r.CategoryId == id);
        }

        public async Task<int> CheckAvailabilityAsync(int categoryId, System.DateTime checkInDate, System.DateTime checkOutDate)
        {
            var totalRooms = await _context.Rooms
                .CountAsync(r => r.CategoryId == categoryId && r.Status != PhucNguyenResort.Enums.RoomStatus.Maintenance);

            var bookedRooms = await _context.BookingDetails
                .Where(bd => bd.RoomCategoryId == categoryId &&
                             bd.Booking != null &&
                             bd.Booking.BookingStatus != PhucNguyenResort.Enums.BookingStatus.Cancelled &&
                             bd.Booking.BookingStatus != PhucNguyenResort.Enums.BookingStatus.CheckedOut &&
                             bd.Booking.CheckInDate < checkOutDate &&
                             bd.Booking.CheckOutDate > checkInDate)
                .CountAsync();

            var available = totalRooms - bookedRooms;
            return available > 0 ? available : 0;
        }

        public async Task<IEnumerable<RoomCategory>> SearchAvailableCategoriesAsync(System.DateTime checkInDate, System.DateTime checkOutDate, int adultsCount, int childrenCount, int roomCount)
        {
            var categories = await _context.RoomCategories
                .Where(c => c.MaxAdults >= adultsCount && c.MaxChildren >= childrenCount)
                .ToListAsync();

            var availableCategories = new List<RoomCategory>();
            foreach (var cat in categories)
            {
                var availableRooms = await CheckAvailabilityAsync(cat.CategoryId, checkInDate, checkOutDate);
                if (availableRooms >= roomCount)
                {
                    cat.AvailableRoomsCount = availableRooms;
                    availableCategories.Add(cat);
                }
            }

            return availableCategories;
        }
    }
}
