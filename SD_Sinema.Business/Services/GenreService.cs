using SD_Sinema.Core.Entities;
using SD_Sinema.Core.Interfaces;
using SD_Sinema.Data.Repositories;

namespace SD_Sinema.Business.Services
{
    public class GenreService : IGenreService
    {
        private readonly IRepository<Genre> _genreRepository;

        public GenreService(IRepository<Genre> genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<IEnumerable<Genre>> GetAllAsync()
        {
            return await _genreRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Genre>> GetActiveGenresAsync()
        {
            return await _genreRepository.GetAllAsync(g => g.IsActive);
        }

        public async Task<IEnumerable<string>> GetActiveGenreNamesAsync()
        {
            var genres = await _genreRepository.GetAllAsync(g => g.IsActive);
            return genres.Select(g => g.Name);
        }

        public async Task<Genre?> GetByIdAsync(int id)
        {
            return await _genreRepository.GetByIdAsync(id);
        }

        public async Task<Genre> CreateAsync(Genre genre)
        {
            return await _genreRepository.AddAsync(genre);
        }

        public async Task<Genre> UpdateAsync(int id, Genre genre)
        {
            var existingGenre = await _genreRepository.GetByIdAsync(id);
            if (existingGenre == null)
                throw new InvalidOperationException("Genre bulunamadı.");

            existingGenre.Name = genre.Name;
            existingGenre.Description = genre.Description;
            existingGenre.IsActive = genre.IsActive;

            await _genreRepository.UpdateAsync(existingGenre);
            return existingGenre;
        }

        public async Task DeleteAsync(int id)
        {
            await _genreRepository.DeleteAsync(id, "System", "Manual deletion");
        }
    }
} 