using System;
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
internal class LikeService : LikeServiceBase, ILikeService
{
    private readonly ITopicReadingService topicReadingService;
    private readonly ICommentaryReadingService commentaryReadingService;
    private readonly IIntentionManager intentionManager;

    /// <inheritdoc />
    public LikeService(
        ITopicReadingService topicReadingService,
        ICommentaryReadingService commentaryReadingService,
        IIntentionManager intentionManager,
        IIdentityProvider identityProvider,
        ILikeFactory likeFactory,
        ILikeRepository likeRepository,
        IInvokedEventProducer invokedEventProducer)
        : base(identityProvider, likeFactory, likeRepository, invokedEventProducer)
    {
        this.topicReadingService = topicReadingService;
        this.commentaryReadingService = commentaryReadingService;
        this.intentionManager = intentionManager;
    }

    /// <inheritdoc />
    public async Task<GeneralUser> LikeTopic(Guid topicId)
    {
        var topic = await topicReadingService.GetTopic(topicId);
        intentionManager.ThrowIfForbidden(TopicIntention.Like, topic);
        return await Like(topic, EventType.LikedTopic);
    }

    /// <inheritdoc />
    public async Task<GeneralUser> LikeComment(Guid commentId)
    {
        var comment = await commentaryReadingService.Get(commentId);
        intentionManager.ThrowIfForbidden(CommentIntention.Like, comment);
        return await Like(comment, EventType.LikedForumComment);
    }

    /// <inheritdoc />
    public async Task DislikeTopic(Guid topicId)
    {
        var topic = await topicReadingService.GetTopic(topicId);
        intentionManager.ThrowIfForbidden(TopicIntention.Like, topic);
        await Dislike(topic);
    }

    /// <inheritdoc />
    public async Task DislikeComment(Guid commentId)
    {
        var comment = await commentaryReadingService.Get(commentId);
        intentionManager.ThrowIfForbidden(CommentIntention.Like, comment);
        await Dislike(comment);
    }
}