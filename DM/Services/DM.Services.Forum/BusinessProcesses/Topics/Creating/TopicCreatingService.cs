using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.Forum.Authorization;
using DM.Services.Forum.BusinessProcesses.Fora;
using DM.Services.Forum.Dto.Input;
using DM.Services.Forum.Dto.Output;
using DM.Services.MessageQueuing.Publish;
using FluentValidation;

namespace DM.Services.Forum.BusinessProcesses.Topics.Creating
{
    /// <inheritdoc />
    public class TopicCreatingService : ITopicCreatingService
    {
        private readonly IValidator<CreateTopic> validator;
        private readonly IForumReadingService forumReadingService;
        private readonly IIntentionManager intentionManager;
        private readonly ITopicFactory topicFactory;
        private readonly ITopicCreatingRepository repository;
        private readonly IUnreadCountersRepository unreadCountersRepository;
        private readonly IInvokedEventPublisher invokedEventPublisher;
        private readonly IIdentityProvider identityProvider;

        /// <inheritdoc />
        public TopicCreatingService(
            IValidator<CreateTopic> validator,
            IForumReadingService forumReadingService,
            IIntentionManager intentionManager,
            IIdentityProvider identityProvider,
            ITopicFactory topicFactory,
            ITopicCreatingRepository repository,
            IUnreadCountersRepository unreadCountersRepository,
            IInvokedEventPublisher invokedEventPublisher)
        {
            this.validator = validator;
            this.forumReadingService = forumReadingService;
            this.intentionManager = intentionManager;
            this.topicFactory = topicFactory;
            this.repository = repository;
            this.unreadCountersRepository = unreadCountersRepository;
            this.invokedEventPublisher = invokedEventPublisher;
            this.identityProvider = identityProvider;
        }

        /// <inheritdoc />
        public async Task<Topic> CreateTopic(CreateTopic createTopic)
        {
            await validator.ValidateAndThrowAsync(createTopic);

            var forum = await forumReadingService.GetForum(createTopic.ForumTitle);
            intentionManager.ThrowIfForbidden(ForumIntention.CreateTopic, forum);

            var topicToCreate = topicFactory.Create(forum.Id, identityProvider.Current.User.UserId, createTopic);
            var topic = await repository.Create(topicToCreate);

            await Task.WhenAll(
                invokedEventPublisher.Publish(EventType.NewTopic, topic.Id),
                unreadCountersRepository.Create(topic.Id, forum.Id, UnreadEntryType.Message));

            return topic;
        }
    }
}