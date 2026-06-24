using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Enums;
using PhucNguyenResort.Models;
using PhucNguyenResort.ViewModels.Housekeeping;

namespace PhucNguyenResort.Services.Housekeeping
{
    public class HousekeepingService : IHousekeepingService
    {
        private readonly ResortDbContext _context;

        public HousekeepingService(ResortDbContext context)
        {
            _context = context;
        }

        public async Task<List<DirtyRoomViewModel>> GetDirtyRoomsAsync()
        {
            var dirtyRoomsQuery = await _context.Rooms
                .Include(r => r.Category)
                .Where(r => r.Status == RoomStatus.NeedsCleaning)
                .ToListAsync();

            var result = new List<DirtyRoomViewModel>();

            foreach (var room in dirtyRoomsQuery)
            {
                var lastBooking = await _context.Bookings
                    .Where(b => b.RoomId == room.RoomId && b.BookingStatus == BookingStatus.CheckedOut)
                    .OrderByDescending(b => b.CheckOutDate)
                    .FirstOrDefaultAsync();

                result.Add(new DirtyRoomViewModel
                {
                    RoomId = room.RoomId,
                    RoomNumber = room.RoomNumber ?? "N/A",
                    FloorNumber = room.FloorNumber ?? 0,
                    RoomType = room.Category != null ? room.Category.CategoryName : "Chưa phân loại",
                    Status = room.Status,
                    DirtyReason = lastBooking != null ? "Khách vừa trả phòng (Check-out)" : "Lịch trình dọn dẹp định kỳ",
                    LastCheckedOutTime = lastBooking?.CheckOutDate
                });
            }

            return result;
        }

        public async Task<bool> UpdateCleaningStatusAsync(int roomId)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null) return false;

            room.Status = RoomStatus.Available;
            await _context.SaveChangesAsync();
            return true;
        }

        // LUỒNG CHUYÊN NGHIỆP: Đổi trạng thái + Tạo phiếu sửa chữa
        public async Task<bool> ReportMaintenanceAsync(int roomId, string description)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null) return false;

            // 1. Khóa phòng bằng cách đổi trạng thái sang Bảo trì
            room.Status = RoomStatus.Maintenance;

            // 2. Tạo bản ghi chi tiết sự cố
            var maintenanceRequest = new MaintenanceRequest
            {
                RoomId = roomId,
                Description = string.IsNullOrEmpty(description) ? "Nhân viên phát hiện hỏng hóc khi dọn phòng" : description,
                CreatedAt = DateTime.Now,
                IsResolved = false
            };

            _context.MaintenanceRequests.Add(maintenanceRequest);

            // 3. Lưu đồng thời cả 2 thay đổi xuống DB thông qua Unit of Work của EF Core
            await _context.SaveChangesAsync();
            return true;
        }
    }
}