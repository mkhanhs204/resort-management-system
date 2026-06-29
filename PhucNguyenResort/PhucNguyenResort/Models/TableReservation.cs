using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.Models;

public class TableReservation
{
    public int ReservationId { get; set; }
    public string ReservationCode { get; set; } = string.Empty;
    public int UserId { get; set; }
    public int TableId { get; set; }
    public int? BookingId { get; set; }

    [DataType(DataType.Date)]
    public DateTime ReservationDate { get; set; }

    public TimeSpan ReservationTime { get; set; }

    public int DurationMinutes { get; set; } = 120;

    [Range(1, 50)]
    public int PartySize { get; set; }

    [StringLength(500)]
    public string? SpecialRequests { get; set; }

    public TableReservationStatus Status { get; set; } = TableReservationStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ConfirmedAt { get; set; }
    public int? ConfirmedByUserId { get; set; }
    public DateTime? CancelledAt { get; set; }

    [StringLength(500)]
    public string? CancellationReason { get; set; }

    public bool IsChargedToRoom { get; set; } = false;

    [Range(0, 999999999)]
    public decimal ChargedAmount { get; set; } = 0;

    public DateTime? ChargedAt { get; set; }

    public User? User { get; set; }
    public DiningTable? Table { get; set; }
    public Booking? Booking { get; set; }
    public User? ConfirmedByUser { get; set; }
}
