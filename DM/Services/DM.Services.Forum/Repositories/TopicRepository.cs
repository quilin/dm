using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                .Select(t => new TopicsListItem
                {
                    Id = t.ForumTopicId,
                    Forum = new ForaListItem
                    {
                        Id = t.Forum.ForumId,
                        Title = t.Forum.Title,
                        UnreadTopicsCount = 0
                    },
                    Title = t.Title,
                    Text = t.Text,
                    CreateDate = t.CreateDate,
                    Closed = t.Closed,
                    Attached = t.Attached,
                    LastActivityDate = t.LastComment == null ? t.CreateDate : t.LastComment.CreateDate,
                    Author = new GeneralUser
                    {
                        UserId = t.Author.UserId,
                        Login = t.Author.Login,
                        Role = t.Author.Role,
                        AccessPolicy = t.Author.AccessPolicy,
                        LastVisitDate = t.Author.LastVisitDate,
                        ProfilePictureUrl = t.Author.ProfilePictures
                            .Where(p => !p.IsRemoved)
                            .Select(p => p.VirtualPath)
                            .FirstOrDefault(),
                        RatingDisabled = t.Author.RatingDisabled,
                        QualityRating = t.Author.QualityRating,
                        QuantityRating = t.Author.QuantityRating
                    },
                    TotalCommentsCount = t.Comments.Count(c => !c.IsRemoved),
                    LastComment = t.LastComment == null
                        ? null
                        : new LastComment
                        {
                            CreateDate = t.LastComment.CreateDate,
                            Author = new GeneralUser
                            {
                                UserId = t.LastComment.Author.UserId,
                                Login = t.LastComment.Author.Login,
                                Role = t.LastComment.Author.Role,
                                AccessPolicy = t.LastComment.Author.AccessPolicy,
                                LastVisitDate = t.LastComment.Author.LastVisitDate,
                                ProfilePictureUrl = t.LastComment.Author.ProfilePictures
                                    .Where(p => !p.IsRemoved)
                                    .Select(p => p.VirtualPath)
                                    .FirstOrDefault(),
                                RatingDisabled = t.LastComment.Author.RatingDisabled,
                                QualityRating = t.LastComment.Author.QualityRating,
                                QuantityRating = t.LastComment.Author.QuantityRating
                            }
                        }
                });

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