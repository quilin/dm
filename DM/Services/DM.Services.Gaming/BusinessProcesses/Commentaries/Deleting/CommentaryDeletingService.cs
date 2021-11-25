using System;
using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Commentaries.Reading;
using DM.Services.Gaming.BusinessProcesses.Commentaries.Updating;
using DM.Services.MessageQueuing.GeneralBus;
using Comment = DM.Services.DataAccess.BusinessObjects.Common.Comment;

namespace DM.Services.Gaming.BusinessProcesses.Commentaries.Deleting
{
    /// <inheritdoc />
    internal class CommentaryDeletingService : ICommentaryDeletingService
    {
        private readonly IIntentionManager intentionManager;
        private readonly IUpdateBuilderFactory updateBuilderFactory;
        private readonly ICommentaryReadingService readingService;
        private readonly ICommentaryUpdatingRepository updatingRepository;
        private readonly IUnreadCountersRepository unreadCountersRepository;
        private readonly IInvokedEventPublisher invokedEventPublisher;

        /// <inheritdoc />
        public CommentaryDeletingService(
            IIntentionManager intentionManager,
            IUpdateBuilderFactory updateBuilderFactory,
            ICommentaryReadingService readingService,
            ICommentaryUpdatingRepository updatingRepository,
            IUnreadCountersRepository unreadCountersRepository,
            IInvokedEventPublisher invokedEventPublisher)
        {
            this.intentionManager = intentionManager;
            this.updateBuilderFactory = updateBuilderFactory;
            this.readingService = readingService;
            this.updatingRepository = updatingRepository;
            this.unreadCountersRepository = unreadCountersRepository;
            this.invokedEventPublisher = invokedEventPublisher;
        }

        /// <inheritdoc />
        public async Task Delete(Guid commentId)
        {
            var comment = await readingService.Get(commentId);
            intentionManager.ThrowIfForbidden(CommentIntention.Delete, comment);

            var updateComment = updateBuilderFactory.Create<Comment>(commentId)
                .Field(c => c.IsRemoved, true);
            await updatingRepository.Update(updateComment);
            await unreadCountersRepository.Decrement(comment.EntityId, UnreadEntryType.Message, comment.CreateDate);

            await invokedEventPublisher.Publish(EventType.DeletedGameComment, commentId);
        }
    }
}