using PhucNguyenResort.Models;

namespace PhucNguyenResort.Services.Users
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User> AddStaffAsync(User user);
        Task UpdateStaffAsync(User user);
        Task ToggleUserStatusAsync(int id);
        Task ToggleUserBlockStatusAsync(int id);
    }
}
