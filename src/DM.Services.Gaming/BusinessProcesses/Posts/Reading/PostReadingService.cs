using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Core.Dto;
using DM.Services.Core.Exceptions;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Rooms.Reading;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.BusinessProcesses.Posts.Reading;

/// <inheritdoc />
internal class PostReadingService : IPostReadingService
{
    private readonly IRoomReadingService roomReadingService;
    private readonly IIntentionManager intentionManager;
    private readonly IPostReadingRepository repository;
    private readonly IUnreadCountersRepository unreadCountersRepository;
    private readonly IIdentityProvider identityProvider;

    /// <inheritdoc />
    public PostReadingService(
        IRoomReadingService roomReadingService,
        IIntentionManager intentionManager,
        IPostReadingRepository repository,
        IUnreadCountersRepository unreadCountersRepository,
        IIdentityProvider identityProvider)
    {
        this.roomReadingService = roomReadingService;
        this.intentionManager = intentionManager;
        this.repository = repository;
        this.unreadCountersRepository = unreadCountersRepository;
        this.identityProvider = identityProvider;
    }

    /// <inheritdoc />
    public async Task<(IEnumerable<Post> posts, PagingResult paging)> Get(Guid roomId, PagingQuery query)
    {
        var room = await roomReadingService.Get(roomId);
        intentionManager.ThrowIfForbidden(RoomIntention.CreatePost, room);

        var identity = identityProvider.Current;
        var totalCount = await repository.Count(roomId, identity.User.UserId);
        var paging = new PagingData(query, identity.Settings.Paging.PostsPerPage, totalCount);

        var posts = await repository.Get(roomId, paging, identity.User.UserId);

        return (posts, paging.Result);
    }

    /// <inheritdoc />
    public async Task<Post> Get(Guid postId)
    {
        var post = await repository.Get(postId, identityProvider.Current.User.UserId);
        if (post == null)
        {
            throw new HttpException(HttpStatusCode.Gone, "Post not found");
        }

        return post;
    }

    /// <inheritdoc />
    public async Task MarkAsRead(Guid roomId)
    {
        await roomReadingService.Get(roomId);
        await unreadCountersRepository.Flush(identityProvider.Current.User.UserId,
            UnreadEntryType.Message, roomId);
    }
}