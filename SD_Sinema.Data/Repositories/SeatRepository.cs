using Microsoft.EntityFrameworkCore;
using SD_Sinema.Core.Entities;
using SD_Sinema.Core.Interfaces;
using SD_Sinema.Data.Context;

namespace SD_Sinema.Data.Repositories
{
    public class SeatRepository : Repository<Seat>, IRepository<Seat>
    {
        public SeatRepository(SinemaDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Seat>> GetSeatsBySalonAsync(int salonId)
        {
            return await _context.Seats
                .Where(s => s.SalonId == salonId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Seat>> GetAvailableSeatsAsync(int salonId)
        {
            return await _context.Seats
                .Where(s => s.SalonId == salonId && s.IsAvailable)
                .ToListAsync();
        }

        public async Task<Seat?> GetSeatWithReservationsAsync(int id)
        {
            return await _context.Seats
                .Include(s => s.Reservations)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }
} 