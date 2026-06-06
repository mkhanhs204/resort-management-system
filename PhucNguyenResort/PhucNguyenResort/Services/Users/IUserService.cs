using PhucNguyenResort.Models;

namespace PhucNguyenResort.Services.Users
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllStaffAsync();
        Task<User?> GetStaffByIdAsync(int id);
        Task<User> AddStaffAsync(User user);
        Task UpdateStaffAsync(User user);
        Task ToggleStaffStatusAsync(int id);
    }
}
