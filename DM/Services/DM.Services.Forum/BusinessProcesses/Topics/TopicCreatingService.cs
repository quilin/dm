using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Implementation;
using DM.Services.Common.Repositories;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.Forum.Authorization;
using DM.Services.Forum.BusinessProcesses.Fora;
using DM.Services.Forum.Dto.Input;
using DM.Services.Forum.Dto.Output;
using DM.Services.MessageQueuing.Publish;
using FluentValidation;

namespace DM.Services.Forum.BusinessProcesses.Topics
{
    /// <inheritdoc />
    public class TopicCreatingService : ITopicCreatingService
    {
        private readonly IValidator<CreateTopic> validator;
        private readonly IForumReadingService forumReadingService;
        private readonly IIntentionManager intentionManager;
        private readonly ITopicFactory topicFactory;
        private readonly ITopicRepository topicRepository;
        private readonly IUnreadCountersRepository unreadCountersRepository;
        private readonly IInvokedEventPublisher invokedEventPublisher;
        private readonly IIdentity identity;

        /// <inheritdoc />
        public TopicCreatingService(
            IValidator<CreateTopic> validator,
            IForumReadingService forumReadingService,
            IIntentionManager intentionManager,
            IIdentityProvider identityProvider,
            ITopicFactory topicFactory,
            ITopicRepository topicRepository,
            IUnreadCountersRepository unreadCountersRepository,
            IInvokedEventPublisher invokedEventPublisher)
        {
            this.validator = validator;
            this.forumReadingService = forumReadingService;
            this.intentionManager = intentionManager;
            this.topicFactory = topicFactory;
            this.topicRepository = topicRepository;
            this.unreadCountersRepository = unreadCountersRepository;
            this.invokedEventPublisher = invokedEventPublisher;
            identity = identityProvider.Current;
        }

        /// <inheritdoc />
        public async Task<Topic> CreateTopic(CreateTopic createTopic)
        {
            await validator.ValidateAndThrowAsync(createTopic);

            var forum = await forumReadingService.GetForum(createTopic.ForumTitle);
            await intentionManager.ThrowIfForbidden(ForumIntention.CreateTopic, forum);

            var topic = await topicRepository.Create(topicFactory.Create(forum.Id, identity.User.UserId, createTopic));

            await Task.WhenAll(
                invokedEventPublisher.Publish(EventType.NewTopic, topic.Id),
                unreadCountersRepository.Create(topic.Id, forum.Id, UnreadEntryType.Message));

            return topic;
        }
    }
}