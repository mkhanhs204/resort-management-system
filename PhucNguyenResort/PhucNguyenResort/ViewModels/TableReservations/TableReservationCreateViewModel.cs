using System.ComponentModel.DataAnnotations;

namespace PhucNguyenResort.ViewModels.TableReservations;

public class TableReservationCreateViewModel
{
    [Required]
    public int TableId { get; set; }

    public string TableNumber { get; set; } = string.Empty;
    public string TableName { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? Description { get; set; }
    public string? Area { get; set; }
    public string? TableType { get; set; }
    public string? LocationNote { get; set; }
    public int Capacity { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime ReservationDate { get; set; }

    [Required]
    [DataType(DataType.Time)]
    public TimeSpan ReservationTime { get; set; }

    [Range(1, 480)]
    public int DurationMinutes { get; set; } = 120;

    [Required]
    [Range(1, 50)]
    public int PartySize { get; set; }

    [StringLength(500)]
    public string? SpecialRequests { get; set; }
}
