using System;
using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Forum.Authorization;
using DM.Services.Forum.BusinessProcesses.Topics.Reading;
using DM.Services.Forum.BusinessProcesses.Topics.Updating;
using DM.Services.MessageQueuing.GeneralBus;

namespace DM.Services.Forum.BusinessProcesses.Topics.Deleting;

/// <inheritdoc />
internal class TopicDeletingService : ITopicDeletingService
{
    private readonly ITopicReadingService topicReadingService;
    private readonly IIntentionManager intentionManager;
    private readonly IUpdateBuilderFactory updateBuilderFactory;
    private readonly ITopicUpdatingRepository repository;
    private readonly IInvokedEventProducer invokedEventProducer;
    private readonly IUnreadCountersRepository unreadCountersRepository;

    /// <inheritdoc />
    public TopicDeletingService(
        ITopicReadingService topicReadingService,
        IIntentionManager intentionManager,
        IUpdateBuilderFactory updateBuilderFactory,
        ITopicUpdatingRepository repository,
        IInvokedEventProducer invokedEventProducer,
        IUnreadCountersRepository unreadCountersRepository)
    {
        this.topicReadingService = topicReadingService;
        this.intentionManager = intentionManager;
        this.updateBuilderFactory = updateBuilderFactory;
        this.repository = repository;
        this.invokedEventProducer = invokedEventProducer;
        this.unreadCountersRepository = unreadCountersRepository;
    }

    /// <inheritdoc />
    public async Task DeleteTopic(Guid topicId)
    {
        var topic = await topicReadingService.GetTopic(topicId);
        intentionManager.ThrowIfForbidden(ForumIntention.AdministrateTopics, topic.Forum);

        await repository.Update(updateBuilderFactory.Create<ForumTopic>(topicId).Field(t => t.IsRemoved, true));
        await unreadCountersRepository.Delete(topicId, UnreadEntryType.Message);
        await invokedEventProducer.Send(EventType.DeletedForumTopic, topicId);
    }
}