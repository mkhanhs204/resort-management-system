using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Models;
using PhucNguyenResort.Services.RoomCategories;

namespace PhucNguyenResort.Services.RoomCategories
{
    public class RoomCategoryService : IRoomCategoryService
    {
        private readonly ResortDbContext _context;

        public RoomCategoryService(ResortDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RoomCategory>> GetAllAsync()
        {
            return await _context.RoomCategories.ToListAsync();
        }

        public async Task<RoomCategory?> GetByIdAsync(int id)
        {
            return await _context.RoomCategories
                .Include(c => c.Images)
                .FirstOrDefaultAsync(m => m.CategoryId == id);
        }

        public async Task<RoomCategory> AddAsync(RoomCategory roomCategory)
        {
            _context.Add(roomCategory);
            await _context.SaveChangesAsync();
            return roomCategory;
        }

        public async Task UpdateAsync(RoomCategory roomCategory)
        {
            _context.Update(roomCategory);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var roomCategory = await _context.RoomCategories.FindAsync(id);
            if (roomCategory != null)
            {
                _context.RoomCategories.Remove(roomCategory);
                await _context.SaveChangesAsync();
            }
        }

        public bool Exists(int id)
        {
            return _context.RoomCategories.Any(e => e.CategoryId == id);
        }

        public async Task<RoomCategoryImage?> GetGalleryImageByIdAsync(int imageId)
        {
            return await _context.RoomCategoryImages.FindAsync(imageId);
        }

        public async Task DeleteGalleryImageAsync(int imageId)
        {
            var image = await _context.RoomCategoryImages.FindAsync(imageId);
            if (image != null)
            {
                _context.RoomCategoryImages.Remove(image);
                await _context.SaveChangesAsync();
            }
        }
    }
}
