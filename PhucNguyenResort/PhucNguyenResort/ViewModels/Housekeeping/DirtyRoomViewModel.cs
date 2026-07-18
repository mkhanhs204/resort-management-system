using PhucNguyenResort.Enums;

namespace PhucNguyenResort.ViewModels.Housekeeping
{
    public class DirtyRoomViewModel
    {
        public int RoomId { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public int FloorNumber { get; set; }
        public string RoomType { get; set; } = string.Empty;
        public RoomStatus Status { get; set; }
        public string DirtyReason { get; set; } = string.Empty;
        public DateTime? LastCheckedOutTime { get; set; }

        public string? AssignedStaffName { get; set; }
    }
}