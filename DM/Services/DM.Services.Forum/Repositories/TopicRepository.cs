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

        public Task<int> CountTopics(Guid forumId) =>
            dmDbContext.ForumTopics.CountAsync(t => !t.IsRemoved && t.ForumId == forumId);

        public async Task<IEnumerable<TopicsListItem>> SelectTopics(Guid userId, Guid forumId, PagingData pagingData)
        {
            var topics = await dmDbContext.ForumTopicsList
                .FromSql($@"select
                    ""Id"",
                    ""CreateDate"",
                    ""Author"",
                    ""Title"",
                    ""Text"",
                    ""Attached"",
                    ""Closed"",
                    ""CommentAuthor"" AS ""LastCommentAuthor"",
                    ""CommentCreateDate"" AS ""LastCommentDate"",
                    ""TotalCommentsCount""
                    from 
                    (select
                        ft.""ForumTopicId"" as ""Id"",
                        ft.""CreateDate"",
                        tu.""Login"" as ""Author"",
                        ft.""Title"",
                        ft.""Text"",
                        ft.""Attached"",
                        ft.""Closed"",
                        cu.""Login"" as ""CommentAuthor"",
                        c.""CreateDate"" as ""CommentCreateDate"",
                        count(c.*)   over(partition by ft.""ForumTopicId"" order by c.""CreateDate"" asc) as ""TotalCommentsCount"",
                        row_number() over(partition by ft.""ForumTopicId"" order by c.""CreateDate"" desc) as rt
                        from ""ForumTopics"" as ft
                        inner join ""Users"" as tu on ft.""UserId"" = tu.""UserId""
                        inner join ""Comments"" as c on c.""EntityId"" = ft.""ForumTopicId""
                        inner join ""Users"" as cu on c.""UserId"" = cu.""UserId"" 
                        where
                            ft.""ForumId"" = {forumId} and
                            ft.""Attached"" = false and
                            ft.""IsRemoved"" = false and
                            c.""IsRemoved"" = false
                        order by ft.""CreateDate"" desc) as com
                    where com.rt = 1
                    limit {pagingData.PageSize} offset {pagingData.PageSize * (pagingData.CurrentPage - 1)}")
                .Select(x => new TopicsListItem
                {
                    Id = x.Id,
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
            var topicIds = topics.Select(t => t.Id).ToArray();
            var counters = await unreadCountersRepository.SelectByEntities(userId, topicIds, UnreadEntryType.Message);

            foreach (var topic in topics)
            {
                topic.UnreadCommentsCount = counters[topic.Id];
            }

            return topics;
        }
    }
}