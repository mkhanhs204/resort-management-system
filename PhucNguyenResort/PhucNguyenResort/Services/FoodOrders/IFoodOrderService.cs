using PhucNguyenResort.ViewModels.FoodOrders;

namespace PhucNguyenResort.Services.FoodOrders;

public interface IFoodOrderService
{
    Task<RoomServiceOrderViewModel> BuildRoomServiceMenuAsync(string? message = null);
    Task<RoomServiceOrderViewModel?> BuildRoomServiceOrderAsync(int bookingId);
    Task<RoomServiceOrderViewModel?> BuildRoomServiceOrderAsync(int bookingId, int currentUserId);
    Task<FoodOrderCreateResult> CreateFoodOrderAsync(FoodOrderCreateRequest request, int currentUserId);
    Task<FoodOrderDetailViewModel?> GetFoodOrderDetailAsync(int foodOrderId);
    Task<FoodOrderDetailViewModel?> GetFoodOrderDetailAsync(int foodOrderId, int currentUserId);
    Task<List<FoodOrderDetailViewModel>> GetAllFoodOrdersAsync();
    Task<List<FoodOrderDetailViewModel>> GetFoodOrdersForCustomerAsync(int currentUserId);
    Task<FoodOrderActionResult> CancelFoodOrderByCustomerAsync(int foodOrderId, int currentUserId);

    // [UC66] F&B Staff cap nhat trang thai don hang
    Task<FoodOrderActionResult> UpdateOrderStatusAsync(int foodOrderId, Enums.FoodOrderStatus newStatus);

    // [UC69] Ghi no vao tien phong
    Task<FoodOrderActionResult> ChargeToRoomAsync(int foodOrderId);
}
