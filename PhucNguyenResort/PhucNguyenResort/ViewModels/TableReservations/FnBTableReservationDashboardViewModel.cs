namespace PhucNguyenResort.ViewModels.TableReservations;

public class FnBTableReservationDashboardViewModel
{
    public int TotalTables { get; set; }
    public int AvailableTables { get; set; }
    public int OccupiedTables { get; set; }
    public int PendingReservationsToday { get; set; }
    public int ConfirmedReservationsToday { get; set; }
    public IReadOnlyList<TableReservationStaffListViewModel> TodayReservations { get; set; }
        = Array.Empty<TableReservationStaffListViewModel>();
}
