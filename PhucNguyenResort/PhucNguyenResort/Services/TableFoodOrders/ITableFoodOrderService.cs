using PhucNguyenResort.Services.FoodOrders;
using PhucNguyenResort.ViewModels.TableFoodOrders;

namespace PhucNguyenResort.Services.TableFoodOrders;

public interface ITableFoodOrderService
{
    Task<TableFoodOrderCreateViewModel?> BuildCreateOrderAsync(int reservationId);
    Task<FoodOrderCreateResult> CreateTableFoodOrderAsync(TableFoodOrderCreateViewModel model, int staffUserId);
    Task<IReadOnlyList<TableFoodOrderListViewModel>> GetOrdersByReservationAsync(int reservationId);
    Task<TableFoodOrderCheckoutViewModel?> BuildCheckoutAsync(int reservationId);
    Task<TableFoodOrderActionResult> ChargeToRoomAsync(int reservationId, int bookingId);
    Task<TableFoodOrderActionResult> PayAtRestaurantAsync(int reservationId);
}
