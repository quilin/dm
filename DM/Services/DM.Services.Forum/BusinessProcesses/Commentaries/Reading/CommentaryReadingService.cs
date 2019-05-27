using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Repositories;
using DM.Services.Core.Dto;
using DM.Services.Core.Exceptions;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.Forum.BusinessProcesses.Topics.Reading;
using Comment = DM.Services.Forum.Dto.Output.Comment;

namespace DM.Services.Forum.BusinessProcesses.Commentaries.Reading
{
    /// <inheritdoc />
    public class CommentaryReadingService : ICommentaryReadingService
    {
        private readonly ITopicReadingService topicReadingService;
        private readonly ICommentaryReadingRepository commentaryRepository;
        private readonly IUnreadCountersRepository unreadCountersRepository;
        private readonly IIdentity identity;

        /// <inheritdoc />
        public CommentaryReadingService(
            ITopicReadingService topicReadingService,
            IIdentityProvider identityProvider,
            ICommentaryReadingRepository commentaryRepository,
            IUnreadCountersRepository unreadCountersRepository)
        {
            this.topicReadingService = topicReadingService;
            this.commentaryRepository = commentaryRepository;
            this.unreadCountersRepository = unreadCountersRepository;
            identity = identityProvider.Current;
        }

        /// <inheritdoc />
        public async Task<(IEnumerable<Comment> comments, PagingResult paging)> GetCommentsList(
            Guid topicId, PagingQuery query)
        {
            await topicReadingService.GetTopic(topicId);

            var totalCount = await commentaryRepository.Count(topicId);
            var paging = new PagingData(query, identity.Settings.CommentsPerPage, totalCount);

            var comments = await commentaryRepository.Get(topicId, paging);
            if (identity.User.IsAuthenticated)
            {
                await unreadCountersRepository.Flush(identity.User.UserId, UnreadEntryType.Message, topicId);
            }

            return (comments, paging.Result);
        }

        /// <inheritdoc />
        public async Task<Comment> Get(Guid commentId)
        {
            return await commentaryRepository.Get(commentId) ??
                throw new HttpException(HttpStatusCode.Gone, $"Comment {commentId} not found");
        }
    }
}