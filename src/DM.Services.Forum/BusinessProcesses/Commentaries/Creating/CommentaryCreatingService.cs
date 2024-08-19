using System.Threading;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Common.BusinessProcesses.Commentaries;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Common.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Forum.Authorization;
using DM.Services.Forum.BusinessProcesses.Topics.Reading;
using DM.Services.MessageQueuing.GeneralBus;
using FluentValidation;
using Comment = DM.Services.Common.Dto.Comment;

namespace DM.Services.Forum.BusinessProcesses.Commentaries.Creating;

/// <inheritdoc />
internal class CommentaryCreatingService(
    IValidator<CreateComment> validator,
    ITopicReadingService topicReadingService,
    IIntentionManager intentionManager,
    IIdentityProvider identityProvider,
    ICommentaryFactory commentaryFactory,
    IUpdateBuilderFactory updateBuilderFactory,
    ICommentaryCreatingRepository repository,
    IUnreadCountersRepository countersRepository,
    IInvokedEventProducer invokedEventProducer) : ICommentaryCreatingService
{
    /// <inheritdoc />
    public async Task<Comment> Create(CreateComment createComment, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(createComment, cancellationToken);

        var topic = await topicReadingService.GetTopic(createComment.EntityId, cancellationToken);
        intentionManager.ThrowIfForbidden(TopicIntention.CreateComment, topic);

        var comment = commentaryFactory.Create(createComment, identityProvider.Current.User.UserId);
        var topicUpdate = updateBuilderFactory.Create<ForumTopic>(topic.Id)
            .Field(t => t.LastCommentId, comment.CommentId);
        var createdComment = await repository.Create(comment, topicUpdate, cancellationToken);
        await countersRepository.Increment(topic.Id, UnreadEntryType.Message, cancellationToken);
        await invokedEventProducer.Send(EventType.NewForumComment, comment.CommentId);

        return createdComment;
    }
}