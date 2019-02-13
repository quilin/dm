using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Common.Repositories;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Exceptions;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.Forum.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using DbForum = DM.Services.DataAccess.BusinessObjects.Fora.Forum;

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
            var fora = await SelectFora(accessPolicy);
            var counters = await unreadCountersRepository.SelectByParents(
                userId, UnreadEntryType.Message, fora.Select(f => f.Id).ToArray());

            foreach (var forum in fora)
            {
                forum.UnreadTopicsCount = counters[forum.Id];
            }

            return fora;
        }

        public async Task<ForaListItem> GetForum(string forumTitle, ForumAccessPolicy accessPolicy, Guid? userId = null)
        {
            var fora = await SelectFora(accessPolicy);
            var forum = fora.FirstOrDefault(f => f.Title == forumTitle);
            if (forum == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, $"Forum \"{forumTitle}\" not found");
            }

            if (userId.HasValue)
            {
                forum.UnreadTopicsCount = (await unreadCountersRepository.SelectByParents(
                    userId.Value, UnreadEntryType.Message, forum.Id))[forum.Id];
            }

            return forum;
        }

        private async Task<ICollection<ForaListItem>> SelectFora(ForumAccessPolicy accessPolicy)
        {
            return await memoryCache.GetOrCreateAsync($"ForaList_{accessPolicy}", async _ =>
                await dmDbContext.Fora
                    .Include(f => f.Moderators)
                    .ThenInclude(m => m.User)
                    .ThenInclude(u => u.ProfilePictures)
                    .Where(f => (f.ViewPolicy & accessPolicy) != ForumAccessPolicy.NoOne)
                    .OrderBy(f => f.Order)
                    .Select(f => new ForaListItem
                    {
                        Id = f.ForumId,
                        Title = f.Title,
                        Moderators = f.Moderators
                            .Where(u => !u.User.IsRemoved)
                            .Select(u => Users.GeneralProjection.Invoke(u.User))
                    })
                    .ToArrayAsync());
        }
    }
}