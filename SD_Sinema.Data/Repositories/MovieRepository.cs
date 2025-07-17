using Microsoft.EntityFrameworkCore;
using SD_Sinema.Core.Entities;
using SD_Sinema.Core.Interfaces;
using SD_Sinema.Data.Context;

namespace SD_Sinema.Data.Repositories
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        public MovieRepository(SinemaDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Movie>> GetAllWithSessionsAsync()
        {
            return await _dbSet
                .Where(m => !m.IsDeleted)
                .Include(m => m.Sessions.Where(s => !s.IsDeleted))
                .ThenInclude(s => s.Salon)
                .ToListAsync();
        }

        public async Task<Movie?> GetByIdWithSessionsAsync(int id)
        {
            return await _dbSet
                .Where(m => m.Id == id && !m.IsDeleted)
                .Include(m => m.Sessions.Where(s => !s.IsDeleted))
                .ThenInclude(s => s.Salon)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Movie>> GetActiveMoviesAsync()
        {
            return await _dbSet
                .Where(m => !m.IsDeleted && m.IsActive && (m.EndDate == null || m.EndDate > DateTime.Now))
                .Include(m => m.Sessions.Where(s => !s.IsDeleted))
                .ToListAsync();
        }
    }
} 