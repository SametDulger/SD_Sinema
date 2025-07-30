using Microsoft.AspNetCore.Mvc;
using Moq;
using SD_Sinema.API.Controllers;
using SD_Sinema.Business.DTOs;
using SD_Sinema.Business.Services;
using Xunit;

namespace SD_Sinema.Tests.Controllers
{
    public class MoviesControllerTests
    {
        private readonly Mock<IMovieService> _mockMovieService;
        private readonly MoviesController _controller;

        public MoviesControllerTests()
        {
            _mockMovieService = new Mock<IMovieService>();
            _controller = new MoviesController(_mockMovieService.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkResult_WithAllMovies()
        {
            // Arrange
            var movies = new List<MovieDto>
            {
                new MovieDto { Id = 1, Title = "Test Movie 1", Director = "Director 1", Duration = 120 },
                new MovieDto { Id = 2, Title = "Test Movie 2", Director = "Director 2", Duration = 150 }
            };

            _mockMovieService.Setup(x => x.GetAllAsync()).ReturnsAsync(movies);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedMovies = Assert.IsType<List<MovieDto>>(okResult.Value);
            Assert.Equal(2, returnedMovies.Count);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkResult_WithEmptyList()
        {
            // Arrange
            _mockMovieService.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<MovieDto>());

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedMovies = Assert.IsType<List<MovieDto>>(okResult.Value);
            Assert.Empty(returnedMovies);
        }

        [Fact]
        public async Task GetById_WithValidId_ShouldReturnOkResult()
        {
            // Arrange
            var movie = new MovieDto { Id = 1, Title = "Test Movie", Director = "Test Director", Duration = 120 };
            _mockMovieService.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(movie);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedMovie = Assert.IsType<MovieDto>(okResult.Value);
            Assert.Equal(1, returnedMovie.Id);
            Assert.Equal("Test Movie", returnedMovie.Title);
        }

        [Fact]
        public async Task GetById_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            _mockMovieService.Setup(x => x.GetByIdAsync(999)).ReturnsAsync((MovieDto)null);

            // Act
            var result = await _controller.GetById(999);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetById_WithZeroId_ShouldReturnNotFound()
        {
            // Arrange
            _mockMovieService.Setup(x => x.GetByIdAsync(0)).ReturnsAsync((MovieDto)null);

            // Act
            var result = await _controller.GetById(0);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetById_WithNegativeId_ShouldReturnNotFound()
        {
            // Arrange
            _mockMovieService.Setup(x => x.GetByIdAsync(-1)).ReturnsAsync((MovieDto)null);

            // Act
            var result = await _controller.GetById(-1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Create_WithValidMovie_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var createMovieDto = new CreateMovieDto { Title = "New Movie", Director = "New Director", Duration = 130 };
            var createdMovie = new MovieDto { Id = 1, Title = "New Movie", Director = "New Director", Duration = 130 };

            _mockMovieService.Setup(x => x.CreateAsync(createMovieDto)).ReturnsAsync(createdMovie);

            // Act
            var result = await _controller.Create(createMovieDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedMovie = Assert.IsType<MovieDto>(createdAtActionResult.Value);
            Assert.Equal(1, returnedMovie.Id);
            Assert.Equal("New Movie", returnedMovie.Title);
        }

        [Fact]
        public async Task Create_WithInvalidData_ShouldReturnBadRequest()
        {
            // Arrange
            var createMovieDto = new CreateMovieDto { Title = "New Movie", Director = "New Director", Duration = 130 };
            _mockMovieService.Setup(x => x.CreateAsync(createMovieDto))
                .ThrowsAsync(new InvalidOperationException("Invalid data"));

            // Act
            var result = await _controller.Create(createMovieDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid data", badRequestResult.Value);
        }

        [Fact]
        public async Task Create_WithNullDto_ShouldReturnBadRequest()
        {
            // Arrange
            _mockMovieService.Setup(x => x.CreateAsync(null))
                .ThrowsAsync(new ArgumentNullException("createMovieDto"));

            // Act
            var result = await _controller.Create(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task Create_WithEmptyTitle_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var createMovieDto = new CreateMovieDto { Title = "", Director = "New Director", Duration = 130 };
            var createdMovie = new MovieDto { Id = 1, Title = "", Director = "New Director", Duration = 130 };

            _mockMovieService.Setup(x => x.CreateAsync(createMovieDto)).ReturnsAsync(createdMovie);

            // Act
            var result = await _controller.Create(createMovieDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedMovie = Assert.IsType<MovieDto>(createdAtActionResult.Value);
            Assert.Equal("", returnedMovie.Title);
        }

        [Fact]
        public async Task Update_WithValidMovie_ShouldReturnOkResult()
        {
            // Arrange
            var updateMovieDto = new UpdateMovieDto { Title = "Updated Movie", Director = "Updated Director", Duration = 140 };
            var updatedMovie = new MovieDto { Id = 1, Title = "Updated Movie", Director = "Updated Director", Duration = 140 };

            _mockMovieService.Setup(x => x.UpdateAsync(1, updateMovieDto)).ReturnsAsync(updatedMovie);

            // Act
            var result = await _controller.Update(1, updateMovieDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedMovie = Assert.IsType<MovieDto>(okResult.Value);
            Assert.Equal(1, returnedMovie.Id);
            Assert.Equal("Updated Movie", returnedMovie.Title);
        }

        [Fact]
        public async Task Update_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            var updateMovieDto = new UpdateMovieDto { Title = "Updated Movie", Director = "Updated Director", Duration = 140 };
            _mockMovieService.Setup(x => x.UpdateAsync(999, updateMovieDto))
                .ThrowsAsync(new InvalidOperationException("Film bulunamadı."));

            // Act
            var result = await _controller.Update(999, updateMovieDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Film bulunamadı.", notFoundResult.Value);
        }

        [Fact]
        public async Task Update_WithNullDto_ShouldReturnBadRequest()
        {
            // Arrange
            _mockMovieService.Setup(x => x.UpdateAsync(1, null))
                .ThrowsAsync(new ArgumentNullException("updateMovieDto"));

            // Act
            var result = await _controller.Update(1, null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task Update_WithZeroId_ShouldReturnNotFound()
        {
            // Arrange
            var updateMovieDto = new UpdateMovieDto { Title = "Updated Movie", Director = "Updated Director", Duration = 140 };
            _mockMovieService.Setup(x => x.UpdateAsync(0, updateMovieDto))
                .ThrowsAsync(new InvalidOperationException("Film bulunamadı."));

            // Act
            var result = await _controller.Update(0, updateMovieDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task Delete_WithValidId_ShouldReturnNoContent()
        {
            // Arrange
            _mockMovieService.Setup(x => x.DeleteAsync(1, "test", "test reason")).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(1, "test", "test reason");

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            _mockMovieService.Setup(x => x.DeleteAsync(999, "test", "test reason"))
                .ThrowsAsync(new InvalidOperationException("Film bulunamadı."));

            // Act
            var result = await _controller.Delete(999, "test", "test reason");

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Film bulunamadı.", notFoundResult.Value);
        }

        [Fact]
        public async Task Delete_WithEmptyDeletedBy_ShouldReturnNoContent()
        {
            // Arrange
            _mockMovieService.Setup(x => x.DeleteAsync(1, "", "test reason")).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(1, "", "test reason");

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_WithEmptyReason_ShouldReturnNoContent()
        {
            // Arrange
            _mockMovieService.Setup(x => x.DeleteAsync(1, "test", "")).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(1, "test", "");

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GetActive_ShouldReturnOkResult_WithActiveMovies()
        {
            // Arrange
            var movies = new List<MovieDto>
            {
                new MovieDto { Id = 1, Title = "Active Movie 1", IsActive = true },
                new MovieDto { Id = 2, Title = "Active Movie 2", IsActive = true }
            };

            _mockMovieService.Setup(x => x.GetActiveMoviesAsync()).ReturnsAsync(movies);

            // Act
            var result = await _controller.GetActive();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedMovies = Assert.IsType<List<MovieDto>>(okResult.Value);
            Assert.Equal(2, returnedMovies.Count);
        }

        [Fact]
        public async Task GetActive_ShouldReturnOkResult_WithEmptyList()
        {
            // Arrange
            _mockMovieService.Setup(x => x.GetActiveMoviesAsync()).ReturnsAsync(new List<MovieDto>());

            // Act
            var result = await _controller.GetActive();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedMovies = Assert.IsType<List<MovieDto>>(okResult.Value);
            Assert.Empty(returnedMovies);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(999, false)]
        [InlineData(0, false)]
        [InlineData(-1, false)]
        public async Task GetById_WithVariousIds_ShouldReturnExpectedResult(int id, bool shouldExist)
        {
            // Arrange
            if (shouldExist)
            {
                var movie = new MovieDto { Id = id, Title = "Test Movie", Director = "Test Director", Duration = 120 };
                _mockMovieService.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(movie);
            }
            else
            {
                _mockMovieService.Setup(x => x.GetByIdAsync(id)).ReturnsAsync((MovieDto)null);
            }

            // Act
            var result = await _controller.GetById(id);

            // Assert
            if (shouldExist)
            {
                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                var returnedMovie = Assert.IsType<MovieDto>(okResult.Value);
                Assert.Equal(id, returnedMovie.Id);
            }
            else
            {
                Assert.IsType<NotFoundResult>(result.Result);
            }
        }

        [Theory]
        [InlineData("Action", "Director 1", 120)]
        [InlineData("Comedy", "Director 2", 150)]
        [InlineData("Drama", "Director 3", 180)]
        public async Task Create_WithDifferentGenres_ShouldReturnCreatedAtAction(string genre, string director, int duration)
        {
            // Arrange
            var createMovieDto = new CreateMovieDto { Title = $"Test {genre} Movie", Director = director, Duration = duration };
            var createdMovie = new MovieDto { Id = 1, Title = $"Test {genre} Movie", Director = director, Duration = duration };

            _mockMovieService.Setup(x => x.CreateAsync(createMovieDto)).ReturnsAsync(createdMovie);

            // Act
            var result = await _controller.Create(createMovieDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedMovie = Assert.IsType<MovieDto>(createdAtActionResult.Value);
            Assert.Equal($"Test {genre} Movie", returnedMovie.Title);
            Assert.Equal(director, returnedMovie.Director);
            Assert.Equal(duration, returnedMovie.Duration);
        }

        [Theory]
        [InlineData(1, "test", "test reason")]
        [InlineData(2, "admin", "no longer available")]
        [InlineData(999, "system", "maintenance")]
        public async Task Delete_WithVariousParameters_ShouldReturnNoContent(int id, string deletedBy, string reason)
        {
            // Arrange
            _mockMovieService.Setup(x => x.DeleteAsync(id, deletedBy, reason)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(id, deletedBy, reason);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockMovieService.Verify(x => x.DeleteAsync(id, deletedBy, reason), Times.Once);
        }
    }
} 