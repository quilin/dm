using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Forum.Authorization;
using DM.Services.Forum.BusinessProcesses.Topics.Reading;
using DM.Services.Forum.Dto.Input;
using DM.Services.Forum.Dto.Output;
using DM.Services.MessageQueuing.Publish;
using FluentValidation;

namespace DM.Services.Forum.BusinessProcesses.Commentaries.Creating
{
    /// <inheritdoc />
    public class CommentaryCreatingService : ICommentaryCreatingService
    {
        private readonly IValidator<CreateComment> validator;
        private readonly ITopicReadingService topicReadingService;
        private readonly IIntentionManager intentionManager;
        private readonly IIdentity identity;
        private readonly ICommentaryFactory commentaryFactory;
        private readonly ICommentaryCreatingRepository repository;
        private readonly IUnreadCountersRepository countersRepository;
        private readonly IInvokedEventPublisher invokedEventPublisher;

        /// <inheritdoc />
        public CommentaryCreatingService(
            IValidator<CreateComment> validator,
            ITopicReadingService topicReadingService,
            IIntentionManager intentionManager,
            IIdentityProvider identityProvider,
            ICommentaryFactory commentaryFactory,
            ICommentaryCreatingRepository repository,
            IUnreadCountersRepository countersRepository,
            IInvokedEventPublisher invokedEventPublisher)
        {
            this.validator = validator;
            this.topicReadingService = topicReadingService;
            this.intentionManager = intentionManager;
            this.commentaryFactory = commentaryFactory;
            this.repository = repository;
            this.countersRepository = countersRepository;
            this.invokedEventPublisher = invokedEventPublisher;
            identity = identityProvider.Current;
        }

        /// <inheritdoc />
        public async Task<Comment> Create(CreateComment createComment)
        {
            await validator.ValidateAndThrowAsync(createComment);

            var topic = await topicReadingService.GetTopic(createComment.TopicId);
            await intentionManager.ThrowIfForbidden(TopicIntention.CreateComment, topic);

            var comment = commentaryFactory.Create(createComment, identity.User.UserId);
            var createdComment = await repository.Create(comment,
                new UpdateBuilder<ForumTopic>(topic.Id).Field(t => t.LastCommentId, comment.ForumCommentId));
            await countersRepository.Increment(topic.Id, UnreadEntryType.Message);
            await invokedEventPublisher.Publish(EventType.NewForumComment, comment.ForumCommentId);

            return createdComment;
        }
    }
}