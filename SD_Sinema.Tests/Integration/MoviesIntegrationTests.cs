using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SD_Sinema.API;
using SD_Sinema.Data.Context;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace SD_Sinema.Tests.Integration
{
    public class MoviesIntegrationTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly SinemaDbContext _context;

        public MoviesIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var sp = services.BuildServiceProvider();

                    using (var scope = sp.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var db = scopedServices.GetRequiredService<SinemaDbContext>();
                        db.Database.EnsureCreated();
                    }
                });
            });

            var scope = _factory.Services.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<SinemaDbContext>();
        }

        [Fact]
        public async Task GetMovies_ShouldReturnEmptyList_WhenNoMoviesExist()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/movies");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var movies = JsonSerializer.Deserialize<List<object>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            Assert.Empty(movies);
        }

        [Fact]
        public async Task PostMovie_ShouldCreateMovie_AndReturnCreatedResponse()
        {
            // Arrange
            var client = _factory.CreateClient();
            var movie = new
            {
                Title = "Test Movie",
                Director = "Test Director",
                Duration = 120,
                Description = "Test Description",
                ReleaseDate = DateTime.Now,
                Genre = "Action"
            };

            var json = JsonSerializer.Serialize(movie);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/movies", content);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();
            var createdMovie = JsonSerializer.Deserialize<JsonElement>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            Assert.Equal("Test Movie", createdMovie.GetProperty("title").GetString());
        }

        [Fact]
        public async Task GetMovieById_ShouldReturnMovie_WhenMovieExists()
        {
            // Arrange
            var client = _factory.CreateClient();
            var movie = new
            {
                Title = "Test Movie",
                Director = "Test Director",
                Duration = 120,
                Description = "Test Description",
                ReleaseDate = DateTime.Now,
                Genre = "Action"
            };

            var json = JsonSerializer.Serialize(movie);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var postResponse = await client.PostAsync("/api/movies", content);
            var postContent = await postResponse.Content.ReadAsStringAsync();
            var createdMovie = JsonSerializer.Deserialize<JsonElement>(postContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            var movieId = createdMovie.GetProperty("id").GetInt32();

            // Act
            var getResponse = await client.GetAsync($"/api/movies/{movieId}");

            // Assert
            getResponse.EnsureSuccessStatusCode();
            var getContent = await getResponse.Content.ReadAsStringAsync();
            var retrievedMovie = JsonSerializer.Deserialize<JsonElement>(getContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            Assert.Equal("Test Movie", retrievedMovie.GetProperty("title").GetString());
            Assert.Equal(movieId, retrievedMovie.GetProperty("id").GetInt32());
        }

        [Fact]
        public async Task GetMovieById_ShouldReturnNotFound_WhenMovieDoesNotExist()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/movies/999");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PutMovie_ShouldUpdateMovie_WhenMovieExists()
        {
            // Arrange
            var client = _factory.CreateClient();
            var movie = new
            {
                Title = "Original Movie",
                Director = "Original Director",
                Duration = 120,
                Description = "Original Description",
                ReleaseDate = DateTime.Now,
                Genre = "Action"
            };

            var json = JsonSerializer.Serialize(movie);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var postResponse = await client.PostAsync("/api/movies", content);
            var postContent = await postResponse.Content.ReadAsStringAsync();
            var createdMovie = JsonSerializer.Deserialize<JsonElement>(postContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            var movieId = createdMovie.GetProperty("id").GetInt32();

            var updatedMovie = new
            {
                Id = movieId,
                Title = "Updated Movie",
                Director = "Updated Director",
                Duration = 150,
                Description = "Updated Description",
                ReleaseDate = DateTime.Now,
                Genre = "Comedy"
            };

            var updateJson = JsonSerializer.Serialize(updatedMovie);
            var updateContent = new StringContent(updateJson, Encoding.UTF8, "application/json");

            // Act
            var putResponse = await client.PutAsync($"/api/movies/{movieId}", updateContent);

            // Assert
            Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);

            // Verify the update
            var getResponse = await client.GetAsync($"/api/movies/{movieId}");
            getResponse.EnsureSuccessStatusCode();
            var getContent = await getResponse.Content.ReadAsStringAsync();
            var retrievedMovie = JsonSerializer.Deserialize<JsonElement>(getContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            Assert.Equal("Updated Movie", retrievedMovie.GetProperty("title").GetString());
            Assert.Equal("Updated Director", retrievedMovie.GetProperty("director").GetString());
        }

        [Fact]
        public async Task DeleteMovie_ShouldDeleteMovie_WhenMovieExists()
        {
            // Arrange
            var client = _factory.CreateClient();
            var movie = new
            {
                Title = "Movie to Delete",
                Director = "Director",
                Duration = 120,
                Description = "Description",
                ReleaseDate = DateTime.Now,
                Genre = "Action"
            };

            var json = JsonSerializer.Serialize(movie);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var postResponse = await client.PostAsync("/api/movies", content);
            var postContent = await postResponse.Content.ReadAsStringAsync();
            var createdMovie = JsonSerializer.Deserialize<JsonElement>(postContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            var movieId = createdMovie.GetProperty("id").GetInt32();

            // Act
            var deleteResponse = await client.DeleteAsync($"/api/movies/{movieId}?deletedBy=test&reason=test");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

            // Verify the deletion
            var getResponse = await client.GetAsync($"/api/movies/{movieId}");
            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }

        [Fact]
        public async Task DeleteMovie_ShouldReturnNotFound_WhenMovieDoesNotExist()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.DeleteAsync("/api/movies/999?deletedBy=test&reason=test");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
} 