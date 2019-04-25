using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DM.Services.DataAccess.Design
{
    /// <inheritdoc />
    public class DmDbContextFactory : IDesignTimeDbContextFactory<DmDbContext>
    {
        /// <inheritdoc />
        public DmDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<DmDbContext>();
            builder.UseNpgsql("User ID=postgres;Password=admin;Host=localhost;Port=5432;Database=dm3.5;Pooling=true;MinPoolSize=0;MaxPoolSize=100;Connection Idle Lifetime=60;");
            return new DmDbContext(builder.Options);
        }
    }
}