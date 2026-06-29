using PhucNguyenResort.Enums;

namespace PhucNguyenResort.ViewModels.TableReservations;

public class TableReservationListViewModel
{
    public int ReservationId { get; set; }
    public string ReservationCode { get; set; } = string.Empty;
    public string TableNumber { get; set; } = string.Empty;
    public string TableName { get; set; } = string.Empty;
    public string? Area { get; set; }
    public string? TableType { get; set; }
    public DateTime ReservationDate { get; set; }
    public TimeSpan ReservationTime { get; set; }
    public int PartySize { get; set; }
    public TableReservationStatus Status { get; set; }
    public bool CanCancel { get; set; }
}
