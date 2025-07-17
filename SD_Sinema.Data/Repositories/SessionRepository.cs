using Microsoft.EntityFrameworkCore;
using SD_Sinema.Core.Entities;
using SD_Sinema.Core.Interfaces;
using SD_Sinema.Data.Context;

namespace SD_Sinema.Data.Repositories
{
    public class SessionRepository : Repository<Session>, ISessionRepository
    {
        public SessionRepository(SinemaDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Session>> GetAllWithDetailsAsync()
        {
            return await _dbSet
                .Where(s => !s.IsDeleted)
                .Include(s => s.Movie)
                .Include(s => s.Salon)
                .Include(s => s.Reservations.Where(r => !r.IsDeleted))
                .ThenInclude(r => r.Seat)
                .ToListAsync();
        }

        public async Task<Session?> GetByIdWithDetailsAsync(int id)
        {
            return await _dbSet
                .Where(s => s.Id == id && !s.IsDeleted)
                .Include(s => s.Movie)
                .Include(s => s.Salon)
                .Include(s => s.Reservations.Where(r => !r.IsDeleted))
                .ThenInclude(r => r.Seat)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Session>> GetSessionsByMovieAsync(int movieId)
        {
            return await _dbSet
                .Where(s => s.MovieId == movieId && !s.IsDeleted)
                .Include(s => s.Salon)
                .Include(s => s.Reservations.Where(r => !r.IsDeleted))
                .ToListAsync();
        }

        public async Task<IEnumerable<Session>> GetSessionsBySalonAsync(int salonId)
        {
            return await _dbSet
                .Where(s => s.SalonId == salonId && !s.IsDeleted)
                .Include(s => s.Movie)
                .Include(s => s.Reservations.Where(r => !r.IsDeleted))
                .ToListAsync();
        }
    }
} 