using System;
using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Forum.Authorization;
using DM.Services.Forum.Dto.Output;
using DM.Services.MessageQueuing.Publish;

namespace DM.Services.Forum.BusinessProcesses.Commentaries.Deleting
{
    /// <inheritdoc />
    public class CommentaryDeletingService : ICommentaryDeletingService
    {
        private readonly IIntentionManager intentionManager;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly ICommentaryDeletingRepository repository;
        private readonly IUnreadCountersRepository unreadCountersRepository;
        private readonly IInvokedEventPublisher invokedEventPublisher;

        /// <inheritdoc />
        public CommentaryDeletingService(
            IIntentionManager intentionManager,
            IDateTimeProvider dateTimeProvider,
            ICommentaryDeletingRepository repository,
            IUnreadCountersRepository unreadCountersRepository,
            IInvokedEventPublisher invokedEventPublisher)
        {
            this.intentionManager = intentionManager;
            this.dateTimeProvider = dateTimeProvider;
            this.repository = repository;
            this.unreadCountersRepository = unreadCountersRepository;
            this.invokedEventPublisher = invokedEventPublisher;
        }
        
        /// <inheritdoc />
        public async Task Delete(Guid commentId)
        {
            var comment = await repository.GetForDelete(commentId);
            await intentionManager.ThrowIfForbidden(CommentIntention.Delete, (Comment) comment);

            UpdateBuilder<ForumTopic> updateTopic;
            if (comment.IsLastCommentOfTopic)
            {
                updateTopic = new UpdateBuilder<ForumTopic>(comment.TopicId);
                var previousCommentaryId = await repository.GetSecondLastCommentId(comment.TopicId);
                updateTopic.Field(t => t.LastCommentId, previousCommentaryId);
            }
            else
            {
                updateTopic = UpdateBuilder<ForumTopic>.Empty();
            }

            await repository.Delete(new UpdateBuilder<ForumComment>(commentId)
                .Field(c => c.LastUpdateDate, dateTimeProvider.Now)
                .Field(c => c.IsRemoved, true), updateTopic);
            await unreadCountersRepository.Decrement(commentId, UnreadEntryType.Message, comment.CreateDate);

            await invokedEventPublisher.Publish(EventType.DeletedForumComment, commentId);
        }
    }
}