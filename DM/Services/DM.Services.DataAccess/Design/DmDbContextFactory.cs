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
            builder.UseNpgsql("User ID=jbwoppwtyuobiw;Password=73d1be40148580d44f4ed1c0d87fa4108ebc64981101495280dbad92c2e83a24;Host=ec2-54-75-245-196.eu-west-1.compute.amazonaws.com;Port=5432;Database=df9o6q827rd30n;sslmode=Prefer;Trust Server Certificate=true");
            return new DmDbContext(builder.Options);
        }
    }
}