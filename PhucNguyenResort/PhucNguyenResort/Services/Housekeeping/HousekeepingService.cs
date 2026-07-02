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

        // =========================================================================
        // 1. NÂNG CẤP LẤY DANH SÁCH: Lấy cả phòng đang bẩn HOẶC đang tiến hành dọn
        // =========================================================================
        public async Task<List<DirtyRoomViewModel>> GetDirtyRoomsAsync()
        {
            // Thay đổi điều kiện Where để bao gồm cả trạng thái CleaningInProgress
            var dirtyRoomsQuery = await _context.Rooms
                .Include(r => r.Category)
                .Where(r => r.Status == RoomStatus.NeedsCleaning || r.Status == RoomStatus.CleaningInProgress)
                .ToListAsync();

            var result = new List<DirtyRoomViewModel>();

            foreach (var room in dirtyRoomsQuery)
            {
                var lastBooking = await _context.Bookings
                    .Where(b => b.BookingDetails.Any(bd => bd.RoomId == room.RoomId)
                             && b.BookingStatus == BookingStatus.CheckedOut)
                    .OrderByDescending(b => b.CheckOutDate)
                    .FirstOrDefaultAsync();

                // Xác định nội dung lý do hiển thị linh hoạt theo trạng thái dọn dẹp
                string reason = "Lịch trình dọn dẹp định kỳ";
                if (room.Status == RoomStatus.CleaningInProgress)
                {
                    reason = "Đang tiến hành dọn dẹp...";
                }
                else if (lastBooking != null)
                {
                    reason = "Khách vừa trả phòng (Check-out)";
                }

                result.Add(new DirtyRoomViewModel
                {
                    RoomId = room.RoomId,
                    RoomNumber = room.RoomNumber ?? "N/A",
                    FloorNumber = room.FloorNumber ?? 0,
                    RoomType = room.Category != null ? room.Category.CategoryName : "Chưa phân loại",
                    Status = room.Status,
                    DirtyReason = reason,
                    LastCheckedOutTime = lastBooking?.CheckOutDate
                });
            }

            return result;
        }

        // =========================================================================
        // 2. CẬP NHẬT HOÀN THÀNH DỌN DẸP (MARK CLEANED)
        // =========================================================================
        public async Task<bool> UpdateCleaningStatusAsync(int roomId)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null) return false;

            room.Status = RoomStatus.Available;
            await _context.SaveChangesAsync();
            return true;
        }

        // =========================================================================
        // 3. BÁO HỎNG PHÒNG (KÈM TẠO PHIẾU BẢO TRÌ)
        // =========================================================================
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

            // 3. Lưu đồng thời cả 2 thay đổi xuống DB
            await _context.SaveChangesAsync();
            return true;
        }

        // =========================================================================
        // 4. HÀM MỚI TÍCH HỢP: Đổi trạng thái linh hoạt (Dùng cho tính năng Start Cleaning)
        // =========================================================================
        public async Task<bool> UpdateRoomStatusAsync(int roomId, RoomStatus status)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null) return false;

            room.Status = status;
            await _context.SaveChangesAsync();
            return true;
        }

        // =========================================================================
        // 5. CÁC HÀM THỐNG KÊ CARDS (KPI) CHO DASHBOARD
        // =========================================================================
        public async Task<int> GetCountRoomsNeedCleaningAsync()
        {
            return await _context.Rooms.CountAsync(r => r.Status == RoomStatus.NeedsCleaning);
        }

        public async Task<int> GetCountCurrentlyCleaningAsync()
        {
            return await _context.Rooms.CountAsync(r => r.Status == RoomStatus.CleaningInProgress);
        }

        public async Task<int> GetCountCleanedTodayAsync()
        {
            return await _context.Rooms.CountAsync(r => r.Status == RoomStatus.Available);
        }
    }
}