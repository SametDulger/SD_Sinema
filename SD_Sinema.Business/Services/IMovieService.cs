using SD_Sinema.Business.DTOs;

namespace SD_Sinema.Business.Services
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieDto>> GetAllAsync();
        Task<MovieDto?> GetByIdAsync(int id);
        Task<MovieDto> CreateAsync(CreateMovieDto createMovieDto);
        Task<MovieDto> UpdateAsync(int id, UpdateMovieDto updateMovieDto);
        Task DeleteAsync(int id, string deletedBy, string reason);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<MovieDto>> GetActiveMoviesAsync();
    }
} 