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
            .OrderBy(t => t.TableNumber)
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
            Capacity = model.Capacity,
            Status = model.Status
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
        table.Capacity = model.Capacity;
        table.Status = model.Status;

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
}
