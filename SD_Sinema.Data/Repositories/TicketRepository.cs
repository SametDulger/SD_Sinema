using Microsoft.EntityFrameworkCore;
using SD_Sinema.Core.Entities;
using SD_Sinema.Core.Interfaces;
using SD_Sinema.Data.Context;

namespace SD_Sinema.Data.Repositories
{
    public class TicketRepository : Repository<Ticket>, IRepository<Ticket>
    {
        public TicketRepository(SinemaDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Ticket>> GetTicketsByUserAsync(int userId)
        {
            return await _context.Tickets
                .Include(t => t.Session)
                .Include(t => t.Seat)
                .Include(t => t.TicketType)
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Ticket>> GetTicketsBySessionAsync(int sessionId)
        {
            return await _context.Tickets
                .Include(t => t.User)
                .Include(t => t.Seat)
                .Include(t => t.TicketType)
                .Where(t => t.SessionId == sessionId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Ticket>> GetActiveTicketsAsync()
        {
            return await _context.Tickets
                .Include(t => t.Session)
                .Include(t => t.User)
                .Where(t => t.Status == "Active")
                .ToListAsync();
        }
    }
} 