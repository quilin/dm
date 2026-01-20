using System;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Common.BusinessProcesses.Likes;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.Forum.Authorization;
using DM.Services.Forum.BusinessProcesses.Commentaries.Reading;
using DM.Services.Forum.BusinessProcesses.Topics.Reading;
using DM.Services.MessageQueuing.GeneralBus;

namespace DM.Services.Forum.BusinessProcesses.Likes;

/// <summary>
/// Forum like service
/// </summary>
internal class LikeService(
    ITopicReadingService topicReadingService,
    ICommentaryReadingService commentaryReadingService,
    IIntentionManager intentionManager,
    IIdentityProvider identityProvider,
    ILikeFactory likeFactory,
    ILikeRepository likeRepository,
    IInvokedEventProducer invokedEventProducer)
    : LikeServiceBase(identityProvider, likeFactory, likeRepository, invokedEventProducer), ILikeService
{
    /// <inheritdoc />
    public async Task<GeneralUser> LikeTopic(Guid topicId, CancellationToken cancellationToken)
    {
        var topic = await topicReadingService.GetTopic(topicId, cancellationToken);
        intentionManager.ThrowIfForbidden(TopicIntention.Like, topic);
        return await Like(topic, EventType.LikedTopic, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<GeneralUser> LikeComment(Guid commentId, CancellationToken cancellationToken)
    {
        var comment = await commentaryReadingService.Get(commentId, cancellationToken);
        intentionManager.ThrowIfForbidden(CommentIntention.Like, comment);
        return await Like(comment, EventType.LikedForumComment, cancellationToken);
    }

    /// <inheritdoc />
    public async Task DislikeTopic(Guid topicId, CancellationToken cancellationToken)
    {
        var topic = await topicReadingService.GetTopic(topicId, cancellationToken);
        intentionManager.ThrowIfForbidden(TopicIntention.Like, topic);
        await Dislike(topic, cancellationToken);
    }

    /// <inheritdoc />
    public async Task DislikeComment(Guid commentId, CancellationToken cancellationToken)
    {
        var comment = await commentaryReadingService.Get(commentId, cancellationToken);
        intentionManager.ThrowIfForbidden(CommentIntention.Like, comment);
        await Dislike(comment, cancellationToken);
    }
}