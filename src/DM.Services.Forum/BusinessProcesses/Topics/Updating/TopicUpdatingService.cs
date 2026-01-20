using System.Threading;
using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Forum.Authorization;
using DM.Services.Forum.BusinessProcesses.Fora;
using DM.Services.Forum.BusinessProcesses.Topics.Reading;
using DM.Services.Forum.Dto.Input;
using DM.Services.Forum.Dto.Output;
using DM.Services.MessageQueuing.GeneralBus;
using FluentValidation;

namespace DM.Services.Forum.BusinessProcesses.Topics.Updating;

/// <inheritdoc />
internal class TopicUpdatingService(
    IValidator<UpdateTopic> validator,
    ITopicReadingService topicReadingService,
    IForumReadingService forumReadingService,
    IIntentionManager intentionManager,
    IUpdateBuilderFactory updateBuilderFactory,
    ITopicUpdatingRepository repository,
    IUnreadCountersRepository unreadCountersRepository,
    IInvokedEventProducer invokedEventProducer) : ITopicUpdatingService
{
    /// <inheritdoc />
    public async Task<Topic> UpdateTopic(UpdateTopic updateTopic, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(updateTopic, cancellationToken);
        var oldTopic = await topicReadingService.GetTopic(updateTopic.TopicId, cancellationToken);

        intentionManager.ThrowIfForbidden(TopicIntention.Edit, oldTopic);

        var changes = updateBuilderFactory.Create<ForumTopic>(updateTopic.TopicId)
            .MaybeField(t => t.Title, updateTopic.Title?.Trim())
            .MaybeField(t => t.Text, updateTopic.Text?.Trim());

        if (intentionManager.IsAllowed(ForumIntention.AdministrateTopics, oldTopic.Forum))
        {
            changes
                .MaybeField(t => t.Closed, updateTopic.Closed)
                .MaybeField(t => t.Attached, updateTopic.Attached);

            if (updateTopic.ForumTitle != default &&
                oldTopic.Forum.Title != updateTopic.ForumTitle)
            {
                var forum = await forumReadingService.GetForum(updateTopic.ForumTitle, false, cancellationToken);
                intentionManager.ThrowIfForbidden(ForumIntention.CreateTopic, forum);
                changes.Field(t => t.ForumId, forum.Id);
                await unreadCountersRepository.ChangeParent(
                    oldTopic.Forum.Id, UnreadEntryType.Message, forum.Id, cancellationToken);
            }
        }

        var topic = await repository.Update(changes, cancellationToken);
        await invokedEventProducer.Send(EventType.ChangedForumTopic, topic.Id);

        return topic;
    }
}