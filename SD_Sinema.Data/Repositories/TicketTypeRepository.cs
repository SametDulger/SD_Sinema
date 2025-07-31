using Microsoft.EntityFrameworkCore;
using SD_Sinema.Core.Entities;
using SD_Sinema.Core.Interfaces;
using SD_Sinema.Data.Context;

namespace SD_Sinema.Data.Repositories
{
    public class TicketTypeRepository : Repository<TicketType>, IRepository<TicketType>
    {
        public TicketTypeRepository(SinemaDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TicketType>> GetActiveTicketTypesAsync()
        {
            return await _context.TicketTypes
                .Where(t => t.IsActive)
                .ToListAsync();
        }

        public async Task<TicketType?> GetTicketTypeByNameAsync(string name)
        {
            return await _context.TicketTypes
                .FirstOrDefaultAsync(t => t.Name == name);
        }

        public async Task<IEnumerable<TicketType>> GetTicketTypesWithDiscountAsync()
        {
            return await _context.TicketTypes
                .Where(t => t.DiscountPercentage > 0)
                .ToListAsync();
        }
    }
} 