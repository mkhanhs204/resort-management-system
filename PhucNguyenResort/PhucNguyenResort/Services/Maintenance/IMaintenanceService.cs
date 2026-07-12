using PhucNguyenResort.ViewModels.Maintenance;

namespace PhucNguyenResort.Services.Maintenance
{
    public interface IMaintenanceService
    {
        // Lấy danh sách tác vụ bảo trì đang hoạt động (Pending & Fixing)
        Task<List<MaintenanceTaskViewModel>> GetMaintenanceTasksAsync();

        // Đếm tổng số sự cố phòng đang chờ xử lý (Chưa ai tiếp nhận)
        Task<int> GetPendingTasksCountAsync();

        // Đếm tổng số sự cố đã sửa thành công trong ngày hôm nay của toàn hệ thống
        Task<int> GetResolvedTodayCountAsync();

        // Tiếp nhận tác vụ (Chuyển trạng thái sang Đang sửa)
        Task<bool> StartFixingTaskAsync(int requestId, string staffName);

        // Hoàn thành tác vụ (Đồng bộ trạng thái phòng về NeedsCleaning)
        Task<bool> ResolveTaskAsync(int requestId, string staffName);

        // Lấy danh sách thành tích phòng đã sửa xong hôm nay của RIÊNG nhân viên đang đăng nhập
        Task<List<MaintenanceTaskViewModel>> GetResolvedTasksByStaffTodayAsync(string staffName);
    }
}