using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Common.Repositories;
using DM.Services.Core.Dto;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.Forum.Dto;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Forum.Repositories
{
    public class TopicRepository : ITopicRepository
    {
        private readonly ReadDmDbContext dmDbContext;
        private readonly IUnreadCountersRepository unreadCountersRepository;

        public TopicRepository(
            ReadDmDbContext dmDbContext,
            IUnreadCountersRepository unreadCountersRepository)
        {
            this.dmDbContext = dmDbContext;
            this.unreadCountersRepository = unreadCountersRepository;
        }

        private static readonly Guid NewsForumId = Guid.Parse("00000000-0000-0000-0000-000000000008");
        private static readonly Guid ErrorsForumId = Guid.Parse("00000000-0000-0000-0000-000000000006");

        public Task<int> CountTopics(Guid forumId) =>
            dmDbContext.ForumTopics.CountAsync(t => !t.IsRemoved && t.ForumId == forumId);

        public async Task<IEnumerable<TopicsListItem>> SelectTopics(
            Guid userId, Guid forumId, PagingData pagingData, bool attached)
        {
            var forumType = forumId == NewsForumId
                ? TopicQueries.TopicsSortType.News
                : forumId == ErrorsForumId
                    ? TopicQueries.TopicsSortType.Errors
                    : TopicQueries.TopicsSortType.Default;

            var query = TopicQueries.List(forumId, pagingData, attached, forumType);
            var topics = await dmDbContext.ForumTopicsList
                .FromSql(query,
                    forumId,
                    attached,
                    pagingData == null
                        ? 0
                        : pagingData.PageSize * (pagingData.CurrentPage - 1),
                    pagingData?.PageSize ?? 1000)
                .Select(x => new TopicsListItem
                {
                    Id = x.Id,
                    Forum = new ForaListItem
                    {
                        Id = x.ForumId,
                        Title = x.ForumTitle
                    },
                    Title = x.Title,
                    Text = x.Text,
                    Author = x.Author,
                    CreateDate = x.CreateDate,
                    LastCommentDate = x.LastCommentDate,
                    LastCommentAuthor = x.LastCommentAuthor,
                    TotalCommentsCount = x.TotalCommentsCount,
                    Attached = x.Attached,
                    Closed = x.Closed
                })
                .ToArrayAsync();

            var counters = await unreadCountersRepository.SelectByEntities(
                userId, UnreadEntryType.Message, topics.Select(t => t.Id).ToArray());

            foreach (var topic in topics)
            {
                topic.UnreadCommentsCount = counters[topic.Id];
            }

            return topics;
        }
    }
}