using PhucNguyenResort.Enums;
using PhucNguyenResort.ViewModels.Housekeeping;

namespace PhucNguyenResort.Services.Housekeeping
{
    public interface IHousekeepingService
    {
        // 5.3.1. Lấy danh sách phòng bẩn
        Task<List<DirtyRoomViewModel>> GetDirtyRoomsAsync();

        // 5.3.2. Cập nhật đã dọn xong (Nâng cấp nhận thêm nhân viên để đối chiếu bảo mật)
        Task<bool> UpdateCleaningStatusAsync(int roomId, string currentStaff);

        // 5.3.3. Báo hỏng phòng
        Task<bool> ReportMaintenanceAsync(int roomId, string description);

        // Nâng cấp: Tiếp nhận phòng và gán tên nhân viên dọn (Phục vụ tính năng Start Cleaning)
        Task<bool> UpdateRoomStatusWithStaffAsync(int roomId, RoomStatus status, string currentStaff);

        // Giữ nguyên hàm cũ để tương thích ngược nếu Controller cũ có gọi ở chỗ khác
        Task<bool> UpdateRoomStatusAsync(int roomId, RoomStatus status);

        // Thống kê cho giao diện Housekeeping (Nhân viên Buồng phòng)
        Task<int> GetCountRoomsNeedCleaningAsync();
        Task<int> GetCountCurrentlyCleaningAsync();
        Task<int> GetCountCleanedTodayAsync();
        Task<List<DirtyRoomViewModel>> GetCleanedRoomsByStaffTodayAsync(string staffName);
    }
}