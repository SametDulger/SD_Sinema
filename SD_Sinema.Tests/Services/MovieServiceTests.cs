using Moq;
using SD_Sinema.Business.Services;
using SD_Sinema.Core.Entities;
using SD_Sinema.Core.Interfaces;
using SD_Sinema.Business.DTOs;
using Xunit;

namespace SD_Sinema.Tests.Services
{
    public class MovieServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly MovieService _movieService;

        public MovieServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _movieService = new MovieService(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllMovies()
        {
            // Arrange
            var movies = new List<Movie>
            {
                new Movie { Id = 1, Title = "Test Movie 1", Director = "Director 1", Duration = 120 },
                new Movie { Id = 2, Title = "Test Movie 2", Director = "Director 2", Duration = 150 }
            };

            _mockUnitOfWork.Setup(x => x.Movies.GetAllWithSessionsAsync()).ReturnsAsync(movies);

            // Act
            var result = await _movieService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockUnitOfWork.Verify(x => x.Movies.GetAllWithSessionsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoMoviesExist()
        {
            // Arrange
            _mockUnitOfWork.Setup(x => x.Movies.GetAllWithSessionsAsync()).ReturnsAsync(new List<Movie>());

            // Act
            var result = await _movieService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockUnitOfWork.Verify(x => x.Movies.GetAllWithSessionsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnMovie()
        {
            // Arrange
            var movie = new Movie { Id = 1, Title = "Test Movie", Director = "Test Director", Duration = 120 };
            _mockUnitOfWork.Setup(x => x.Movies.GetByIdWithSessionsAsync(1)).ReturnsAsync(movie);

            // Act
            var result = await _movieService.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Test Movie", result.Title);
            _mockUnitOfWork.Verify(x => x.Movies.GetByIdWithSessionsAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            _mockUnitOfWork.Setup(x => x.Movies.GetByIdWithSessionsAsync(999)).ReturnsAsync((Movie)null);

            // Act
            var result = await _movieService.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
            _mockUnitOfWork.Verify(x => x.Movies.GetByIdWithSessionsAsync(999), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WithZeroId_ShouldReturnNull()
        {
            // Arrange
            _mockUnitOfWork.Setup(x => x.Movies.GetByIdWithSessionsAsync(0)).ReturnsAsync((Movie)null);

            // Act
            var result = await _movieService.GetByIdAsync(0);

            // Assert
            Assert.Null(result);
            _mockUnitOfWork.Verify(x => x.Movies.GetByIdWithSessionsAsync(0), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WithNegativeId_ShouldReturnNull()
        {
            // Arrange
            _mockUnitOfWork.Setup(x => x.Movies.GetByIdWithSessionsAsync(-1)).ReturnsAsync((Movie)null);

            // Act
            var result = await _movieService.GetByIdAsync(-1);

            // Assert
            Assert.Null(result);
            _mockUnitOfWork.Verify(x => x.Movies.GetByIdWithSessionsAsync(-1), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WithValidMovie_ShouldCreateMovie()
        {
            // Arrange
            var createMovieDto = new CreateMovieDto 
            { 
                Title = "New Movie", 
                Director = "New Director", 
                Duration = 130,
                Description = "Test Description",
                Genre = "Action"
            };
            
            var movie = new Movie 
            { 
                Id = 1,
                Title = "New Movie", 
                Director = "New Director", 
                Duration = 130 
            };

            _mockUnitOfWork.Setup(x => x.Movies.AddAsync(It.IsAny<Movie>())).ReturnsAsync((Movie m) => { m.Id = 1; return m; });
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _movieService.CreateAsync(createMovieDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("New Movie", result.Title);
            _mockUnitOfWork.Verify(x => x.Movies.AddAsync(It.IsAny<Movie>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WithNullDto_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _movieService.CreateAsync(null));
        }

        [Fact]
        public async Task CreateAsync_WithEmptyTitle_ShouldCreateMovie()
        {
            // Arrange
            var createMovieDto = new CreateMovieDto 
            { 
                Title = "", 
                Director = "New Director", 
                Duration = 130,
                Description = "Test Description",
                Genre = "Action"
            };
            
            var movie = new Movie 
            { 
                Id = 1,
                Title = "", 
                Director = "New Director", 
                Duration = 130 
            };

            _mockUnitOfWork.Setup(x => x.Movies.AddAsync(It.IsAny<Movie>())).ReturnsAsync((Movie m) => { m.Id = 1; return m; });
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _movieService.CreateAsync(createMovieDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("", result.Title);
        }

        [Fact]
        public async Task UpdateAsync_WithValidMovie_ShouldUpdateMovie()
        {
            // Arrange
            var updateMovieDto = new UpdateMovieDto 
            { 
                Title = "Updated Movie", 
                Director = "Updated Director", 
                Duration = 140,
                Description = "Updated Description",
                Genre = "Comedy"
            };
            
            var movie = new Movie 
            { 
                Id = 1, 
                Title = "Original Movie", 
                Director = "Original Director", 
                Duration = 120 
            };

            _mockUnitOfWork.Setup(x => x.Movies.GetByIdAsync(1)).ReturnsAsync(movie);
            _mockUnitOfWork.Setup(x => x.Movies.UpdateAsync(movie)).ReturnsAsync(movie);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _movieService.UpdateAsync(1, updateMovieDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            _mockUnitOfWork.Verify(x => x.Movies.GetByIdAsync(1), Times.Once);
            _mockUnitOfWork.Verify(x => x.Movies.UpdateAsync(movie), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WithInvalidId_ShouldThrowException()
        {
            // Arrange
            var updateMovieDto = new UpdateMovieDto 
            { 
                Title = "Updated Movie", 
                Director = "Updated Director", 
                Duration = 140 
            };

            _mockUnitOfWork.Setup(x => x.Movies.GetByIdAsync(999)).ReturnsAsync((Movie)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _movieService.UpdateAsync(999, updateMovieDto));
            _mockUnitOfWork.Verify(x => x.Movies.GetByIdAsync(999), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WithNullDto_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => 
                _movieService.UpdateAsync(1, null));
        }

        [Fact]
        public async Task UpdateAsync_WithZeroId_ShouldThrowException()
        {
            // Arrange
            var updateMovieDto = new UpdateMovieDto 
            { 
                Title = "Updated Movie", 
                Director = "Updated Director", 
                Duration = 140 
            };

            _mockUnitOfWork.Setup(x => x.Movies.GetByIdAsync(0)).ReturnsAsync((Movie)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _movieService.UpdateAsync(0, updateMovieDto));
        }

        [Fact]
        public async Task DeleteAsync_WithValidId_ShouldDeleteMovie()
        {
            // Arrange
            _mockUnitOfWork.Setup(x => x.Movies.DeleteAsync(1, "test", "test reason")).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            await _movieService.DeleteAsync(1, "test", "test reason");

            // Assert
            _mockUnitOfWork.Verify(x => x.Movies.DeleteAsync(1, "test", "test reason"), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WithEmptyDeletedBy_ShouldDeleteMovie()
        {
            // Arrange
            _mockUnitOfWork.Setup(x => x.Movies.DeleteAsync(1, "", "test reason")).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            await _movieService.DeleteAsync(1, "", "test reason");

            // Assert
            _mockUnitOfWork.Verify(x => x.Movies.DeleteAsync(1, "", "test reason"), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WithEmptyReason_ShouldDeleteMovie()
        {
            // Arrange
            _mockUnitOfWork.Setup(x => x.Movies.DeleteAsync(1, "test", "")).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            await _movieService.DeleteAsync(1, "test", "");

            // Assert
            _mockUnitOfWork.Verify(x => x.Movies.DeleteAsync(1, "test", ""), Times.Once);
        }

        [Fact]
        public async Task ExistsAsync_WithValidId_ShouldReturnTrue()
        {
            // Arrange
            _mockUnitOfWork.Setup(x => x.Movies.ExistsAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _movieService.ExistsAsync(1);

            // Assert
            Assert.True(result);
            _mockUnitOfWork.Verify(x => x.Movies.ExistsAsync(1), Times.Once);
        }

        [Fact]
        public async Task ExistsAsync_WithInvalidId_ShouldReturnFalse()
        {
            // Arrange
            _mockUnitOfWork.Setup(x => x.Movies.ExistsAsync(999)).ReturnsAsync(false);

            // Act
            var result = await _movieService.ExistsAsync(999);

            // Assert
            Assert.False(result);
            _mockUnitOfWork.Verify(x => x.Movies.ExistsAsync(999), Times.Once);
        }

        [Fact]
        public async Task ExistsAsync_WithZeroId_ShouldReturnFalse()
        {
            // Arrange
            _mockUnitOfWork.Setup(x => x.Movies.ExistsAsync(0)).ReturnsAsync(false);

            // Act
            var result = await _movieService.ExistsAsync(0);

            // Assert
            Assert.False(result);
            _mockUnitOfWork.Verify(x => x.Movies.ExistsAsync(0), Times.Once);
        }

        [Fact]
        public async Task GetActiveMoviesAsync_ShouldReturnActiveMovies()
        {
            // Arrange
            var movies = new List<Movie>
            {
                new Movie { Id = 1, Title = "Active Movie 1", IsActive = true },
                new Movie { Id = 2, Title = "Active Movie 2", IsActive = true }
            };

            _mockUnitOfWork.Setup(x => x.Movies.GetActiveMoviesAsync()).ReturnsAsync(movies);

            // Act
            var result = await _movieService.GetActiveMoviesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockUnitOfWork.Verify(x => x.Movies.GetActiveMoviesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetActiveMoviesAsync_ShouldReturnEmptyList_WhenNoActiveMovies()
        {
            // Arrange
            _mockUnitOfWork.Setup(x => x.Movies.GetActiveMoviesAsync()).ReturnsAsync(new List<Movie>());

            // Act
            var result = await _movieService.GetActiveMoviesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockUnitOfWork.Verify(x => x.Movies.GetActiveMoviesAsync(), Times.Once);
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
            _mockUnitOfWork.Setup(x => x.Movies.ExistsAsync(id)).ReturnsAsync(expectedResult);

            // Act
            var result = await _movieService.ExistsAsync(id);

            // Assert
            Assert.Equal(expectedResult, result);
            _mockUnitOfWork.Verify(x => x.Movies.ExistsAsync(id), Times.Once);
        }

        [Theory]
        [InlineData("Action", "Director 1", 120)]
        [InlineData("Comedy", "Director 2", 150)]
        [InlineData("Drama", "Director 3", 180)]
        public async Task CreateAsync_WithDifferentGenres_ShouldCreateMovie(string genre, string director, int duration)
        {
            // Arrange
            var createMovieDto = new CreateMovieDto 
            { 
                Title = $"Test {genre} Movie", 
                Director = director, 
                Duration = duration,
                Description = $"Test {genre} Description",
                Genre = genre
            };
            
            var movie = new Movie 
            { 
                Id = 1,
                Title = $"Test {genre} Movie", 
                Director = director, 
                Duration = duration 
            };

            _mockUnitOfWork.Setup(x => x.Movies.AddAsync(It.IsAny<Movie>())).ReturnsAsync((Movie m) => { m.Id = 1; return m; });
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _movieService.CreateAsync(createMovieDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal($"Test {genre} Movie", result.Title);
            Assert.Equal(director, result.Director);
            Assert.Equal(duration, result.Duration);
        }
    }
} 