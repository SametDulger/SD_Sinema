using Microsoft.EntityFrameworkCore;
using SD_Sinema.Core.Entities;
using SD_Sinema.Core.Interfaces;
using SD_Sinema.Data.Context;

namespace SD_Sinema.Data.Repositories
{
    public class UserRepository : Repository<User>, IRepository<User>
    {
        public UserRepository(SinemaDbContext context) : base(context)
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> GetActiveUsersAsync()
        {
            return await _context.Users
                .Where(u => u.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetVipUsersAsync()
        {
            return await _context.Users
                .Where(u => u.IsVipMember && u.VipEndDate > DateTime.Now)
                .ToListAsync();
        }
    }
} 