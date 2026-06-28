using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.Models;

public class DiningTable
{
    public int TableId { get; set; }

    [StringLength(20)]
    public string? TableNumber { get; set; }

    [Required, StringLength(100)]
    public string TableName { get; set; } = string.Empty;

    [StringLength(255)]
    public string? ImageUrl { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(50)]
    public string? Area { get; set; }

    [StringLength(50)]
    public string? TableType { get; set; }

    [StringLength(150)]
    public string? LocationNote { get; set; }

    [Range(1, 50)]
    public int? Capacity { get; set; }

    public DiningTableStatus Status { get; set; } = DiningTableStatus.Available;
    public bool IsReservable { get; set; } = true;
    public int DisplayOrder { get; set; } = 0;

    public ICollection<TableReservation> TableReservations { get; set; } = new List<TableReservation>();
}
