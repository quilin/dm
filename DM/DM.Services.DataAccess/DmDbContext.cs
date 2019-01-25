using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.DataAccess.BusinessObjects.Users;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.DataAccess
{
    public class DmDbContext : DbContext
    {
        public DmDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Forum> Fora { get; set; }
        public DbSet<ForumTopic> ForumTopics { get; set; }
        public DbSet<ForumModerator> ForumModerators { get; set; }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
    }
}