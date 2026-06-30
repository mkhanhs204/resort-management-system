using PhucNguyenResort.ViewModels.TableReservations;

namespace PhucNguyenResort.Services.TableReservations;

public interface ITableReservationService
{
    Task<IReadOnlyList<AvailableTableViewModel>> SearchAvailableTablesAsync(
        TableReservationSearchViewModel search);

    Task<IReadOnlyList<PublicDiningTableViewModel>> GetPublicDiningTablesAsync();

    Task<TableReservationCreateResult> CreateReservationAsync(
        TableReservationCreateViewModel model,
        int currentUserId);

    Task<TableReservationDetailViewModel?> GetReservationDetailAsync(
        int reservationId,
        int currentUserId);

    Task<IReadOnlyList<TableReservationListViewModel>> GetReservationsForCustomerAsync(
        int currentUserId);

    Task<bool> HasTableConflictAsync(
        int tableId,
        DateTime reservationDate,
        TimeSpan reservationTime,
        int durationMinutes,
        int? excludeReservationId = null);

    Task<bool> CancelReservationAsync(
        int reservationId,
        int currentUserId,
        string? reason);

    Task<FnBTableReservationDashboardViewModel> GetFnBDashboardAsync(DateTime date);

    Task<IReadOnlyList<TableReservationStaffListViewModel>> GetStaffReservationsAsync(
        DateTime? date = null,
        PhucNguyenResort.Enums.TableReservationStatus? status = null);

    Task<bool> ConfirmReservationAsync(int reservationId, int staffUserId);

    Task<bool> SeatReservationAsync(int reservationId);

    Task<bool> CompleteReservationAsync(int reservationId);

    Task<bool> MarkReservationNoShowAsync(int reservationId);

    Task<bool> CancelReservationByStaffAsync(
        int reservationId,
        int staffUserId,
        string? reason);

    Task<TableReservationActionResult> ChargeReservationToRoomAsync(
        int reservationId,
        decimal amount);
}
