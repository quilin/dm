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

namespace DM.Services.Forum.BusinessProcesses.Commentaries.Reading
{
    /// <inheritdoc />
    public class CommentReadingService : ICommentaryReadingService
    {
        private readonly ITopicReadingService topicReadingService;
        private readonly IReadingCommentRepository readingCommentRepository;
        private readonly IUnreadCountersRepository unreadCountersRepository;
        private readonly IIdentity identity;

        /// <inheritdoc />
        public CommentReadingService(
            ITopicReadingService topicReadingService,
            IIdentityProvider identityProvider,
            IReadingCommentRepository readingCommentRepository,
            IUnreadCountersRepository unreadCountersRepository)
        {
            this.topicReadingService = topicReadingService;
            this.readingCommentRepository = readingCommentRepository;
            this.unreadCountersRepository = unreadCountersRepository;
            identity = identityProvider.Current;
        }
        
        /// <inheritdoc />
        public async Task<(IEnumerable<Comment> comments, PagingResult paging)> GetCommentsList(
            Guid topicId, PagingQuery query)
        {
            await topicReadingService.GetTopic(topicId);

            var totalCount = await readingCommentRepository.Count(topicId);
            var paging = new PagingData(query, identity.Settings.CommentsPerPage, totalCount);

            var comments = await readingCommentRepository.Get(topicId, paging);
            if (identity.User.IsAuthenticated)
            {
                await unreadCountersRepository.Flush(identity.User.UserId, UnreadEntryType.Message, topicId);
            }

            return (comments, paging.Result);
        }

        /// <inheritdoc />
        public Task<Comment> Get(Guid commentId)
        {
            return readingCommentRepository.Get(commentId);
        }
    }
}