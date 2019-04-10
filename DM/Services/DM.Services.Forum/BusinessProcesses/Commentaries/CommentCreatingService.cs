using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Dto;
using DM.Services.Common.Implementation;
using DM.Services.Core.Dto.Enums;
using DM.Services.Forum.Authorization;
using DM.Services.Forum.BusinessProcesses.Topics;
using DM.Services.Forum.Dto.Input;
using DM.Services.MessageQueuing.Publish;
using FluentValidation;

namespace DM.Services.Forum.BusinessProcesses.Commentaries
{
    /// <inheritdoc />
    public class CommentCreatingService : ICommentaryCreatingService
    {
        private readonly IValidator<CreateComment> validator;
        private readonly ITopicReadingService topicReadingService;
        private readonly IIntentionManager intentionManager;
        private readonly IIdentity identity;
        private readonly ICommentFactory commentFactory;
        private readonly Services.Common.BusinessProcesses.Commentaries.ICommentRepository commentRepository;
        private readonly IInvokedEventPublisher invokedEventPublisher;

        /// <inheritdoc />
        public CommentCreatingService(
            IValidator<CreateComment> validator,
            ITopicReadingService topicReadingService,
            IIntentionManager intentionManager,
            IIdentityProvider identityProvider,
            ICommentFactory commentFactory,
            Services.Common.BusinessProcesses.Commentaries.ICommentRepository commentRepository,
            IInvokedEventPublisher invokedEventPublisher)
        {
            this.validator = validator;
            this.topicReadingService = topicReadingService;
            this.intentionManager = intentionManager;
            this.commentFactory = commentFactory;
            this.commentRepository = commentRepository;
            this.invokedEventPublisher = invokedEventPublisher;
            identity = identityProvider.Current;
        }
        
        /// <inheritdoc />
        public async Task<Comment> Create(CreateComment createComment)
        {
            await validator.ValidateAndThrowAsync(createComment);

            var topic = await topicReadingService.GetTopic(createComment.TopicId);
            await intentionManager.ThrowIfForbidden(TopicIntention.CreateComment, topic);

            var comment = commentFactory.Create(createComment, identity.User.UserId);
            var createdComment = await commentRepository.Create(comment);
            await invokedEventPublisher.Publish(EventType.NewForumComment, comment.CommentId);

            return createdComment;
        }
    }
}