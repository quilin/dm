using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Core.Dto;
using DM.Services.Core.Exceptions;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;
using DM.Services.Gaming.Dto.Output;
using Comment = DM.Services.Common.Dto.Comment;

namespace DM.Services.Gaming.BusinessProcesses.Commentaries.Reading;

/// <inheritdoc />
internal class CommentaryReadingService(
    IIntentionManager intentionManager,
    IGameReadingService gameReadingService,
    ICommentaryReadingRepository commentaryRepository,
    IUnreadCountersRepository unreadCountersRepository,
    IIdentityProvider identityProvider) : ICommentaryReadingService
{
    /// <inheritdoc />
    public async Task<(IEnumerable<Comment> comments, PagingResult paging, Game game)> Get(
        Guid gameId, PagingQuery query, CancellationToken cancellationToken)
    {
        var game = await gameReadingService.GetGame(gameId, cancellationToken);
        intentionManager.ThrowIfForbidden(GameIntention.ReadComments, game);

        var totalCount = await commentaryRepository.Count(gameId, cancellationToken);
        var paging = new PagingData(query, identityProvider.Current.Settings.Paging.CommentsPerPage, totalCount);

        var comments = await commentaryRepository.Get(gameId, paging, cancellationToken);

        return (comments, paging.Result, game);
    }

    /// <inheritdoc />
    public async Task<Comment> Get(Guid commentId, CancellationToken cancellationToken)
    {
        return await commentaryRepository.Get(commentId, cancellationToken) ??
               throw new HttpException(HttpStatusCode.Gone, $"Comment {commentId} not found");
    }

    /// <inheritdoc />
    public async Task MarkAsRead(Guid gameId, CancellationToken cancellationToken)
    {
        var game = await gameReadingService.GetGame(gameId, cancellationToken);
        intentionManager.ThrowIfForbidden(GameIntention.ReadComments, game);
        await unreadCountersRepository.Flush(identityProvider.Current.User.UserId,
            UnreadEntryType.Message, gameId, cancellationToken);
    }
}