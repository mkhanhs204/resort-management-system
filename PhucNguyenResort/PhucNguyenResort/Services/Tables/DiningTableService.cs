using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Models;
using PhucNguyenResort.ViewModels.Tables;

namespace PhucNguyenResort.Services.Tables;

public class DiningTableService : IDiningTableService
{
    private readonly ResortDbContext _context;

    public DiningTableService(ResortDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DiningTable>> GetAllAsync()
    {
        return await _context.DiningTables
            .OrderBy(t => t.DisplayOrder)
            .ThenBy(t => t.TableNumber)
            .ToListAsync();
    }

    public async Task<DiningTable?> GetByIdAsync(int id)
    {
        return await _context.DiningTables.FindAsync(id);
    }

    public async Task<DiningTable> CreateAsync(TableCreateVM model)
    {
        var table = new DiningTable
        {
            TableNumber = model.TableNumber,
            TableName = NormalizeDisplayName(model.TableName, model.TableNumber),
            ImageUrl = NormalizeNullableText(model.ImageUrl),
            Description = NormalizeNullableText(model.Description),
            Area = NormalizeNullableText(model.Area),
            TableType = NormalizeNullableText(model.TableType),
            LocationNote = NormalizeNullableText(model.LocationNote),
            Capacity = model.Capacity,
            Status = model.Status,
            IsReservable = model.IsReservable,
            DisplayOrder = model.DisplayOrder
        };

        _context.DiningTables.Add(table);
        await _context.SaveChangesAsync();

        return table;
    }

    public async Task<bool> UpdateAsync(TableEditVM model)
    {
        var table = await _context.DiningTables.FindAsync(model.TableId);
        if (table == null) return false;

        table.TableNumber = model.TableNumber;
        table.TableName = NormalizeDisplayName(model.TableName, model.TableNumber);
        table.ImageUrl = NormalizeNullableText(model.ImageUrl);
        table.Description = NormalizeNullableText(model.Description);
        table.Area = NormalizeNullableText(model.Area);
        table.TableType = NormalizeNullableText(model.TableType);
        table.LocationNote = NormalizeNullableText(model.LocationNote);
        table.Capacity = model.Capacity;
        table.Status = model.Status;
        table.IsReservable = model.IsReservable;
        table.DisplayOrder = model.DisplayOrder;

        _context.DiningTables.Update(table);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var table = await _context.DiningTables.FindAsync(id);
        if (table == null) return false;

        // Check if there are active reservations before deleting
        var hasReservations = await _context.TableReservations
            .AnyAsync(r => r.TableId == id);
            
        if (hasReservations)
        {
            throw new InvalidOperationException("Cannot delete this table because it has reservations.");
        }

        _context.DiningTables.Remove(table);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ExistsByTableNumberAsync(string tableNumber, int? excludeTableId = null)
    {
        var query = _context.DiningTables.Where(t => t.TableNumber == tableNumber);
        
        if (excludeTableId.HasValue)
        {
            query = query.Where(t => t.TableId != excludeTableId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task<int> GetReservationCountAsync(int tableId)
    {
        return await _context.TableReservations
            .CountAsync(r => r.TableId == tableId);
    }

    private static string NormalizeDisplayName(string? tableName, string? tableNumber)
    {
        if (!string.IsNullOrWhiteSpace(tableName))
        {
            return tableName.Trim();
        }

        return string.IsNullOrWhiteSpace(tableNumber)
            ? "Dining Table"
            : $"Table {tableNumber.Trim()}";
    }

    private static string? NormalizeNullableText(string? value)
    {
        var trimmedValue = value?.Trim();
        return string.IsNullOrWhiteSpace(trimmedValue) ? null : trimmedValue;
    }
}
