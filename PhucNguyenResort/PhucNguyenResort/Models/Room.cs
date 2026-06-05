using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PhucNguyenResort.Enums;

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

    public RoomCategory? Category { get; set; }
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
