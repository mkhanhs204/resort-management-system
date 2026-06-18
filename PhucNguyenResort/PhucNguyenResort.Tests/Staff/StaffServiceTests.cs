using System;
using System.Threading.Tasks;
using Moq;
using Xunit;
using PhucNguyenResort.Models;

namespace PhucNguyenResort.Tests.Staff
{
    // DTO
    public class CreateStaffRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public int RoleId { get; set; }
    }
    
    // Exceptions
    public class EmailAlreadyExistsException : Exception { public EmailAlreadyExistsException(string msg) : base(msg) { } }
    public class UserNotFoundException : Exception { public UserNotFoundException(string msg) : base(msg) { } }

    // Interfaces (To be implemented by real repositories / hasher)
    public interface IUserRepository
    {
        Task<User?> FindByEmailAsync(string email);
        Task<User?> FindByIdAsync(int userId);
        Task<User> AddAsync(User user);
        Task UpdateAsync(User user);
    }
    
    public interface IPasswordHasher
    {
        string HashPassword(string password);
    }

    // Service Under Test
    public class StaffService
    {
        private readonly IUserRepository _userRepo;
        private readonly IPasswordHasher _hasher;

        public StaffService(IUserRepository userRepo, IPasswordHasher hasher)
        {
            _userRepo = userRepo;
            _hasher = hasher;
        }

        public async Task<User> CreateStaffAccountAsync(CreateStaffRequest request)
        {
            var existing = await _userRepo.FindByEmailAsync(request.Email);
            if (existing != null)
                throw new EmailAlreadyExistsException("STF-001: Email already exists");

            var user = new User
            {
                Email = request.Email,
                FullName = request.FullName,
                RoleId = request.RoleId,
                PasswordHash = _hasher.HashPassword(request.Password),
                IsActive = true // active by default
            };

            return await _userRepo.AddAsync(user);
        }

        public async Task<User> ToggleAccountStatusAsync(int userId, bool isActive)
        {
            var user = await _userRepo.FindByIdAsync(userId);
            if (user == null) throw new UserNotFoundException("STF-003: User not found");

            user.IsActive = isActive;
            await _userRepo.UpdateAsync(user);
            return user;
        }
    }

    // TDD Unit Tests matching ManagerStaff_TDD.md
    public class StaffServiceTests
    {
        private readonly Mock<IUserRepository> _mockRepo;
        private readonly Mock<IPasswordHasher> _mockHasher;
        private readonly StaffService _service;

        public StaffServiceTests()
        {
            _mockRepo = new Mock<IUserRepository>();
            _mockHasher = new Mock<IPasswordHasher>();
            _service = new StaffService(_mockRepo.Object, _mockHasher.Object);
        }

        [Fact]
        public async Task MANAGERSTAFF_TC_001_CreateStaffAccount_HappyPath()
        {
            // Arrange
            var req = new CreateStaffRequest { Email = "reception1@resort.com", Password = "Sec", FullName = "Nguyen Van A", RoleId = 3 };
            _mockRepo.Setup(r => r.FindByEmailAsync(req.Email)).ReturnsAsync((User?)null);
            _mockHasher.Setup(h => h.HashPassword(req.Password)).Returns("hashed_pwd");
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<User>())).ReturnsAsync((User u) => u);

            // Act
            var result = await _service.CreateStaffAccountAsync(req);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("reception1@resort.com", result.Email);
            Assert.Equal("hashed_pwd", result.PasswordHash); // Ensure password is NOT plain text
            Assert.Equal(3, result.RoleId);
            Assert.True(result.IsActive);
        }

        [Fact]
        public async Task MANAGERSTAFF_TC_002_PreventDuplicateEmail()
        {
            // Arrange
            var req = new CreateStaffRequest { Email = "admin@resort.com" };
            _mockRepo.Setup(r => r.FindByEmailAsync(req.Email)).ReturnsAsync(new User { Email = req.Email });

            // Act & Assert
            var ex = await Assert.ThrowsAsync<EmailAlreadyExistsException>(() => _service.CreateStaffAccountAsync(req));
            Assert.Contains("STF-001", ex.Message);
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task MANAGERSTAFF_TC_003_BlockAccountSuccessfully()
        {
            // Arrange
            var user = new User { UserId = 1, IsActive = true };
            _mockRepo.Setup(r => r.FindByIdAsync(1)).ReturnsAsync(user);

            // Act (Toggle IsActive to false means Block)
            var result = await _service.ToggleAccountStatusAsync(1, false);

            // Assert
            Assert.False(result.IsActive);
            _mockRepo.Verify(r => r.UpdateAsync(It.Is<User>(u => u.IsActive == false)), Times.Once);
        }
    }
}
