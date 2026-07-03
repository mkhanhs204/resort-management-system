using PhucNguyenResort.Enums;

namespace PhucNguyenResort.ViewModels.Housekeeping
{
    public class DirtyRoomViewModel
    {
        public int RoomId { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public int FloorNumber { get; set; } // Khớp với Room.cs
        public string RoomType { get; set; } = string.Empty;
        public RoomStatus Status { get; set; } // Khớp kiểu Enum của Room.cs
        public string DirtyReason { get; set; } = string.Empty;
        public DateTime? LastCheckedOutTime { get; set; }
    }
}