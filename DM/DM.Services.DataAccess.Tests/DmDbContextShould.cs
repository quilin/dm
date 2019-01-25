using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DM.Services.DataAccess.Tests
{
    public class DmDbContextShould
    {
        private readonly DmDbContext dbContext;

        public DmDbContextShould()
        {
            var builder = new DbContextOptionsBuilder<DmDbContext>();
            builder.UseNpgsql("User ID=postgres;Password=admin;Host=localhost;Port=5432;Database=dm3;Pooling=true;MinPoolSize=0;MaxPoolSize=100;Connection Idle Lifetime=60;");
            dbContext = new DmDbContext(builder.Options);
        }
        
        [Fact]
        public async Task MapForumsCorrectly()
        {
            var forums = await dbContext.Fora.AsNoTracking().ToArrayAsync();
            forums.Should().NotBeEmpty();
            forums.Should().NotContainNulls();
        }

        [Fact]
        public async Task MapForumModeratorsCorrectly()
        {
            var moderators = await dbContext.Fora.AsNoTracking()
                .Include(f => f.Moderators)
                .SelectMany(f => f.Moderators)
                .ToArrayAsync();
            moderators.Should().NotBeEmpty();
            moderators.Should().NotContainNulls();
        }

        [Fact]
        public async Task MapLikesForTopics()
        {
            var forumTopic = await dbContext.ForumTopics.AsNoTracking()
                .Include(t => t.Likes)
                .Where(t => t.ForumTopicId == Guid.Parse("00c3349d-926d-47b8-91ed-8fe19057a443"))
                .FirstAsync();
            forumTopic.Likes.Should().NotBeEmpty();
        }

        [Fact]
        public async Task MapLikesForComment()
        {
            var comment = await dbContext.Comments.AsNoTracking()
                .Include(c => c.Likes)
                .Where(c => c.CommentId == Guid.Parse("090b4485-62ce-4ee2-9973-a0b0d33b9ac5"))
                .FirstAsync();
            comment.Likes.Should().NotBeEmpty();
        }
    }
}