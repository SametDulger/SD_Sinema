using Microsoft.EntityFrameworkCore;
using SD_Sinema.Core.Entities;
using SD_Sinema.Data.Context;
using SD_Sinema.Data.Repositories;
using Xunit;

namespace SD_Sinema.Tests.Repositories
{
    public class MovieRepositoryTests : IDisposable
    {
        private readonly SinemaDbContext _context;
        private readonly MovieRepository _repository;

        public MovieRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<SinemaDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new SinemaDbContext(options);
            _repository = new MovieRepository(_context);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllMovies()
        {
            // Arrange
            var movies = new List<Movie>
            {
                new Movie { Title = "Test Movie 1", Director = "Director 1", Duration = 120 },
                new Movie { Title = "Test Movie 2", Director = "Director 2", Duration = 150 }
            };

            await _context.Movies.AddRangeAsync(movies);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, m => m.Title == "Test Movie 1");
            Assert.Contains(result, m => m.Title == "Test Movie 2");
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoMoviesExist()
        {
            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllAsync_ShouldNotReturnDeletedMovies()
        {
            // Arrange
            var movies = new List<Movie>
            {
                new Movie { Title = "Active Movie", Director = "Director 1", Duration = 120 },
                new Movie { Title = "Deleted Movie", Director = "Director 2", Duration = 150, IsDeleted = true }
            };

            await _context.Movies.AddRangeAsync(movies);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Single(result);
            Assert.Contains(result, m => m.Title == "Active Movie");
            Assert.DoesNotContain(result, m => m.Title == "Deleted Movie");
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnMovie()
        {
            // Arrange
            var movie = new Movie { Title = "Test Movie", Director = "Test Director", Duration = 120 };
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(movie.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Movie", result.Title);
            Assert.Equal("Test Director", result.Director);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            // Act
            var result = await _repository.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByIdAsync_WithZeroId_ShouldReturnNull()
        {
            // Act
            var result = await _repository.GetByIdAsync(0);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByIdAsync_WithNegativeId_ShouldReturnNull()
        {
            // Act
            var result = await _repository.GetByIdAsync(-1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByIdAsync_WithDeletedMovie_ShouldReturnNull()
        {
            // Arrange
            var movie = new Movie { Title = "Deleted Movie", Director = "Test Director", Duration = 120, IsDeleted = true };
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(movie.Id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_ShouldAddMovie()
        {
            // Arrange
            var movie = new Movie { Title = "New Movie", Director = "New Director", Duration = 130 };

            // Act
            var result = await _repository.AddAsync(movie);
            await _context.SaveChangesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(0, result.Id);
            Assert.Equal("New Movie", result.Title);
            Assert.NotNull(result.CreatedDate);

            var savedMovie = await _context.Movies.FindAsync(result.Id);
            Assert.NotNull(savedMovie);
            Assert.Equal("New Movie", savedMovie.Title);
        }

        [Fact]
        public async Task AddAsync_WithMultipleMovies_ShouldAddAllMovies()
        {
            // Arrange
            var movies = new List<Movie>
            {
                new Movie { Title = "Movie 1", Director = "Director 1", Duration = 120 },
                new Movie { Title = "Movie 2", Director = "Director 2", Duration = 150 },
                new Movie { Title = "Movie 3", Director = "Director 3", Duration = 180 }
            };

            // Act
            foreach (var movie in movies)
            {
                await _repository.AddAsync(movie);
            }
            await _context.SaveChangesAsync();

            // Assert
            var allMovies = await _repository.GetAllAsync();
            Assert.Equal(3, allMovies.Count());
            Assert.Contains(allMovies, m => m.Title == "Movie 1");
            Assert.Contains(allMovies, m => m.Title == "Movie 2");
            Assert.Contains(allMovies, m => m.Title == "Movie 3");
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateMovie()
        {
            // Arrange
            var movie = new Movie { Title = "Original Movie", Director = "Original Director", Duration = 120 };
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();

            movie.Title = "Updated Movie";
            movie.Director = "Updated Director";

            // Act
            var result = await _repository.UpdateAsync(movie);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Movie", result.Title);
            Assert.Equal("Updated Director", result.Director);
            Assert.NotNull(result.UpdatedDate);

            var updatedMovie = await _context.Movies.FindAsync(movie.Id);
            Assert.NotNull(updatedMovie);
            Assert.Equal("Updated Movie", updatedMovie.Title);
            Assert.Equal("Updated Director", updatedMovie.Director);
        }

        [Fact]
        public async Task UpdateAsync_WithMultipleUpdates_ShouldUpdateCorrectly()
        {
            // Arrange
            var movie = new Movie { Title = "Original Movie", Director = "Original Director", Duration = 120 };
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();

            // First update
            movie.Title = "First Update";
            await _repository.UpdateAsync(movie);
            await _context.SaveChangesAsync();

            // Second update
            movie.Title = "Second Update";
            movie.Director = "Updated Director";
            var result = await _repository.UpdateAsync(movie);

            // Assert
            Assert.Equal("Second Update", result.Title);
            Assert.Equal("Updated Director", result.Director);
        }

        [Fact]
        public async Task DeleteAsync_ShouldSoftDeleteMovie()
        {
            // Arrange
            var movie = new Movie { Title = "Test Movie", Director = "Test Director", Duration = 120 };
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(movie.Id, "test", "test reason");
            await _context.SaveChangesAsync();

            // Assert
            var deletedMovie = await _context.Movies.FindAsync(movie.Id);
            Assert.NotNull(deletedMovie);
            Assert.True(deletedMovie.IsDeleted);
            Assert.Equal("test", deletedMovie.DeletedBy);
            Assert.Equal("test reason", deletedMovie.DeleteReason);
            Assert.NotNull(deletedMovie.DeletedDate);
        }

        [Fact]
        public async Task DeleteAsync_WithEmptyDeletedBy_ShouldSoftDeleteMovie()
        {
            // Arrange
            var movie = new Movie { Title = "Test Movie", Director = "Test Director", Duration = 120 };
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(movie.Id, "", "test reason");
            await _context.SaveChangesAsync();

            // Assert
            var deletedMovie = await _context.Movies.FindAsync(movie.Id);
            Assert.NotNull(deletedMovie);
            Assert.True(deletedMovie.IsDeleted);
            Assert.Equal("", deletedMovie.DeletedBy);
        }

        [Fact]
        public async Task DeleteAsync_WithEmptyReason_ShouldSoftDeleteMovie()
        {
            // Arrange
            var movie = new Movie { Title = "Test Movie", Director = "Test Director", Duration = 120 };
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(movie.Id, "test", "");
            await _context.SaveChangesAsync();

            // Assert
            var deletedMovie = await _context.Movies.FindAsync(movie.Id);
            Assert.NotNull(deletedMovie);
            Assert.True(deletedMovie.IsDeleted);
            Assert.Equal("", deletedMovie.DeleteReason);
        }

        [Fact]
        public async Task GetAllWithSessionsAsync_ShouldReturnMoviesWithSessions()
        {
            // Arrange
            var salon = new Salon { Name = "Test Salon", Capacity = 100 };
            await _context.Salons.AddAsync(salon);
            await _context.SaveChangesAsync();

            var movie = new Movie { Title = "Test Movie", Director = "Test Director", Duration = 120 };
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();

            var session = new Session { MovieId = movie.Id, SalonId = salon.Id, StartTime = TimeSpan.FromHours(14), SessionDate = DateTime.Today };
            await _context.Sessions.AddAsync(session);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllWithSessionsAsync();

            // Assert
            Assert.Single(result);
            var resultMovie = result.First();
            Assert.Equal("Test Movie", resultMovie.Title);
            Assert.Single(resultMovie.Sessions);
            Assert.Equal(salon.Id, resultMovie.Sessions.First().SalonId);
        }

        [Fact]
        public async Task GetAllWithSessionsAsync_ShouldNotReturnDeletedMovies()
        {
            // Arrange
            var salon = new Salon { Name = "Test Salon", Capacity = 100 };
            await _context.Salons.AddAsync(salon);
            await _context.SaveChangesAsync();

            var activeMovie = new Movie { Title = "Active Movie", Director = "Test Director", Duration = 120 };
            var deletedMovie = new Movie { Title = "Deleted Movie", Director = "Test Director", Duration = 120, IsDeleted = true };
            
            await _context.Movies.AddRangeAsync(activeMovie, deletedMovie);
            await _context.SaveChangesAsync();

            var activeSession = new Session { MovieId = activeMovie.Id, SalonId = salon.Id, StartTime = TimeSpan.FromHours(14), SessionDate = DateTime.Today };
            var deletedSession = new Session { MovieId = deletedMovie.Id, SalonId = salon.Id, StartTime = TimeSpan.FromHours(16), SessionDate = DateTime.Today, IsDeleted = true };
            
            await _context.Sessions.AddRangeAsync(activeSession, deletedSession);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllWithSessionsAsync();

            // Assert
            Assert.Single(result);
            var resultMovie = result.First();
            Assert.Equal("Active Movie", resultMovie.Title);
            Assert.Single(resultMovie.Sessions);
        }

        [Fact]
        public async Task GetByIdWithSessionsAsync_ShouldReturnMovieWithSessions()
        {
            // Arrange
            var salon = new Salon { Name = "Test Salon", Capacity = 100 };
            await _context.Salons.AddAsync(salon);
            await _context.SaveChangesAsync();

            var movie = new Movie { Title = "Test Movie", Director = "Test Director", Duration = 120 };
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();

            var session = new Session { MovieId = movie.Id, SalonId = salon.Id, StartTime = TimeSpan.FromHours(14), SessionDate = DateTime.Today };
            await _context.Sessions.AddAsync(session);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdWithSessionsAsync(movie.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Movie", result.Title);
            Assert.Single(result.Sessions);
            Assert.Equal(salon.Id, result.Sessions.First().SalonId);
        }

        [Fact]
        public async Task GetByIdWithSessionsAsync_WithMultipleSessions_ShouldReturnAllSessions()
        {
            // Arrange
            var salon1 = new Salon { Name = "Salon 1", Capacity = 100 };
            var salon2 = new Salon { Name = "Salon 2", Capacity = 150 };
            await _context.Salons.AddRangeAsync(salon1, salon2);
            await _context.SaveChangesAsync();

            var movie = new Movie { Title = "Test Movie", Director = "Test Director", Duration = 120 };
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();

            var sessions = new List<Session>
            {
                new Session { MovieId = movie.Id, SalonId = salon1.Id, StartTime = TimeSpan.FromHours(14), SessionDate = DateTime.Today },
                new Session { MovieId = movie.Id, SalonId = salon2.Id, StartTime = TimeSpan.FromHours(16), SessionDate = DateTime.Today }
            };
            await _context.Sessions.AddRangeAsync(sessions);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdWithSessionsAsync(movie.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Movie", result.Title);
            Assert.Equal(2, result.Sessions.Count);
        }

        [Fact]
        public async Task GetActiveMoviesAsync_ShouldReturnActiveMovies()
        {
            // Arrange
            var activeMovie = new Movie { Title = "Active Movie", Director = "Director", Duration = 120, IsActive = true };
            var inactiveMovie = new Movie { Title = "Inactive Movie", Director = "Director", Duration = 120, IsActive = false };
            
            await _context.Movies.AddRangeAsync(activeMovie, inactiveMovie);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetActiveMoviesAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal("Active Movie", result.First().Title);
        }

        [Fact]
        public async Task GetActiveMoviesAsync_ShouldReturnEmptyList_WhenNoActiveMovies()
        {
            // Arrange
            var inactiveMovie = new Movie { Title = "Inactive Movie", Director = "Director", Duration = 120, IsActive = false };
            await _context.Movies.AddAsync(inactiveMovie);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetActiveMoviesAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetActiveMoviesAsync_ShouldNotReturnDeletedMovies()
        {
            // Arrange
            var activeMovie = new Movie { Title = "Active Movie", Director = "Director", Duration = 120, IsActive = true };
            var deletedActiveMovie = new Movie { Title = "Deleted Active Movie", Director = "Director", Duration = 120, IsActive = true, IsDeleted = true };
            
            await _context.Movies.AddRangeAsync(activeMovie, deletedActiveMovie);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetActiveMoviesAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal("Active Movie", result.First().Title);
        }

        [Fact]
        public async Task ExistsAsync_WithValidId_ShouldReturnTrue()
        {
            // Arrange
            var movie = new Movie { Title = "Test Movie", Director = "Test Director", Duration = 120 };
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.ExistsAsync(movie.Id);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ExistsAsync_WithInvalidId_ShouldReturnFalse()
        {
            // Act
            var result = await _repository.ExistsAsync(999);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ExistsAsync_WithZeroId_ShouldReturnFalse()
        {
            // Act
            var result = await _repository.ExistsAsync(0);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ExistsAsync_WithNegativeId_ShouldReturnFalse()
        {
            // Act
            var result = await _repository.ExistsAsync(-1);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ExistsAsync_WithDeletedMovie_ShouldReturnFalse()
        {
            // Arrange
            var movie = new Movie { Title = "Deleted Movie", Director = "Test Director", Duration = 120, IsDeleted = true };
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.ExistsAsync(movie.Id);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(999, false)]
        [InlineData(0, false)]
        [InlineData(-1, false)]
        public async Task ExistsAsync_WithVariousIds_ShouldReturnExpectedResult(int id, bool expectedResult)
        {
            // Arrange
            if (expectedResult)
            {
                var movie = new Movie { Id = id, Title = "Test Movie", Director = "Test Director", Duration = 120 };
                await _context.Movies.AddAsync(movie);
                await _context.SaveChangesAsync();
            }

            // Act
            var result = await _repository.ExistsAsync(id);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("Action Movie", "Director 1", 120)]
        [InlineData("Comedy Movie", "Director 2", 150)]
        [InlineData("Drama Movie", "Director 3", 180)]
        public async Task AddAsync_WithDifferentMovies_ShouldAddCorrectly(string title, string director, int duration)
        {
            // Arrange
            var movie = new Movie { Title = title, Director = director, Duration = duration };

            // Act
            var result = await _repository.AddAsync(movie);
            await _context.SaveChangesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(0, result.Id);
            Assert.Equal(title, result.Title);
            Assert.Equal(director, result.Director);
            Assert.Equal(duration, result.Duration);

            var savedMovie = await _context.Movies.FindAsync(result.Id);
            Assert.NotNull(savedMovie);
            Assert.Equal(title, savedMovie.Title);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
} 