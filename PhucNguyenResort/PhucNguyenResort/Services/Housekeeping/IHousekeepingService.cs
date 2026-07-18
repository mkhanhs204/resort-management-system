using PhucNguyenResort.Enums;
using PhucNguyenResort.Models;
using PhucNguyenResort.ViewModels.Housekeeping;

namespace PhucNguyenResort.Services.Housekeeping
{
    public interface IHousekeepingService
    {
        Task<List<DirtyRoomViewModel>> GetDirtyRoomsAsync();

        Task<bool> UpdateCleaningStatusAsync(int roomId, string currentStaff);

        Task<bool> ReportMaintenanceAsync(int roomId, string description);

        Task<bool> UpdateRoomStatusWithStaffAsync(int roomId, RoomStatus status, string currentStaff);

        Task<bool> UpdateRoomStatusAsync(int roomId, RoomStatus status);

        Task<int> GetCountRoomsNeedCleaningAsync();
        Task<int> GetCountCurrentlyCleaningAsync();
        Task<int> GetCountCleanedTodayAsync();
        Task<List<DirtyRoomViewModel>> GetCleanedRoomsByStaffTodayAsync(string staffName);

        Task<List<RoomCleaningLog>> GetAllCleaningLogsAsync();
        Task<List<MaintenanceRequest>> GetAllMaintenanceRequestsAsync();
    }
}