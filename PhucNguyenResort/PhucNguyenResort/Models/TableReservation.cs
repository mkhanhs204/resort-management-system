using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.Models;

public class TableReservation
{
    public int ReservationId { get; set; }
    public int UserId { get; set; }
    public int TableId { get; set; }

    [DataType(DataType.Date)]
    public DateTime? ReservationDate { get; set; }

    public TimeSpan? ReservationTime { get; set; }

    [Range(1, 50)]
    public int? PartySize { get; set; }

    public TableReservationStatus Status { get; set; } = TableReservationStatus.Pending;

    public User? User { get; set; }
    public DiningTable? Table { get; set; }
}
