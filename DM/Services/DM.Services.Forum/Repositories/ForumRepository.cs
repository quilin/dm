using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Common.Repositories;
using DM.Services.Core.Exceptions;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.Forum.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace DM.Services.Forum.Repositories
{
    public class ForumRepository : IForumRepository
    {
        private readonly ReadDmDbContext dmDbContext;
        private readonly IMemoryCache memoryCache;
        private readonly IUnreadCountersRepository unreadCountersRepository;

        public ForumRepository(
            ReadDmDbContext dmDbContext,
            IMemoryCache memoryCache,
            IUnreadCountersRepository unreadCountersRepository)
        {
            this.dmDbContext = dmDbContext;
            this.memoryCache = memoryCache;
            this.unreadCountersRepository = unreadCountersRepository;
        }

        public async Task<IEnumerable<ForaListItem>> SelectFora(Guid userId, ForumAccessPolicy accessPolicy)
        {
            var fora = await memoryCache.GetOrCreateAsync($"ForaList_{accessPolicy}", async _ =>
                await dmDbContext.Fora
                    .Where(f => (f.ViewPolicy & accessPolicy) != ForumAccessPolicy.NoOne)
                    .OrderBy(f => f.Order)
                    .Select(f => new {f.ForumId, f.Title})
                    .ToArrayAsync());
            var counters = await unreadCountersRepository.SelectByParents(
                userId, UnreadEntryType.Message, fora.Select(f => f.ForumId).ToArray());
            return fora.Select(f => new ForaListItem
            {
                Title = f.Title,
                UnreadTopicsCount = counters[f.ForumId]
            });
        }

        public async Task<ForaListItem> GetForum(string forumTitle, Guid userId, ForumAccessPolicy accessPolicy)
        {
            var fora = await memoryCache.GetOrCreateAsync($"ForaList_{accessPolicy}", async _ =>
                await dmDbContext.Fora
                    .Where(f => (f.ViewPolicy & accessPolicy) != ForumAccessPolicy.NoOne)
                    .OrderBy(f => f.Order)
                    .Select(f => new {f.ForumId, f.Title})
                    .ToArrayAsync());
            var forum = fora.FirstOrDefault(f => f.Title == forumTitle);
            if (forum == null) throw new HttpException(HttpStatusCode.NotFound);

            var counters = await unreadCountersRepository.SelectByParents(
                userId, UnreadEntryType.Message, forum.ForumId);
            return new ForaListItem
            {
                Title = forum.Title,
                UnreadTopicsCount = counters[forum.ForumId]
            };
        }

        public Task<ForumTitle> FindForum(string forumTitle, ForumAccessPolicy accessPolicy)
        {
            return dmDbContext.Fora
                .Where(f => (f.ViewPolicy & accessPolicy) != ForumAccessPolicy.NoOne)
                .Where(f => f.Title == forumTitle)
                .Select(f => new ForumTitle
                {
                    ForumId = f.ForumId,
                    Title = f.Title
                })
                .FirstOrDefaultAsync();
        }
    }
}