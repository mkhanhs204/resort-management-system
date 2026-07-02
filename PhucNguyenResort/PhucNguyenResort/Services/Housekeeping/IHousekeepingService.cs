using PhucNguyenResort.ViewModels.Housekeeping;

namespace PhucNguyenResort.Services.Housekeeping
{
    public interface IHousekeepingService
    {
        // 5.3.1. Lấy danh sách phòng bẩn
        Task<List<DirtyRoomViewModel>> GetDirtyRoomsAsync();

        // 5.3.2. Cập nhật đã dọn xong
        Task<bool> UpdateCleaningStatusAsync(int roomId);

        // 5.3.3. Báo hỏng phòng (Có lưu lý do hỏng vào bảng trung gian)
        Task<bool> ReportMaintenanceAsync(int roomId, string description);
    }
}