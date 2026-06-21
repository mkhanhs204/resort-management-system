using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Enums;
using PhucNguyenResort.Services.FoodOrders;

namespace PhucNguyenResort.Services.Kots;

public class KotService : IKotService
{
    private readonly ResortDbContext _context;

    public KotService(ResortDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PhucNguyenResort.ViewModels.Kots.KotDetailViewModel>> GetAllKotsAsync(KotStatus? filterStatus = null)
    {
        var query = _context.KOT
            .Include(k => k.FoodOrder)
                .ThenInclude(fo => fo.FoodOrderItems)
                    .ThenInclude(i => i.MenuItem)
            .AsQueryable();

        if (filterStatus.HasValue)
        {
            query = query.Where(k => k.Status == filterStatus.Value);
        }
        else
        {
            // By default, do not show Cancelled KOTs to kitchen
            query = query.Where(k => k.Status != KotStatus.Cancelled);
        }

        var kots = await query.OrderByDescending(k => k.CreatedAt).ToListAsync();

        return kots.Select(k => new PhucNguyenResort.ViewModels.Kots.KotDetailViewModel
        {
            KotId = k.KotId,
            FoodOrderId = k.FoodOrderId,
            Status = k.Status,
            CreatedAt = k.CreatedAt,
            Items = k.FoodOrder?.FoodOrderItems?.Select(i => new PhucNguyenResort.ViewModels.Kots.KotItemViewModel
            {
                MenuItemName = i.MenuItem?.ItemName ?? "Unknown Item",
                Quantity = i.Quantity,
                Notes = null // No Notes column in FoodOrderItem
            }).ToList() ?? new List<PhucNguyenResort.ViewModels.Kots.KotItemViewModel>()
        });
    }

    public async Task<FoodOrderActionResult> UpdateKotStatusAsync(int kotId, KotStatus newStatus)
    {
        var kot = await _context.KOT.FindAsync(kotId);
        if (kot == null)
        {
            return new FoodOrderActionResult { Status = FoodOrderActionStatus.NotFound, ErrorMessage = "KOT not found" };
        }

        if (newStatus == KotStatus.Cancelled)
        {
            return new FoodOrderActionResult { Status = FoodOrderActionStatus.InvalidState, ErrorMessage = "KOT-003: Cannot manually set KOT to Cancelled. Only F&B Staff can cancel orders." };
        }

        bool isValidTransition = false;
        if (kot.Status == KotStatus.Pending && newStatus == KotStatus.Cooking) isValidTransition = true;
        else if (kot.Status == KotStatus.Cooking && newStatus == KotStatus.Ready) isValidTransition = true;
        else if (kot.Status == KotStatus.Ready && newStatus == KotStatus.Completed) isValidTransition = true;

        if (!isValidTransition)
        {
            return new FoodOrderActionResult { Status = FoodOrderActionStatus.InvalidState, ErrorMessage = $"KOT-003: Invalid transition from {kot.Status} to {newStatus}" };
        }

        kot.Status = newStatus;
        await _context.SaveChangesAsync();

        return new FoodOrderActionResult { Status = FoodOrderActionStatus.Success };
    }
}
