using Microsoft.EntityFrameworkCore;

namespace Migration.BusinessObjects
{
    public class LegacyDbContext : DbContext
    {
        public LegacyDbContext(DbContextOptions options) : base(options) 
        {
        }
        
        public DbSet<User> Users { get; set; }
    }
}