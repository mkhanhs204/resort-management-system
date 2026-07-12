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
        // 1. NÂNG CẤP LẤY DANH SÁCH: Sửa lỗi lặp thông tin hiển thị tại cột Reason
        // =========================================================================
        public async Task<List<DirtyRoomViewModel>> GetDirtyRoomsAsync()
        {
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

                // ĐÃ SỬA: Giữ nguyên lý do bẩn gốc của phòng, không đè chữ "Đang dọn bởi..." 
                // để tránh trùng lặp thông tin với cột Trạng thái (Status Badge)
                string reason = lastBooking != null ? "Khách vừa trả phòng (Check-out)" : "Lịch trình dọn dẹp định kỳ";

                result.Add(new DirtyRoomViewModel
                {
                    RoomId = room.RoomId,
                    RoomNumber = room.RoomNumber ?? "N/A",
                    FloorNumber = room.FloorNumber ?? 0,
                    RoomType = room.Category != null ? room.Category.CategoryName : "Chưa phân loại",
                    Status = room.Status,
                    DirtyReason = reason,
                    LastCheckedOutTime = lastBooking?.CheckOutDate,
                    // QUAN TRỌNG: Đẩy dữ liệu nhân viên từ DB lên ViewModel
                    AssignedStaffName = room.AssignedStaffName
                });
            }

            return result;
        }

        // =========================================================================
        // TÍCH HỢP MỚI: Lấy danh sách các phòng đã dọn xong trong ngày của nhân viên
        // =========================================================================
        public async Task<List<DirtyRoomViewModel>> GetCleanedRoomsByStaffTodayAsync(string staffName)
        {
            // Quét các phòng hiện tại đang ở trạng thái Sẵn sàng (Available)
            // (Khi hệ thống nâng cấp bảng HousekeepingLog, hàm này sẽ chuyển sang query từ bảng Log)
            var cleanedRooms = await _context.Rooms
                .Include(r => r.Category)
                .Where(r => r.Status == RoomStatus.Available)
                .ToListAsync();

            var result = new List<DirtyRoomViewModel>();
            foreach (var room in cleanedRooms)
            {
                result.Add(new DirtyRoomViewModel
                {
                    RoomId = room.RoomId,
                    RoomNumber = room.RoomNumber ?? "N/A",
                    FloorNumber = room.FloorNumber ?? 0,
                    RoomType = room.Category != null ? room.Category.CategoryName : "Chưa phân loại",
                    Status = room.Status,
                    DirtyReason = "Đã hoàn thành dọn dẹp trong ca"
                });
            }
            return result;
        }

        // =========================================================================
        // 2. NÂNG CẤP HOÀN THÀNH: Kiểm tra bảo mật gốc (Chỉ người nhận phòng mới được hoàn thành)
        // =========================================================================
        public async Task<bool> UpdateCleaningStatusAsync(int roomId, string currentStaff)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null) return false;

            // BẢO MẬT: Ngăn chặn hack request cố tình bấm nút phòng của người khác
            if (room.Status == RoomStatus.CleaningInProgress &&
                !string.IsNullOrEmpty(room.AssignedStaffName) &&
                !room.AssignedStaffName.Equals(currentStaff, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            room.Status = RoomStatus.Available;
            room.AssignedStaffName = null; // Clear nhân viên sau khi phòng sạch sẽ trở lại

            await _context.SaveChangesAsync();
            return true;
        }

        // Đổ bóng hàm cũ để tránh lỗi biên dịch hệ thống cũ
        public async Task<bool> UpdateCleaningStatusAsync(int roomId)
        {
            return await UpdateCleaningStatusAsync(roomId, "System");
        }

        // =========================================================================
        // 3. BÁO HỎNG PHÒNG (KÈM TẠO PHIẾU BẢO TRÌ)
        // =========================================================================
        public async Task<bool> ReportMaintenanceAsync(int roomId, string description)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null) return false;

            room.Status = RoomStatus.Maintenance;
            room.AssignedStaffName = null; // Huỷ gán vết dọn dẹp cũ khi phòng chuyển sang hỏng

            var maintenanceRequest = new MaintenanceRequest
            {
                RoomId = roomId,
                Description = string.IsNullOrEmpty(description) ? "Nhân viên phát hiện hỏng hóc khi dọn phòng" : description,
                CreatedAt = DateTime.Now,
                IsResolved = false
            };

            _context.MaintenanceRequests.Add(maintenanceRequest);
            await _context.SaveChangesAsync();
            return true;
        }

        // =========================================================================
        // 4. HÀM MỚI NÂNG CẤP: Lưu vết tài khoản khi nhân viên ấn "Start Cleaning"
        // =========================================================================
        public async Task<bool> UpdateRoomStatusWithStaffAsync(int roomId, RoomStatus status, string currentStaff)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null) return false;

            // Chống xung đột: Nếu trong lúc mở danh sách có người khác đã nhấn Start trước rồi
            if (room.Status == RoomStatus.CleaningInProgress && room.AssignedStaffName != currentStaff)
            {
                return false;
            }

            room.Status = status;

            if (status == RoomStatus.CleaningInProgress)
            {
                room.AssignedStaffName = currentStaff; // Đóng dấu chủ quyền phòng cho nhân viên này
            }
            else if (status == RoomStatus.Available)
            {
                room.AssignedStaffName = null;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        // Giữ nguyên hàm cũ để không ảnh hưởng các hàm hệ thống khác
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