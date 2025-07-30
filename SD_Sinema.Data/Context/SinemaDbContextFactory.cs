using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SD_Sinema.Data.Context
{
    public class SinemaDbContextFactory : IDesignTimeDbContextFactory<SinemaDbContext>
    {
        public SinemaDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SinemaDbContext>();
            optionsBuilder.UseSqlServer("Server=MSI\\SQLEXPRESS;Database=SD_Sinema;Trusted_Connection=true;TrustServerCertificate=true;MultipleActiveResultSets=true");

            return new SinemaDbContext(optionsBuilder.Options);
        }
    }
} 