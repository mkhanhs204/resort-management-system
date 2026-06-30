using PhucNguyenResort.Enums;

namespace PhucNguyenResort.ViewModels.TableReservations;

public class TableReservationStaffListViewModel
{
    public int ReservationId { get; set; }
    public string ReservationCode { get; set; } = string.Empty;
    public int? BookingId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;
    public string TableNumber { get; set; } = string.Empty;
    public string TableName { get; set; } = string.Empty;
    public string? Area { get; set; }
    public string? TableType { get; set; }
    public int Capacity { get; set; }
    public DateTime ReservationDate { get; set; }
    public TimeSpan ReservationTime { get; set; }
    public int DurationMinutes { get; set; }
    public int PartySize { get; set; }
    public string? SpecialRequests { get; set; }
    public TableReservationStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsChargedToRoom { get; set; }
    public decimal ChargedAmount { get; set; }
    public DateTime? ChargedAt { get; set; }
    public bool CanChargeToRoom { get; set; }
    public bool CanConfirm { get; set; }
    public bool CanSeat { get; set; }
    public bool CanComplete { get; set; }
    public bool CanMarkNoShow { get; set; }
    public DateTime? NoShowAvailableAt { get; set; }
    public bool CanCancel { get; set; }
}
