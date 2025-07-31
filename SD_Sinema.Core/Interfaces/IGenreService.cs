using SD_Sinema.Core.Entities;

namespace SD_Sinema.Core.Interfaces
{
    public interface IGenreService
    {
        Task<IEnumerable<Genre>> GetAllAsync();
        Task<IEnumerable<Genre>> GetActiveGenresAsync();
        Task<IEnumerable<string>> GetActiveGenreNamesAsync();
        Task<Genre?> GetByIdAsync(int id);
        Task<Genre> CreateAsync(Genre genre);
        Task<Genre> UpdateAsync(int id, Genre genre);
        Task DeleteAsync(int id);
    }
} 