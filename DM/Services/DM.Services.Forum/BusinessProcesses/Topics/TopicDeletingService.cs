using System;
using System.Threading.Tasks;
using DM.Services.Common.Implementation;
using DM.Services.Common.Repositories;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Forum.Authorization;
using DM.Services.MessageQueuing.Publish;

namespace DM.Services.Forum.BusinessProcesses.Topics
{
    /// <inheritdoc />
    public class TopicDeletingService : ITopicDeletingService
    {
        private readonly ITopicReadingService topicReadingService;
        private readonly IIntentionManager intentionManager;
        private readonly ITopicRepository topicRepository;
        private readonly IInvokedEventPublisher invokedEventPublisher;
        private readonly IUnreadCountersRepository unreadCountersRepository;

        /// <inheritdoc />
        public TopicDeletingService(
            ITopicReadingService topicReadingService,
            IIntentionManager intentionManager,
            ITopicRepository topicRepository,
            IInvokedEventPublisher invokedEventPublisher,
            IUnreadCountersRepository unreadCountersRepository)
        {
            this.topicReadingService = topicReadingService;
            this.intentionManager = intentionManager;
            this.topicRepository = topicRepository;
            this.invokedEventPublisher = invokedEventPublisher;
            this.unreadCountersRepository = unreadCountersRepository;
        }

        /// <inheritdoc />
        public async Task DeleteTopic(Guid topicId)
        {
            var topic = await topicReadingService.GetTopic(topicId);
            await intentionManager.ThrowIfForbidden(ForumIntention.AdministrateTopics, topic.Forum);

            await topicRepository.Update(topicId, new UpdateBuilder<ForumTopic>().Field(t => t.IsRemoved, true));
            await unreadCountersRepository.Delete(topicId, UnreadEntryType.Message);
            await invokedEventPublisher.Publish(EventType.DeletedTopic, topicId);
        }
    }
}