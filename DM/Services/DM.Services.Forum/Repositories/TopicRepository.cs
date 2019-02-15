using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Services.DataAccess;
using DM.Services.Forum.Dto;
using Microsoft.EntityFrameworkCore;
using Npgsql;

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

            var forumIdParameter = new NpgsqlParameter("@ForumId", forumId);
            var attachedParameter = new NpgsqlParameter("@Attached", attached);
            var skipParameter = new NpgsqlParameter(@"Page_From",
                pagingData == null ? 0 : pagingData.PageSize * (pagingData.CurrentPage - 1));
            var sizeParameter = new NpgsqlParameter("@Page_Size", pagingData?.PageSize ?? 0);
            var query = TopicQueries.List(pagingData, attached, forumType);
            return await dmDbContext.ForumTopicsList
                .FromSql(query, forumIdParameter, attachedParameter, skipParameter, sizeParameter)
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
                    Author = new GeneralUser
                    {
                        UserId = x.AuthorUserId,
                        Login = x.AuthorLogin,
                        Role = x.AuthorRole,
                        AccessPolicy = x.AuthorAccessPolicy,
                        LastVisitDate = x.AuthorLastVisitDate,
                        ProfilePictureUrl = x.AuthorProfilePictureUrl,
                        RatingDisabled = x.AuthorRatingDisabled,
                        QualityRating = x.AuthorQualityRating,
                        QuantityRating = x.AuthorQuantityRating
                    },
                    CreateDate = x.CreateDate,
                    LastComment = new LastComment
                    {
                        CreateDate = x.LastCommentDate,
                        Author = new GeneralUser
                        {
                            UserId = x.LastCommentAuthorUserId,
                            Login = x.LastCommentAuthorLogin,
                            Role = x.LastCommentAuthorRole,
                            AccessPolicy = x.LastCommentAuthorAccessPolicy,
                            LastVisitDate = x.LastCommentAuthorLastVisitDate,
                            ProfilePictureUrl = x.LastCommentAuthorProfilePictureUrl,
                            RatingDisabled = x.LastCommentAuthorRatingDisabled,
                            QualityRating = x.LastCommentAuthorQualityRating,
                            QuantityRating = x.LastCommentAuthorQuantityRating
                        }
                    },
                    TotalCommentsCount = x.TotalCommentsCount,
                    Attached = x.Attached,
                    Closed = x.Closed
                })
                .ToArrayAsync();
        }
    }
}