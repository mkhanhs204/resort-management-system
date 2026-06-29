using System.ComponentModel.DataAnnotations;

namespace PhucNguyenResort.ViewModels.TableReservations;

public class TableReservationSearchViewModel
{
    [DataType(DataType.Date)]
    public DateTime? ReservationDate { get; set; }

    [DataType(DataType.Time)]
    public TimeSpan? ReservationTime { get; set; }

    [Range(1, 50)]
    public int PartySize { get; set; }

    public List<PublicDiningTableViewModel> Tables { get; set; } = new();

    public List<AvailableTableViewModel> AvailableTables { get; set; } = new();
}
