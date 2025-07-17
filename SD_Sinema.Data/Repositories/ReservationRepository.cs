using Microsoft.EntityFrameworkCore;
using SD_Sinema.Core.Entities;
using SD_Sinema.Core.Interfaces;
using SD_Sinema.Data.Context;

namespace SD_Sinema.Data.Repositories
{
    public class ReservationRepository : Repository<Reservation>, IReservationRepository
    {
        public ReservationRepository(SinemaDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Reservation>> GetAllWithDetailsAsync()
        {
            return await _dbSet
                .Where(r => !r.IsDeleted)
                .Include(r => r.User)
                .Include(r => r.Session)
                .ThenInclude(s => s.Movie)
                .Include(r => r.Session)
                .ThenInclude(s => s.Salon)
                .Include(r => r.Seat)
                .ToListAsync();
        }

        public async Task<Reservation?> GetByIdWithDetailsAsync(int id)
        {
            return await _dbSet
                .Where(r => r.Id == id && !r.IsDeleted)
                .Include(r => r.User)
                .Include(r => r.Session)
                .ThenInclude(s => s.Movie)
                .Include(r => r.Session)
                .ThenInclude(s => s.Salon)
                .Include(r => r.Seat)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByUserAsync(int userId)
        {
            return await _dbSet
                .Where(r => r.UserId == userId && !r.IsDeleted)
                .Include(r => r.Session)
                .ThenInclude(s => s.Movie)
                .Include(r => r.Session)
                .ThenInclude(s => s.Salon)
                .Include(r => r.Seat)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetReservationsBySessionAsync(int sessionId)
        {
            return await _dbSet
                .Where(r => r.SessionId == sessionId && !r.IsDeleted)
                .Include(r => r.User)
                .Include(r => r.Seat)
                .ToListAsync();
        }
    }
} 