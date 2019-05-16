using System;
using System.Threading.Tasks;
using DM.Services.Common.Implementation;
using DM.Services.Common.Repositories;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Forum.Authorization;
using DM.Services.Forum.BusinessProcesses.Topics.Reading;
using DM.Services.Forum.BusinessProcesses.Topics.Updating;
using DM.Services.MessageQueuing.Publish;

namespace DM.Services.Forum.BusinessProcesses.Topics.Deleting
{
    /// <inheritdoc />
    public class TopicDeletingService : ITopicDeletingService
    {
        private readonly ITopicReadingService topicReadingService;
        private readonly IIntentionManager intentionManager;
        private readonly ITopicUpdatingRepository repository;
        private readonly IInvokedEventPublisher invokedEventPublisher;
        private readonly IUnreadCountersRepository unreadCountersRepository;

        /// <inheritdoc />
        public TopicDeletingService(
            ITopicReadingService topicReadingService,
            IIntentionManager intentionManager,
            ITopicUpdatingRepository repository,
            IInvokedEventPublisher invokedEventPublisher,
            IUnreadCountersRepository unreadCountersRepository)
        {
            this.topicReadingService = topicReadingService;
            this.intentionManager = intentionManager;
            this.repository = repository;
            this.invokedEventPublisher = invokedEventPublisher;
            this.unreadCountersRepository = unreadCountersRepository;
        }

        /// <inheritdoc />
        public async Task DeleteTopic(Guid topicId)
        {
            var topic = await topicReadingService.GetTopic(topicId);
            await intentionManager.ThrowIfForbidden(ForumIntention.AdministrateTopics, topic.Forum);

            await repository.Update(new UpdateBuilder<ForumTopic>(topicId).Field(t => t.IsRemoved, true));
            await unreadCountersRepository.Delete(topicId, UnreadEntryType.Message);
            await invokedEventPublisher.Publish(EventType.DeletedTopic, topicId);
        }
    }
}