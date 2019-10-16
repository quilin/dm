using System;
using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Forum.Authorization;
using DM.Services.MessageQueuing.Publish;
using Comment = DM.Services.DataAccess.BusinessObjects.Common.Comment;

namespace DM.Services.Forum.BusinessProcesses.Commentaries.Deleting
{
    /// <inheritdoc />
    public class CommentaryDeletingService : ICommentaryDeletingService
    {
        private readonly IIntentionManager intentionManager;
        private readonly IUpdateBuilderFactory updateBuilderFactory;
        private readonly ICommentaryDeletingRepository repository;
        private readonly IUnreadCountersRepository unreadCountersRepository;
        private readonly IInvokedEventPublisher invokedEventPublisher;

        /// <inheritdoc />
        public CommentaryDeletingService(
            IIntentionManager intentionManager,
            IUpdateBuilderFactory updateBuilderFactory,
            ICommentaryDeletingRepository repository,
            IUnreadCountersRepository unreadCountersRepository,
            IInvokedEventPublisher invokedEventPublisher)
        {
            this.intentionManager = intentionManager;
            this.updateBuilderFactory = updateBuilderFactory;
            this.repository = repository;
            this.unreadCountersRepository = unreadCountersRepository;
            this.invokedEventPublisher = invokedEventPublisher;
        }
        
        /// <inheritdoc />
        public async Task Delete(Guid commentId)
        {
            var comment = await repository.GetForDelete(commentId);
            await intentionManager.ThrowIfForbidden(CommentIntention.Delete, (Dto.Output.Comment) comment);

            var updateTopic = updateBuilderFactory.Create<ForumTopic>(comment.TopicId);
            if (comment.IsLastCommentOfTopic)
            {
                var previousCommentaryId = await repository.GetSecondLastCommentId(comment.TopicId);
                updateTopic = updateTopic.Field(t => t.LastCommentId, previousCommentaryId);
            }

            var updateComment = updateBuilderFactory.Create<Comment>(commentId)
                .Field(c => c.IsRemoved, true);
            await repository.Delete(updateComment, updateTopic);
            await unreadCountersRepository.Decrement(comment.TopicId, UnreadEntryType.Message, comment.CreateDate);

            await invokedEventPublisher.Publish(EventType.DeletedForumComment, commentId);
        }
    }
}