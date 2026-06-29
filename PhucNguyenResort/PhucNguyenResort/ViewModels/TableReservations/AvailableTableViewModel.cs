namespace PhucNguyenResort.ViewModels.TableReservations;

public class AvailableTableViewModel
{
    public int TableId { get; set; }
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
}
