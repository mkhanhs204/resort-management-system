using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PhucNguyenResort.DAL.Enums;

namespace PhucNguyenResort.DAL.Entities;

public class DiningTable
{
    public int TableId { get; set; }

    [StringLength(20)]
    public string? TableNumber { get; set; }

    [Range(1, 50)]
    public int? Capacity { get; set; }

    public DiningTableStatus Status { get; set; } = DiningTableStatus.Available;

    public ICollection<TableReservation> TableReservations { get; set; } = new List<TableReservation>();
}
