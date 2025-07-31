using Moq;
using SD_Sinema.Business.Services;
using SD_Sinema.Core.Entities;
using SD_Sinema.Core.Interfaces;
using SD_Sinema.Business.DTOs;
using Xunit;

namespace SD_Sinema.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _userService = new UserService(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, Email = "user1@test.com", FirstName = "John", LastName = "Doe" },
                new User { Id = 2, Email = "user2@test.com", FirstName = "Jane", LastName = "Smith" }
            };

            _mockUnitOfWork.Setup(x => x.Users.GetAllAsync()).ReturnsAsync(users);

            // Act
            var result = await _userService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockUnitOfWork.Verify(x => x.Users.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoUsersExist()
        {
            // Arrange
            _mockUnitOfWork.Setup(x => x.Users.GetAllAsync()).ReturnsAsync(new List<User>());

            // Act
            var result = await _userService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockUnitOfWork.Verify(x => x.Users.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnUser()
        {
            // Arrange
            var user = new User { Id = 1, Email = "test@test.com", FirstName = "Test", LastName = "User" };
            _mockUnitOfWork.Setup(x => x.Users.GetByIdAsync(1)).ReturnsAsync(user);

            // Act
            var result = await _userService.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("test@test.com", result.Email);
            _mockUnitOfWork.Verify(x => x.Users.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            _mockUnitOfWork.Setup(x => x.Users.GetByIdAsync(999)).ReturnsAsync((User?)null);

            // Act
            var result = await _userService.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
            _mockUnitOfWork.Verify(x => x.Users.GetByIdAsync(999), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WithZeroId_ShouldReturnNull()
        {
            // Arrange
            _mockUnitOfWork.Setup(x => x.Users.GetByIdAsync(0)).ReturnsAsync((User?)null);

            // Act
            var result = await _userService.GetByIdAsync(0);

            // Assert
            Assert.Null(result);
            _mockUnitOfWork.Verify(x => x.Users.GetByIdAsync(0), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WithValidUser_ShouldCreateUser()
        {
            // Arrange
            var createUserDto = new CreateUserDto 
            { 
                Email = "newuser@test.com", 
                FirstName = "New",
                LastName = "User",
                Password = "password123"
            };
            
            var user = new User 
            { 
                Id = 1,
                Email = "newuser@test.com", 
                FirstName = "New",
                LastName = "User"
            };

            _mockUnitOfWork.Setup(x => x.Users.AddAsync(It.IsAny<User>())).ReturnsAsync((User u) => { u.Id = 1; return u; });
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _userService.CreateAsync(createUserDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("newuser@test.com", result.Email);
            _mockUnitOfWork.Verify(x => x.Users.AddAsync(It.IsAny<User>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WithNullDto_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _userService.CreateAsync(null!));
        }

        [Fact]
        public async Task CreateAsync_WithEmptyEmail_ShouldCreateUser()
        {
            // Arrange
            var createUserDto = new CreateUserDto 
            { 
                Email = "", 
                FirstName = "Test",
                LastName = "User",
                Password = "password123"
            };
            
            var user = new User 
            { 
                Id = 1,
                Email = "", 
                FirstName = "Test",
                LastName = "User"
            };

            _mockUnitOfWork.Setup(x => x.Users.AddAsync(It.IsAny<User>())).ReturnsAsync((User u) => { u.Id = 1; return u; });
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _userService.CreateAsync(createUserDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("", result.Email);
        }

        [Fact]
        public async Task UpdateAsync_WithValidUser_ShouldUpdateUser()
        {
            // Arrange
            var updateUserDto = new UpdateUserDto 
            { 
                FirstName = "Updated", 
                LastName = "User", 
                Email = "updated@test.com"
            };
            
            var user = new User 
            { 
                Id = 1, 
                Email = "original@test.com",
                FirstName = "Original", 
                LastName = "User"
            };

            _mockUnitOfWork.Setup(x => x.Users.GetByIdAsync(1)).ReturnsAsync(user);
            _mockUnitOfWork.Setup(x => x.Users.UpdateAsync(user)).ReturnsAsync(user);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _userService.UpdateAsync(1, updateUserDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            _mockUnitOfWork.Verify(x => x.Users.GetByIdAsync(1), Times.Once);
            _mockUnitOfWork.Verify(x => x.Users.UpdateAsync(user), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WithInvalidId_ShouldThrowException()
        {
            // Arrange
            var updateUserDto = new UpdateUserDto 
            { 
                FirstName = "Updated", 
                LastName = "User", 
                Email = "updated@test.com"
            };

            _mockUnitOfWork.Setup(x => x.Users.GetByIdAsync(999)).ReturnsAsync((User)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _userService.UpdateAsync(999, updateUserDto));
            _mockUnitOfWork.Verify(x => x.Users.GetByIdAsync(999), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WithNullDto_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => 
                _userService.UpdateAsync(1, null!));
        }

        [Fact]
        public async Task DeleteAsync_WithValidId_ShouldDeleteUser()
        {
            // Arrange
            _mockUnitOfWork.Setup(x => x.Users.DeleteAsync(1, "test", "test reason")).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            await _userService.DeleteAsync(1, "test", "test reason");

            // Assert
            _mockUnitOfWork.Verify(x => x.Users.DeleteAsync(1, "test", "test reason"), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WithEmptyDeletedBy_ShouldDeleteUser()
        {
            // Arrange
            _mockUnitOfWork.Setup(x => x.Users.DeleteAsync(1, "", "test reason")).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            await _userService.DeleteAsync(1, "", "test reason");

            // Assert
            _mockUnitOfWork.Verify(x => x.Users.DeleteAsync(1, "", "test reason"), Times.Once);
        }

        [Fact]
        public async Task ExistsAsync_WithValidId_ShouldReturnTrue()
        {
            // Arrange
            _mockUnitOfWork.Setup(x => x.Users.ExistsAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _userService.ExistsAsync(1);

            // Assert
            Assert.True(result);
            _mockUnitOfWork.Verify(x => x.Users.ExistsAsync(1), Times.Once);
        }

        [Fact]
        public async Task ExistsAsync_WithInvalidId_ShouldReturnFalse()
        {
            // Arrange
            _mockUnitOfWork.Setup(x => x.Users.ExistsAsync(999)).ReturnsAsync(false);

            // Act
            var result = await _userService.ExistsAsync(999);

            // Assert
            Assert.False(result);
            _mockUnitOfWork.Verify(x => x.Users.ExistsAsync(999), Times.Once);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(2, false)]
        [InlineData(999, false)]
        [InlineData(0, false)]
        [InlineData(-1, false)]
        public async Task ExistsAsync_WithVariousIds_ShouldReturnExpectedResult(int id, bool expectedResult)
        {
            // Arrange
            _mockUnitOfWork.Setup(x => x.Users.ExistsAsync(id)).ReturnsAsync(expectedResult);

            // Act
            var result = await _userService.ExistsAsync(id);

            // Assert
            Assert.Equal(expectedResult, result);
            _mockUnitOfWork.Verify(x => x.Users.ExistsAsync(id), Times.Once);
        }

        [Theory]
        [InlineData("john@test.com", "John", "Doe")]
        [InlineData("jane@test.com", "Jane", "Smith")]
        [InlineData("bob@test.com", "Bob", "Wilson")]
        public async Task CreateAsync_WithDifferentUsers_ShouldCreateUser(string email, string firstName, string lastName)
        {
            // Arrange
            var createUserDto = new CreateUserDto 
            { 
                Email = email, 
                FirstName = firstName, 
                LastName = lastName,
                Password = "password123"
            };
            
            var user = new User 
            { 
                Id = 1,
                Email = email, 
                FirstName = firstName, 
                LastName = lastName
            };

            _mockUnitOfWork.Setup(x => x.Users.AddAsync(It.IsAny<User>())).ReturnsAsync((User u) => { u.Id = 1; return u; });
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _userService.CreateAsync(createUserDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(email, result.Email);
            Assert.Equal(firstName, result.FirstName);
            Assert.Equal(lastName, result.LastName);
        }

        [Theory]
        [InlineData("test1@test.com")]
        [InlineData("test2@test.com")]
        [InlineData("test3@test.com")]
        public async Task GetByIdAsync_WithVariousEmails_ShouldReturnExpectedResult(string email)
        {
            // Arrange
            var user = new User { Id = 1, Email = email, FirstName = "Test", LastName = "User" };
            _mockUnitOfWork.Setup(x => x.Users.GetByIdAsync(1)).ReturnsAsync(user);

            // Act
            var result = await _userService.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(email, result.Email);
            _mockUnitOfWork.Verify(x => x.Users.GetByIdAsync(1), Times.Once);
        }
    }
} 