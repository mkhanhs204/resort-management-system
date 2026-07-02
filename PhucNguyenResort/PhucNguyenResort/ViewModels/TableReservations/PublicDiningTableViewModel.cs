using PhucNguyenResort.Enums;

namespace PhucNguyenResort.ViewModels.TableReservations;

public class PublicDiningTableViewModel
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
    public DiningTableStatus Status { get; set; }
    public bool IsReservable { get; set; }
}
