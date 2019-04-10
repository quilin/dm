using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Repositories;
using DM.Services.Core.Dto;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.Forum.BusinessProcesses.Topics;
using Comment = DM.Services.Forum.Dto.Output.Comment;

namespace DM.Services.Forum.BusinessProcesses.Commentaries
{
    /// <inheritdoc />
    public class CommentReadingService : ICommentaryReadingService
    {
        private readonly ITopicReadingService topicReadingService;
        private readonly ICommentRepository commentRepository;
        private readonly IUnreadCountersRepository unreadCountersRepository;
        private readonly IIdentity identity;

        /// <inheritdoc />
        public CommentReadingService(
            ITopicReadingService topicReadingService,
            IIdentityProvider identityProvider,
            ICommentRepository commentRepository,
            IUnreadCountersRepository unreadCountersRepository)
        {
            this.topicReadingService = topicReadingService;
            this.commentRepository = commentRepository;
            this.unreadCountersRepository = unreadCountersRepository;
            identity = identityProvider.Current;
        }
        
        /// <inheritdoc />
        public async Task<(IEnumerable<Comment> comments, PagingData paging)> GetCommentsList(
            Guid topicId, int entityNumber)
        {
            await topicReadingService.GetTopic(topicId);

            var pageSize = identity.Settings.CommentsPerPage;
            var commentsCount = await commentRepository.Count(topicId);
            var pagingData = PagingData.Create(commentsCount, entityNumber, pageSize);

            var comments = await commentRepository.Get(topicId, pagingData);
            if (identity.User.IsAuthenticated)
            {
                await unreadCountersRepository.Flush(identity.User.UserId, UnreadEntryType.Message, topicId);
            }

            return (comments, pagingData);
        }

        /// <inheritdoc />
        public Task<Comment> Get(Guid commentId)
        {
            return commentRepository.Get(commentId);
        }
    }
}