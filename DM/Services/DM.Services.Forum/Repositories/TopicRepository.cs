using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess;
using DM.Services.Forum.Dto;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Forum.Repositories
{
    internal class TopicRepository : ITopicRepository
    {
        private readonly ReadDmDbContext dmDbContext;
        private readonly IMapper mapper;

        public TopicRepository(
            ReadDmDbContext dmDbContext,
            IMapper mapper)
        {
            this.dmDbContext = dmDbContext;
            this.mapper = mapper;
        }

        private static readonly Guid NewsForumId = Guid.Parse("00000000-0000-0000-0000-000000000008");
        private static readonly Guid ErrorsForumId = Guid.Parse("00000000-0000-0000-0000-000000000006");

        public Task<int> Count(Guid forumId) =>
            dmDbContext.ForumTopics.CountAsync(t => !t.IsRemoved && t.ForumId == forumId && !t.Attached);

        public async Task<IEnumerable<Topic>> Get(Guid forumId, PagingData pagingData, bool attached)
        {
            var query = dmDbContext.ForumTopics
                .Where(t => !t.IsRemoved && t.ForumId == forumId && t.Attached == attached)
                .ProjectTo<Topic>(mapper.ConfigurationProvider);

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

        public async Task<Topic> Get(Guid topicId, ForumAccessPolicy accessPolicy)
        {
            return await dmDbContext.ForumTopics
                .Where(t => !t.IsRemoved && t.ForumTopicId == topicId &&
                    (t.Forum.ViewPolicy & accessPolicy) != ForumAccessPolicy.NoOne)
                .ProjectTo<Topic>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }
    }
}