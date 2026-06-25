using PhucNguyenResort.Enums;
using PhucNguyenResort.Services.FoodOrders;

namespace PhucNguyenResort.Services.Kots;

public interface IKotService
{
    Task<IEnumerable<PhucNguyenResort.ViewModels.Kots.KotDetailViewModel>> GetAllKotsAsync(KotStatus? filterStatus = null);
    Task<FoodOrderActionResult> UpdateKotStatusAsync(int kotId, KotStatus newStatus);
}
