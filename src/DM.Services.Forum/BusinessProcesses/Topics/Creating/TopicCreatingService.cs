using System.Threading;
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
using DM.Services.MessageQueuing.GeneralBus;
using FluentValidation;

namespace DM.Services.Forum.BusinessProcesses.Topics.Creating;

/// <inheritdoc />
internal class TopicCreatingService(
    IValidator<CreateTopic> validator,
    IForumReadingService forumReadingService,
    IIntentionManager intentionManager,
    IIdentityProvider identityProvider,
    ITopicFactory topicFactory,
    ITopicCreatingRepository repository,
    IUnreadCountersRepository unreadCountersRepository,
    IInvokedEventProducer invokedEventProducer) : ITopicCreatingService
{
    /// <inheritdoc />
    public async Task<Topic> CreateTopic(CreateTopic createTopic, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(createTopic, cancellationToken);

        var forum = await forumReadingService.GetForum(createTopic.ForumTitle, true, cancellationToken);
        intentionManager.ThrowIfForbidden(ForumIntention.CreateTopic, forum);

        var topicToCreate = topicFactory.Create(forum.Id, identityProvider.Current.User.UserId, createTopic);
        var topic = await repository.Create(topicToCreate, cancellationToken);

        await Task.WhenAll(
            invokedEventProducer.Send(EventType.NewForumTopic, topic.Id),
            unreadCountersRepository.Create(topic.Id, forum.Id, UnreadEntryType.Message, cancellationToken));

        return topic;
    }
}