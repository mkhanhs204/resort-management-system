using PhucNguyenResort.ViewModels.FoodOrders;

namespace PhucNguyenResort.Services.FoodOrders;

public interface IFoodOrderService
{
    Task<FoodOrderCreateResult> CreateFoodOrderAsync(FoodOrderCreateRequest request);
}
