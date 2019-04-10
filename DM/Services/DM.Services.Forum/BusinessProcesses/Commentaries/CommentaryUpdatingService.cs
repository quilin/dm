using System.Threading.Tasks;
using DM.Services.Common.Implementation;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Forum.Authorization;
using DM.Services.Forum.Dto.Input;
using DM.Services.Forum.Dto.Output;
using DM.Services.MessageQueuing.Publish;
using FluentValidation;

namespace DM.Services.Forum.BusinessProcesses.Commentaries
{
    /// <inheritdoc />
    public class CommentaryUpdatingService : ICommentaryUpdatingService
    {
        private readonly IValidator<UpdateComment> validator;
        private readonly ICommentaryReadingService commentaryReadingService;
        private readonly IIntentionManager intentionManager;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly ICommentRepository commentRepository;
        private readonly IInvokedEventPublisher invokedEventPublisher;

        /// <inheritdoc />
        public CommentaryUpdatingService(
            IValidator<UpdateComment> validator,
            ICommentaryReadingService commentaryReadingService,
            IIntentionManager intentionManager,
            IDateTimeProvider dateTimeProvider,
            ICommentRepository commentRepository,
            IInvokedEventPublisher invokedEventPublisher)
        {
            this.validator = validator;
            this.commentaryReadingService = commentaryReadingService;
            this.intentionManager = intentionManager;
            this.dateTimeProvider = dateTimeProvider;
            this.commentRepository = commentRepository;
            this.invokedEventPublisher = invokedEventPublisher;
        }
        
        /// <inheritdoc />
        public async Task<Comment> Update(UpdateComment updateComment)
        {
            await validator.ValidateAndThrowAsync(updateComment);
            var comment = await commentaryReadingService.Get(updateComment.CommentId);
            await intentionManager.ThrowIfForbidden(CommentIntention.Edit, comment);
            var updatedComment = await commentRepository.Update(updateComment.CommentId, new UpdateBuilder<ForumComment>()
                .Field(f => f.Text, updateComment.Text)
                .Field(f => f.LastUpdateDate, dateTimeProvider.Now));
            await invokedEventPublisher.Publish(EventType.ChangedForumComment, updateComment.CommentId);
            return updatedComment;
        }
    }
}