using PhucNguyenResort.Enums;
using System.ComponentModel.DataAnnotations;

namespace PhucNguyenResort.Models;

public class Room
{
    public int RoomId { get; set; }

    [StringLength(20)]
    public string? RoomNumber { get; set; }

    public int CategoryId { get; set; }

    [Range(0, 200)]
    public int? FloorNumber { get; set; }

    public RoomStatus Status { get; set; } = RoomStatus.Available;

    // BỔ SUNG: Lưu vết tên tài khoản nhân viên đang dọn dẹp phòng này
    [StringLength(100)]
    public string? AssignedStaffName { get; set; }

    public RoomCategory? Category { get; set; }
    public ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();
}