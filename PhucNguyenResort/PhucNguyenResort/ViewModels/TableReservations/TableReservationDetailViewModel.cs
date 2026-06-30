using PhucNguyenResort.Enums;

namespace PhucNguyenResort.ViewModels.TableReservations;

public class TableReservationDetailViewModel
{
    public int ReservationId { get; set; }
    public string ReservationCode { get; set; } = string.Empty;
    public int? BookingId { get; set; }
    public string TableNumber { get; set; } = string.Empty;
    public string TableName { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? Description { get; set; }
    public string? Area { get; set; }
    public string? TableType { get; set; }
    public string? LocationNote { get; set; }
    public int Capacity { get; set; }
    public DateTime ReservationDate { get; set; }
    public TimeSpan ReservationTime { get; set; }
    public int DurationMinutes { get; set; }
    public int PartySize { get; set; }
    public string? SpecialRequests { get; set; }
    public TableReservationStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ConfirmedAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    public string? CancellationReason { get; set; }
    public bool IsChargedToRoom { get; set; }
    public decimal ChargedAmount { get; set; }
    public DateTime? ChargedAt { get; set; }
}
