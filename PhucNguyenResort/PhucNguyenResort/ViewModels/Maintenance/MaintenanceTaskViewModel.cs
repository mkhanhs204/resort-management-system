namespace PhucNguyenResort.ViewModels.Maintenance
{
    public class MaintenanceTaskViewModel
    {
        public int RequestId { get; set; }
        public int RoomId { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public string RoomType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsResolved { get; set; }
        public DateTime? ResolvedAt { get; set; }

        // 🟢 BẮT BUỘC THÊM 2 THUỘC TÍNH NÀY ĐỂ SỬA LỖI BUILD FAILED
        public string? AssignedStaffName { get; set; }
        public int FloorNumber { get; set; }
    }
}