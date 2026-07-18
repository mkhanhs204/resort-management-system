namespace PhucNguyenResort.Web.Models.ViewModels
{
    public class MaintenanceHistoryViewModel
    {
        public int RequestId { get; set; }
        public int RoomId { get; set; }

        public string RoomNumber { get; set; } = string.Empty;
        public string RoomType { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }

        public string? AssignedStaffName { get; set; }
    }
}