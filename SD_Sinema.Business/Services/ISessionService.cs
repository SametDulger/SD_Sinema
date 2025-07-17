using SD_Sinema.Business.DTOs;

namespace SD_Sinema.Business.Services
{
    public interface ISessionService
    {
        Task<IEnumerable<SessionDto>> GetAllAsync();
        Task<SessionDto?> GetByIdAsync(int id);
        Task<SessionDto> CreateAsync(CreateSessionDto createSessionDto);
        Task<SessionDto> UpdateAsync(int id, UpdateSessionDto updateSessionDto);
        Task DeleteAsync(int id, string deletedBy, string reason);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<SessionDto>> GetSessionsByMovieAsync(int movieId);
        Task<IEnumerable<SessionDto>> GetSessionsBySalonAsync(int salonId);
        Task<IEnumerable<SessionDto>> GetActiveSessionsAsync();
    }
} 