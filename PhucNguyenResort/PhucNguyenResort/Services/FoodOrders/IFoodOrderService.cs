using PhucNguyenResort.ViewModels.FoodOrders;

namespace PhucNguyenResort.Services.FoodOrders;

public interface IFoodOrderService
{
    Task<RoomServiceOrderViewModel?> BuildRoomServiceOrderAsync(int bookingId);
    Task<FoodOrderCreateResult> CreateFoodOrderAsync(FoodOrderCreateRequest request);
    Task<FoodOrderDetailViewModel?> GetFoodOrderDetailAsync(int foodOrderId);
}
