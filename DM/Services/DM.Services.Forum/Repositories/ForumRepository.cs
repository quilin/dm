using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess;
using DM.Services.Forum.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using DbForum = DM.Services.DataAccess.BusinessObjects.Fora.Forum;

namespace DM.Services.Forum.Repositories
{
    internal class ForumRepository : IForumRepository
    {
        private readonly ReadDmDbContext dmDbContext;
        private readonly IMemoryCache memoryCache;

        public ForumRepository(
            ReadDmDbContext dmDbContext,
            IMemoryCache memoryCache)
        {
            this.dmDbContext = dmDbContext;
            this.memoryCache = memoryCache;
        }

        public async Task<IEnumerable<ForaListItem>> SelectFora(ForumAccessPolicy accessPolicy)
        {
            return await memoryCache.GetOrCreateAsync($"ForaList_{accessPolicy}", async _ =>
                await dmDbContext.Fora
                    .Where(f => (f.ViewPolicy & accessPolicy) != ForumAccessPolicy.NoOne)
                    .OrderBy(f => f.Order)
                    .Select(f => new ForaListItem
                    {
                        Id = f.ForumId,
                        Title = f.Title
                    })
                    .ToArrayAsync());
        }
    }
}