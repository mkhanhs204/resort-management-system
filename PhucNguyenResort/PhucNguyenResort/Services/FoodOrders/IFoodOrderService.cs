using PhucNguyenResort.ViewModels.FoodOrders;

namespace PhucNguyenResort.Services.FoodOrders;

public interface IFoodOrderService
{
    Task<RoomServiceOrderViewModel?> BuildRoomServiceOrderAsync(int bookingId);
    Task<RoomServiceOrderViewModel?> BuildRoomServiceOrderAsync(int bookingId, int currentUserId);
    Task<FoodOrderCreateResult> CreateFoodOrderAsync(FoodOrderCreateRequest request, int currentUserId);
    Task<FoodOrderDetailViewModel?> GetFoodOrderDetailAsync(int foodOrderId);
    Task<FoodOrderDetailViewModel?> GetFoodOrderDetailAsync(int foodOrderId, int currentUserId);
}
