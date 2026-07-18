using PhucNguyenResort.ViewModels.Maintenance;
using PhucNguyenResort.Web.Models.ViewModels;

namespace PhucNguyenResort.Services.Maintenance
{
    public interface IMaintenanceService
    {
        Task<List<MaintenanceTaskViewModel>> GetMaintenanceTasksAsync();

        Task<int> GetPendingTasksCountAsync();

        Task<int> GetResolvedTodayCountAsync();

        Task<bool> StartFixingTaskAsync(int requestId, string staffName);

        Task<bool> ResolveTaskAsync(int requestId, string staffName);

        Task<List<MaintenanceTaskViewModel>> GetResolvedTasksByStaffTodayAsync(string staffName);

        Task<List<MaintenanceHistoryViewModel>> GetAllMaintenanceHistoryRequestsAsync();

        Task<List<CleaningHistoryViewModel>> GetAllCleaningLogsForMaintenanceAsync();
    }
}