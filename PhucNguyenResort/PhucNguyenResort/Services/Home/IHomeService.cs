using PhucNguyenResort.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhucNguyenResort.Services.Home
{
    public interface IHomeService
    {
        Task<IEnumerable<RoomCategory>> GetRoomCategoriesForHomeAsync();
        Task<RoomCategory?> GetRoomDetailsAsync(int id);
        Task<int> CheckAvailabilityAsync(int categoryId, System.DateTime checkInDate, System.DateTime checkOutDate);
        Task<IEnumerable<RoomCategory>> SearchAvailableCategoriesAsync(System.DateTime checkInDate, System.DateTime checkOutDate, int adultsCount, int childrenCount, int roomCount);
    }
}
