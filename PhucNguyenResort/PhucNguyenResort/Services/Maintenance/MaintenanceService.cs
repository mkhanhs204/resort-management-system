using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Enums;
using PhucNguyenResort.ViewModels.Maintenance;

namespace PhucNguyenResort.Services.Maintenance
{
    public class MaintenanceService : IMaintenanceService
    {
        private readonly ResortDbContext _context;

        public MaintenanceService(ResortDbContext context)
        {
            _context = context;
        }

        // 1. LẤY DANH SÁCH TÁC VỤ HOẠT ĐỘNG
        public async Task<List<MaintenanceTaskViewModel>> GetMaintenanceTasksAsync()
        {
            return await _context.MaintenanceRequests
                .Include(m => m.Room)
                    .ThenInclude(r => r!.Category)
                .Where(m => !m.IsResolved)
                .OrderBy(m => m.AssignedStaffName)
                .ThenByDescending(m => m.CreatedAt)
                .Select(m => new MaintenanceTaskViewModel
                {
                    RequestId = m.RequestId,
                    RoomId = m.RoomId,
                    RoomNumber = m.Room != null ? m.Room.RoomNumber : "N/A",
                    RoomType = (m.Room != null && m.Room.Category != null) ? m.Room.Category.CategoryName : "N/A",
                    Description = m.Description,
                    CreatedAt = m.CreatedAt,
                    IsResolved = m.IsResolved,
                    ResolvedAt = m.ResolvedAt,
                    AssignedStaffName = m.AssignedStaffName,
                    // 🟢 GIẢI QUYẾT LỖI DÒNG 37: Sử dụng toán tử ?? để gán giá trị mặc định là 0 nếu tầng bị Null
                    FloorNumber = (m.Room != null && m.Room.FloorNumber.HasValue) ? m.Room.FloorNumber.Value : 0
                })
                .ToListAsync();
        }

        // 2. ĐẾM TỔNG SỰ CỐ CHƯA AI TIẾP NHẬN
        public async Task<int> GetPendingTasksCountAsync()
        {
            return await _context.MaintenanceRequests
                .CountAsync(t => !t.IsResolved && string.IsNullOrEmpty(t.AssignedStaffName));
        }

        // 3. ĐẾM TỔNG SỰ CỐ ĐÃ SỬA TRONG NGÀY
        public async Task<int> GetResolvedTodayCountAsync()
        {
            return await _context.MaintenanceRequests
                .CountAsync(t => t.IsResolved && t.ResolvedAt.HasValue && t.ResolvedAt.Value.Date == DateTime.Today);
        }

        // 4. TIẾP NHẬN TÁC VỤ
        public async Task<bool> StartFixingTaskAsync(int requestId, string staffName)
        {
            var request = await _context.MaintenanceRequests.FindAsync(requestId);

            if (request == null || request.IsResolved || !string.IsNullOrEmpty(request.AssignedStaffName))
                return false;

            request.AssignedStaffName = staffName;

            _context.Update(request);
            await _context.SaveChangesAsync();
            return true;
        }

        // 5. HOÀN THÀNH TÁC VỤ VÀ ĐỒNG BỘ TRẠNG THÁI PHÒNG
        public async Task<bool> ResolveTaskAsync(int requestId, string staffName)
        {
            var request = await _context.MaintenanceRequests
                .Include(m => m.Room)
                .FirstOrDefaultAsync(m => m.RequestId == requestId);

            if (request == null || request.IsResolved) return false;

            if (!string.IsNullOrEmpty(request.AssignedStaffName) && request.AssignedStaffName != staffName)
            {
                return false;
            }

            request.IsResolved = true;
            request.ResolvedAt = DateTime.Now;

            if (string.IsNullOrEmpty(request.AssignedStaffName))
            {
                request.AssignedStaffName = staffName;
            }

            if (request.Room != null)
            {
                request.Room.Status = RoomStatus.NeedsCleaning;
                _context.Update(request.Room);
            }

            _context.Update(request);
            await _context.SaveChangesAsync();
            return true;
        }

        // 6. LẤY THÀNH TÍCH CỦA RIÊNG THỢ TRONG NGÀY
        public async Task<List<MaintenanceTaskViewModel>> GetResolvedTasksByStaffTodayAsync(string staffName)
        {
            return await _context.MaintenanceRequests
                .Include(m => m.Room)
                    .ThenInclude(r => r!.Category)
                .Where(m => m.IsResolved
                         && m.AssignedStaffName == staffName
                         && m.ResolvedAt.HasValue
                         && m.ResolvedAt.Value.Date == DateTime.Today)
                .OrderByDescending(m => m.ResolvedAt)
                .Select(m => new MaintenanceTaskViewModel
                {
                    RequestId = m.RequestId,
                    RoomId = m.RoomId,
                    RoomNumber = m.Room != null ? m.Room.RoomNumber : "N/A",
                    RoomType = (m.Room != null && m.Room.Category != null) ? m.Room.Category.CategoryName : "N/A",
                    Description = m.Description,
                    CreatedAt = m.CreatedAt,
                    IsResolved = m.IsResolved,
                    ResolvedAt = m.ResolvedAt,
                    AssignedStaffName = m.AssignedStaffName,
                    // 🟢 GIẢI QUYẾT LỖI DÒNG 129: Kiểm tra và bóc tách giá trị thực .Value hoặc trả về số tầng mặc định
                    FloorNumber = (m.Room != null && m.Room.FloorNumber.HasValue) ? m.Room.FloorNumber.Value : 0
                })
                .ToListAsync();
        }
    }
}