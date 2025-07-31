using Microsoft.EntityFrameworkCore;
using SD_Sinema.Core.Entities;
using SD_Sinema.Core.Interfaces;
using SD_Sinema.Data.Context;

namespace SD_Sinema.Data.Repositories
{
    public class SalonRepository : Repository<Salon>, IRepository<Salon>
    {
        public SalonRepository(SinemaDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Salon>> GetActiveSalonsAsync()
        {
            return await _context.Salons
                .Where(s => s.IsActive)
                .ToListAsync();
        }

        public async Task<Salon?> GetSalonWithSeatsAsync(int id)
        {
            return await _context.Salons
                .Include(s => s.Seats)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Salon>> GetSalonsWithSessionsAsync()
        {
            return await _context.Salons
                .Include(s => s.Sessions)
                .Where(s => s.IsActive)
                .ToListAsync();
        }
    }
} 