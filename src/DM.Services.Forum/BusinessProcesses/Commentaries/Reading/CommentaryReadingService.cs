using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Core.Dto;
using DM.Services.Core.Exceptions;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.Forum.BusinessProcesses.Fora;
using DM.Services.Forum.BusinessProcesses.Topics.Reading;
using Comment = DM.Services.Common.Dto.Comment;

namespace DM.Services.Forum.BusinessProcesses.Commentaries.Reading;

/// <inheritdoc />
internal class CommentaryReadingService(
    ITopicReadingService topicReadingService,
    IForumReadingService forumReadingService,
    IIdentityProvider identityProvider,
    IUnreadCountersRepository unreadCountersRepository,
    ICommentaryReadingRepository commentaryRepository) : ICommentaryReadingService
{
    /// <inheritdoc />
    public async Task<(IEnumerable<Comment> comments, PagingResult paging)> Get(
        Guid topicId, PagingQuery query, CancellationToken cancellationToken)
    {
        await topicReadingService.GetTopic(topicId, cancellationToken);

        var totalCount = await commentaryRepository.Count(topicId, cancellationToken);
        var paging = new PagingData(query, identityProvider.Current.Settings.Paging.CommentsPerPage, totalCount);

        var comments = await commentaryRepository.Get(topicId, paging, cancellationToken);

        return (comments, paging.Result);
    }

    /// <inheritdoc />
    public async Task<Comment> Get(Guid commentId, CancellationToken cancellationToken)
    {
        return await commentaryRepository.Get(commentId, cancellationToken) ??
               throw new HttpException(HttpStatusCode.Gone, $"Comment {commentId} not found");
    }

    /// <inheritdoc />
    public async Task MarkAsRead(Guid topicId, CancellationToken cancellationToken)
    {
        await topicReadingService.GetTopic(topicId, cancellationToken);
        await unreadCountersRepository.Flush(identityProvider.Current.User.UserId,
            UnreadEntryType.Message, topicId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task MarkAsRead(string forumTitle, CancellationToken cancellationToken)
    {
        var forum = await forumReadingService.GetForum(forumTitle, true, cancellationToken);
        await unreadCountersRepository.FlushAll(identityProvider.Current.User.UserId,
            UnreadEntryType.Message, forum.Id, cancellationToken);
    }
}