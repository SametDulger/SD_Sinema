using SD_Sinema.Business.DTOs;
using SD_Sinema.Core.Entities;
using SD_Sinema.Core.Interfaces;

namespace SD_Sinema.Business.Services
{
    public class MovieService : IMovieService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MovieService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<MovieDto>> GetAllAsync()
        {
            var movies = await _unitOfWork.Movies.GetAllWithSessionsAsync();
            return movies.Select(MapToDto);
        }

        public async Task<MovieDto?> GetByIdAsync(int id)
        {
            var movie = await _unitOfWork.Movies.GetByIdWithSessionsAsync(id);
            return movie != null ? MapToDto(movie) : null;
        }

        public async Task<MovieDto> CreateAsync(CreateMovieDto createMovieDto)
        {
            var movie = new Movie
            {
                Title = createMovieDto.Title,
                Description = createMovieDto.Description,
                Duration = createMovieDto.Duration,
                Director = createMovieDto.Director,
                Cast = createMovieDto.Cast,
                Genre = createMovieDto.Genre,
                AgeRating = createMovieDto.AgeRating,
                PosterUrl = createMovieDto.PosterUrl,
                TrailerUrl = createMovieDto.TrailerUrl,
                ReleaseDate = createMovieDto.ReleaseDate,
                EndDate = createMovieDto.EndDate
            };

            await _unitOfWork.Movies.AddAsync(movie);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(movie);
        }

        public async Task<MovieDto> UpdateAsync(int id, UpdateMovieDto updateMovieDto)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(id);
            if (movie == null)
                throw new InvalidOperationException("Film bulunamadı.");

            movie.Title = updateMovieDto.Title;
            movie.Description = updateMovieDto.Description;
            movie.Duration = updateMovieDto.Duration;
            movie.Director = updateMovieDto.Director;
            movie.Cast = updateMovieDto.Cast;
            movie.Genre = updateMovieDto.Genre;
            movie.AgeRating = updateMovieDto.AgeRating;
            movie.PosterUrl = updateMovieDto.PosterUrl;
            movie.TrailerUrl = updateMovieDto.TrailerUrl;
            movie.ReleaseDate = updateMovieDto.ReleaseDate;
            movie.EndDate = updateMovieDto.EndDate;
            movie.IsActive = updateMovieDto.IsActive;

            await _unitOfWork.Movies.UpdateAsync(movie);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(movie);
        }

        public async Task DeleteAsync(int id, string deletedBy, string reason)
        {
            await _unitOfWork.Movies.DeleteAsync(id, deletedBy, reason);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Movies.ExistsAsync(id);
        }

        public async Task<IEnumerable<MovieDto>> GetActiveMoviesAsync()
        {
            var movies = await _unitOfWork.Movies.GetActiveMoviesAsync();
            return movies.Select(MapToDto);
        }

        private static MovieDto MapToDto(Movie movie)
        {
            return new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                Duration = movie.Duration,
                Director = movie.Director,
                Cast = movie.Cast,
                Genre = movie.Genre,
                AgeRating = movie.AgeRating,
                PosterUrl = movie.PosterUrl,
                TrailerUrl = movie.TrailerUrl,
                ReleaseDate = movie.ReleaseDate,
                EndDate = movie.EndDate,
                IsActive = movie.IsActive,
                CreatedDate = movie.CreatedDate
            };
        }
    }
} 