using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Models;

namespace PhucNguyenResort.Services.Users
{
    public class UserService : IUserService
    {
        private readonly ResortDbContext _context;

        public UserService(ResortDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllStaffAsync()
        {
            return await _context.Users
                .Include(u => u.Role)
                .Where(u => u.Role != null && u.Role.RoleName != "Customer")
                .ToListAsync();
        }

        public async Task<User?> GetStaffByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserId == id && u.Role != null && u.Role.RoleName != "Customer");
        }

        public async Task<User> AddStaffAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task UpdateStaffAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task ToggleStaffStatusAsync(int id)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserId == id && u.Role != null && u.Role.RoleName != "Customer");

            if (user != null)
            {
                user.IsActive = !user.IsActive;
                await _context.SaveChangesAsync();
            }
        }
    }
}