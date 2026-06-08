using PhucNguyenResort.Models;

namespace PhucNguyenResort.Services.RoomCategories
{
    public interface IRoomCategoryService
    {
        Task<IEnumerable<RoomCategory>> GetAllAsync();
        Task<RoomCategory?> GetByIdAsync(int id);
        Task<RoomCategory> AddAsync(RoomCategory roomCategory);
        Task UpdateAsync(RoomCategory roomCategory);
        Task DeleteAsync(int id);
        bool Exists(int id);
        Task<RoomCategoryImage?> GetGalleryImageByIdAsync(int imageId);
        Task DeleteGalleryImageAsync(int imageId);
    }
}
