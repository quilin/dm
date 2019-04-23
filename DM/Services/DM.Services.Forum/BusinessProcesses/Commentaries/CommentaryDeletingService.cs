using System;
using System.Threading.Tasks;
using DM.Services.Common.Implementation;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Forum.Authorization;
using DM.Services.MessageQueuing.Publish;

namespace DM.Services.Forum.BusinessProcesses.Commentaries
{
    /// <inheritdoc />
    public class CommentaryDeletingService : ICommentaryDeletingService
    {
        private readonly ICommentaryReadingService commentaryReadingService;
        private readonly IIntentionManager intentionManager;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly ICommentRepository commentRepository;
        private readonly IInvokedEventPublisher invokedEventPublisher;

        /// <inheritdoc />
        public CommentaryDeletingService(
            ICommentaryReadingService commentaryReadingService,
            IIntentionManager intentionManager,
            IDateTimeProvider dateTimeProvider,
            ICommentRepository commentRepository,
            IInvokedEventPublisher invokedEventPublisher)
        {
            this.commentaryReadingService = commentaryReadingService;
            this.intentionManager = intentionManager;
            this.dateTimeProvider = dateTimeProvider;
            this.commentRepository = commentRepository;
            this.invokedEventPublisher = invokedEventPublisher;
        }
        
        /// <inheritdoc />
        public async Task Delete(Guid commentId)
        {
            var comment = await commentaryReadingService.Get(commentId);
            await intentionManager.ThrowIfForbidden(CommentIntention.Delete, comment);
            await commentRepository.Update(new UpdateBuilder<ForumComment>(commentId)
                .Field(c => c.LastUpdateDate, dateTimeProvider.Now)
                .Field(c => c.IsRemoved, true));
            await invokedEventPublisher.Publish(EventType.DeletedForumComment, commentId);
        }
    }
}