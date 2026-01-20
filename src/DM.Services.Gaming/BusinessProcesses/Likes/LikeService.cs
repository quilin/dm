using System;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Common.BusinessProcesses.Likes;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Commentaries.Reading;
using DM.Services.MessageQueuing.GeneralBus;

namespace DM.Services.Gaming.BusinessProcesses.Likes;

/// <summary>
/// Forum like service
/// </summary>
internal class LikeService(
    ICommentaryReadingService commentaryReadingService,
    IIntentionManager intentionManager,
    IIdentityProvider identityProvider,
    ILikeFactory likeFactory,
    ILikeRepository likeRepository,
    IInvokedEventProducer invokedEventProducer)
    : LikeServiceBase(identityProvider, likeFactory, likeRepository, invokedEventProducer), ILikeService
{
    /// <inheritdoc />
    public async Task<GeneralUser> LikeComment(Guid commentId, CancellationToken cancellationToken)
    {
        var comment = await commentaryReadingService.Get(commentId, cancellationToken);
        intentionManager.ThrowIfForbidden(CommentIntention.Like, comment);
        return await Like(comment, EventType.LikedGameComment, cancellationToken);
    }

    /// <inheritdoc />
    public async Task DislikeComment(Guid commentId, CancellationToken cancellationToken)
    {
        var comment = await commentaryReadingService.Get(commentId, cancellationToken);
        intentionManager.ThrowIfForbidden(CommentIntention.Like, comment);
        await Dislike(comment, cancellationToken);
    }
}