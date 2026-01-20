using System;
using System.Threading;
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
internal class TopicDeletingService(
    ITopicReadingService topicReadingService,
    IIntentionManager intentionManager,
    IUpdateBuilderFactory updateBuilderFactory,
    ITopicUpdatingRepository repository,
    IInvokedEventProducer invokedEventProducer,
    IUnreadCountersRepository unreadCountersRepository) : ITopicDeletingService
{
    /// <inheritdoc />
    public async Task DeleteTopic(Guid topicId, CancellationToken cancellationToken)
    {
        var topic = await topicReadingService.GetTopic(topicId, cancellationToken);
        intentionManager.ThrowIfForbidden(ForumIntention.AdministrateTopics, topic.Forum);

        var updateBuilder = updateBuilderFactory.Create<ForumTopic>(topicId).Field(t => t.IsRemoved, true);
        await repository.Update(updateBuilder, cancellationToken);
        await unreadCountersRepository.Delete(topicId, UnreadEntryType.Message, cancellationToken);
        await invokedEventProducer.Send(EventType.DeletedForumTopic, topicId);
    }
}