using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using DM.Services.Core.Dto;
using DM.Services.DataAccess;
using DM.Services.Forum.Dto;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Forum.Repositories
{
    internal class TopicRepository : ITopicRepository
    {
        private readonly ReadDmDbContext dmDbContext;

        public TopicRepository(
            ReadDmDbContext dmDbContext)
        {
            this.dmDbContext = dmDbContext;
        }

        private static readonly Guid NewsForumId = Guid.Parse("00000000-0000-0000-0000-000000000008");
        private static readonly Guid ErrorsForumId = Guid.Parse("00000000-0000-0000-0000-000000000006");

        public Task<int> Count(Guid forumId) =>
            dmDbContext.ForumTopics.CountAsync(t => !t.IsRemoved && t.ForumId == forumId);

        public async Task<IEnumerable<TopicsListItem>> Get(Guid forumId, PagingData pagingData, bool attached)
        {
            var query = dmDbContext.ForumTopics
                .Include(t => t.Author)
                .Include(t => t.LastComment)
                .ThenInclude(c => c.Author)
                .Where(t => !t.IsRemoved && t.ForumId == forumId && t.Attached == attached)
                .ProjectTo<TopicsListItem>();

            if (forumId == NewsForumId || attached)
            {
                query = query.OrderByDescending(q => q.CreateDate);
            }
            else
            {
                if (forumId == ErrorsForumId)
                {
                    query = query.OrderBy(q => q.Closed);
                }

                query = query.OrderByDescending(q => q.LastActivityDate);
            }

            if (pagingData != null)
            {
                query = query.Skip((pagingData.CurrentPage - 1) * pagingData.PageSize).Take(pagingData.PageSize);
            }

            return await query.ToArrayAsync();
        }
    }
}