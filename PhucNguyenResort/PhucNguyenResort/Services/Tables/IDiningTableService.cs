using PhucNguyenResort.Models;
using PhucNguyenResort.ViewModels.Tables;

namespace PhucNguyenResort.Services.Tables;

public interface IDiningTableService
{
    Task<IEnumerable<DiningTable>> GetAllAsync();
    Task<DiningTable?> GetByIdAsync(int id);
    Task<DiningTable> CreateAsync(TableCreateVM model);
    Task<bool> UpdateAsync(TableEditVM model);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsByTableNumberAsync(string tableNumber, int? excludeTableId = null);
    Task<int> GetReservationCountAsync(int tableId);
}
