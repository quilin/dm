using System;
using System.Collections.Generic;
using System.Net;
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
internal class CommentaryReadingService : ICommentaryReadingService
{
    private readonly ITopicReadingService topicReadingService;
    private readonly IForumReadingService forumReadingService;
    private readonly IUnreadCountersRepository unreadCountersRepository;
    private readonly ICommentaryReadingRepository commentaryRepository;
    private readonly IIdentityProvider identityProvider;

    /// <inheritdoc />
    public CommentaryReadingService(
        ITopicReadingService topicReadingService,
        IForumReadingService forumReadingService,
        IIdentityProvider identityProvider,
        IUnreadCountersRepository unreadCountersRepository,
        ICommentaryReadingRepository commentaryRepository)
    {
        this.topicReadingService = topicReadingService;
        this.forumReadingService = forumReadingService;
        this.unreadCountersRepository = unreadCountersRepository;
        this.commentaryRepository = commentaryRepository;
        this.identityProvider = identityProvider;
    }

    /// <inheritdoc />
    public async Task<(IEnumerable<Comment> comments, PagingResult paging)> Get(
        Guid topicId, PagingQuery query)
    {
        await topicReadingService.GetTopic(topicId);

        var totalCount = await commentaryRepository.Count(topicId);
        var paging = new PagingData(query, identityProvider.Current.Settings.Paging.CommentsPerPage, totalCount);

        var comments = await commentaryRepository.Get(topicId, paging);

        return (comments, paging.Result);
    }

    /// <inheritdoc />
    public async Task<Comment> Get(Guid commentId)
    {
        return await commentaryRepository.Get(commentId) ??
               throw new HttpException(HttpStatusCode.Gone, $"Comment {commentId} not found");
    }

    /// <inheritdoc />
    public async Task MarkAsRead(Guid topicId)
    {
        await topicReadingService.GetTopic(topicId);
        await unreadCountersRepository.Flush(identityProvider.Current.User.UserId,
            UnreadEntryType.Message, topicId);
    }

    /// <inheritdoc />
    public async Task MarkAsRead(string forumTitle)
    {
        var forum = await forumReadingService.GetForum(forumTitle);
        await unreadCountersRepository.FlushAll(identityProvider.Current.User.UserId,
            UnreadEntryType.Message, forum.Id);
    }
}