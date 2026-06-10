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

            var bookedRooms = await _context.Bookings
                .CountAsync(b => b.Room!.CategoryId == categoryId &&
                                 b.BookingStatus != PhucNguyenResort.Enums.BookingStatus.Cancelled &&
                                 b.BookingStatus != PhucNguyenResort.Enums.BookingStatus.NoShow &&
                                 b.CheckInDate < checkOutDate &&
                                 b.CheckOutDate > checkInDate);

            var available = totalRooms - bookedRooms;
            return available > 0 ? available : 0;
        }
    }
}
