using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Extensions;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Forum.Dto.Output;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Forum.BusinessProcesses.Topics
{
    /// <inheritdoc />
    internal class TopicRepository : ITopicRepository
    {
        private readonly DmDbContext dbContext;
        private readonly IMapper mapper;

        public TopicRepository(
            DmDbContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        private static readonly Guid NewsForumId = Guid.Parse("00000000-0000-0000-0000-000000000008");
        private static readonly Guid ErrorsForumId = Guid.Parse("00000000-0000-0000-0000-000000000006");

        /// <inheritdoc />
        public Task<int> Count(Guid forumId) =>
            dbContext.ForumTopics.CountAsync(t => !t.IsRemoved && t.ForumId == forumId && !t.Attached);

        /// <inheritdoc />
        public async Task<IEnumerable<Topic>> Get(Guid forumId, PagingData pagingData, bool attached)
        {
            var query = dbContext.ForumTopics
                .Where(t => !t.IsRemoved && t.ForumId == forumId && t.Attached == attached)
                .ProjectTo<Topic>(mapper.ConfigurationProvider);

            IOrderedQueryable<Topic> orderedQuery;
            if (forumId == NewsForumId || attached)
            {
                orderedQuery = query.OrderByDescending(q => q.CreateDate);
            }
            else if (forumId == ErrorsForumId)
            {
                orderedQuery = query.OrderBy(q => q.Closed).ThenByDescending(q => q.LastActivityDate);
            }
            else
            {
                orderedQuery = query.OrderByDescending(q => q.LastActivityDate);
            }

            return await orderedQuery.Page(pagingData).ToArrayAsync();
        }

        /// <inheritdoc />
        public async Task<Topic> Get(Guid topicId, ForumAccessPolicy accessPolicy)
        {
            return await dbContext.ForumTopics
                .Where(t => !t.IsRemoved && t.ForumTopicId == topicId &&
                            (t.Forum.ViewPolicy & accessPolicy) != ForumAccessPolicy.NoOne)
                .ProjectTo<Topic>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        /// <inheritdoc />
        public async Task<Topic> Create(ForumTopic forumTopic)
        {
            dbContext.ForumTopics.Add(forumTopic);
            await dbContext.SaveChangesAsync();
            return await dbContext.ForumTopics
                .Where(t => t.ForumTopicId == forumTopic.ForumTopicId)
                .ProjectTo<Topic>(mapper.ConfigurationProvider)
                .FirstAsync();
        }

        /// <inheritdoc />
        public async Task<Topic> Update(UpdateBuilder<ForumTopic> updateBuilder)
        {
            var topicId = updateBuilder.Update(dbContext);
            await dbContext.SaveChangesAsync();
            return await dbContext.ForumTopics
                .Where(t => t.ForumTopicId == topicId)
                .ProjectTo<Topic>(mapper.ConfigurationProvider)
                .FirstAsync();
        }
    }
}