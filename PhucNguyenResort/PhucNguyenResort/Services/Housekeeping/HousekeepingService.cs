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
                    AssignedStaffName = room.AssignedStaffName
                });
            }

            return result;
        }

        public async Task<List<DirtyRoomViewModel>> GetCleanedRoomsByStaffTodayAsync(string staffName)
        {
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

        public async Task<bool> UpdateCleaningStatusAsync(int roomId, string currentStaff)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null) return false;

            if (room.Status == RoomStatus.CleaningInProgress &&
                !string.IsNullOrEmpty(room.AssignedStaffName) &&
                !room.AssignedStaffName.Equals(currentStaff, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            var log = new RoomCleaningLog
            {
                RoomId = room.RoomId,
                AssignedStaffName = !string.IsNullOrEmpty(room.AssignedStaffName) ? room.AssignedStaffName : currentStaff,
                StartedAt = DateTime.Now.AddMinutes(-30),
                CompletedAt = DateTime.Now,
                Notes = "Đã hoàn thành dọn dẹp định kỳ đạt chuẩn 5 sao."
            };
            _context.RoomCleaningLogs.Add(log);

            room.Status = RoomStatus.Available;
            room.AssignedStaffName = null;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateCleaningStatusAsync(int roomId)
        {
            return await UpdateCleaningStatusAsync(roomId, "System");
        }

        public async Task<bool> ReportMaintenanceAsync(int roomId, string description)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null) return false;

            room.Status = RoomStatus.Maintenance;
            room.AssignedStaffName = null;

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

        public async Task<bool> UpdateRoomStatusWithStaffAsync(int roomId, RoomStatus status, string currentStaff)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null) return false;

            if (room.Status == RoomStatus.CleaningInProgress && room.AssignedStaffName != currentStaff)
            {
                return false;
            }

            room.Status = status;

            if (status == RoomStatus.CleaningInProgress)
            {
                room.AssignedStaffName = currentStaff;
            }
            else if (status == RoomStatus.Available)
            {
                room.AssignedStaffName = null;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateRoomStatusAsync(int roomId, RoomStatus status)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null) return false;

            room.Status = status;
            await _context.SaveChangesAsync();
            return true;
        }

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

        public async Task<List<RoomCleaningLog>> GetAllCleaningLogsAsync()
        {
            return await _context.RoomCleaningLogs
                .Include(l => l.Room)
                .OrderByDescending(l => l.CompletedAt)
                .ToListAsync();
        }

        public async Task<List<MaintenanceRequest>> GetAllMaintenanceRequestsAsync()
        {
            return await _context.MaintenanceRequests
                .Include(r => r.Room)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }
    }
}