using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Enums;
using PhucNguyenResort.ViewModels.Maintenance;
using PhucNguyenResort.Web.Models.ViewModels;

namespace PhucNguyenResort.Services.Maintenance
{
    public class MaintenanceService : IMaintenanceService
    {
        private readonly ResortDbContext _context;

        public MaintenanceService(ResortDbContext context)
        {
            _context = context;
        }

        public async Task<List<MaintenanceTaskViewModel>> GetMaintenanceTasksAsync()
        {
            await SyncMaintenanceRequestsAsync();

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
                    FloorNumber = (m.Room != null && m.Room.FloorNumber.HasValue) ? m.Room.FloorNumber.Value : 0
                })
                .ToListAsync();
        }

        public async Task<int> GetPendingTasksCountAsync()
        {
            await SyncMaintenanceRequestsAsync();

            return await _context.MaintenanceRequests
                .CountAsync(t => !t.IsResolved && string.IsNullOrEmpty(t.AssignedStaffName));
        }

        public async Task<int> GetResolvedTodayCountAsync()
        {
            return await _context.MaintenanceRequests
                .CountAsync(t => t.IsResolved && t.ResolvedAt.HasValue && t.ResolvedAt.Value.Date == DateTime.Today);
        }

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
                    FloorNumber = (m.Room != null && m.Room.FloorNumber.HasValue) ? m.Room.FloorNumber.Value : 0
                })
                .ToListAsync();
        }

        public async Task<List<MaintenanceHistoryViewModel>> GetAllMaintenanceHistoryRequestsAsync()
        {
            return await _context.MaintenanceRequests
                .Include(m => m.Room)
                    .ThenInclude(r => r!.Category)
                .OrderByDescending(m => m.CreatedAt)
                .Select(req => new MaintenanceHistoryViewModel
                {
                    RequestId = req.RequestId,
                    RoomId = req.RoomId,
                    RoomNumber = req.Room != null ? req.Room.RoomNumber : "N/A",
                    RoomType = (req.Room != null && req.Room.Category != null) ? req.Room.Category.CategoryName : "Chưa phân loại",
                    Description = req.Description,
                    CreatedAt = req.CreatedAt,
                    ResolvedAt = req.IsResolved ? req.CreatedAt.AddHours(1.5) : null,
                    Status = req.IsResolved ? "Đã hoàn thành" : "Đang xử lý",
                    AssignedStaffName = req.AssignedStaffName ?? "Chưa bàn giao"
                })
                .ToListAsync();
        }

        public async Task<List<CleaningHistoryViewModel>> GetAllCleaningLogsForMaintenanceAsync()
        {
            return await _context.RoomCleaningLogs
                .Include(l => l.Room)
                    .ThenInclude(r => r!.Category)
                .OrderByDescending(l => l.CompletedAt)
                .Select(log => new CleaningHistoryViewModel
                {
                    LogId = log.LogId,
                    RoomId = log.RoomId,
                    RoomNumber = log.Room != null ? log.Room.RoomNumber : "N/A",
                    RoomType = (log.Room != null && log.Room.Category != null) ? log.Room.Category.CategoryName : "Chưa phân loại",
                    AssignedStaffName = log.AssignedStaffName,
                    StartedAt = log.StartedAt,
                    CompletedAt = log.CompletedAt,
                    Notes = log.Notes
                })
                .ToListAsync();
        }

        private async Task SyncMaintenanceRequestsAsync()
        {
            var maintenanceRooms = await _context.Rooms
                .Where(r => r.Status == RoomStatus.Maintenance)
                .ToListAsync();

            bool hasChanges = false;
            foreach (var room in maintenanceRooms)
            {
                var existingRequest = await _context.MaintenanceRequests
                    .FirstOrDefaultAsync(m => m.RoomId == room.RoomId && !m.IsResolved);
                
                if (existingRequest == null)
                {
                    var newRequest = new Models.MaintenanceRequest
                    {
                        RoomId = room.RoomId,
                        Description = "Báo hỏng từ danh sách phòng",
                        CreatedAt = DateTime.Now,
                        IsResolved = false
                    };
                    _context.MaintenanceRequests.Add(newRequest);
                    hasChanges = true;
                }
            }

            if (hasChanges)
            {
                await _context.SaveChangesAsync();
            }
        }
    }
}