using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Exceptions;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Rooms.Updating;
using DM.Services.Gaming.Dto.Input;
using DM.Services.Gaming.Dto.Output;
using DM.Services.MessageQueuing.GeneralBus;
using FluentValidation;
using PendingPost = DM.Services.DataAccess.BusinessObjects.Games.Links.PendingPost;

namespace DM.Services.Gaming.BusinessProcesses.Posts.Creating;

/// <inheritdoc />
internal class PostCreatingService : IPostCreatingService
{
    private readonly IValidator<CreatePost> validator;
    private readonly IRoomUpdatingRepository roomUpdatingRepository;
    private readonly IIntentionManager intentionManager;
    private readonly IPostFactory postFactory;
    private readonly IUpdateBuilderFactory updateBuilderFactory;
    private readonly IPostCreatingRepository repository;
    private readonly IUnreadCountersRepository unreadCountersRepository;
    private readonly IInvokedEventProducer producer;
    private readonly IIdentityProvider identityProvider;

    /// <inheritdoc />
    public PostCreatingService(
        IValidator<CreatePost> validator,
        IRoomUpdatingRepository roomUpdatingRepository,
        IIntentionManager intentionManager,
        IPostFactory postFactory,
        IUpdateBuilderFactory updateBuilderFactory,
        IPostCreatingRepository repository,
        IUnreadCountersRepository unreadCountersRepository,
        IInvokedEventProducer producer,
        IIdentityProvider identityProvider)
    {
        this.validator = validator;
        this.roomUpdatingRepository = roomUpdatingRepository;
        this.intentionManager = intentionManager;
        this.postFactory = postFactory;
        this.updateBuilderFactory = updateBuilderFactory;
        this.repository = repository;
        this.unreadCountersRepository = unreadCountersRepository;
        this.producer = producer;
        this.identityProvider = identityProvider;
    }

    /// <inheritdoc />
    public async Task<Post> Create(CreatePost createPost)
    {
        await validator.ValidateAndThrowAsync(createPost);
        var identity = identityProvider.Current;
        var room = await roomUpdatingRepository.GetRoom(createPost.RoomId, identity.User.UserId);
        if (room == null)
        {
            throw new HttpException(HttpStatusCode.Gone, "Room not found");
        }

        intentionManager.ThrowIfForbidden(RoomIntention.CreatePost, (room, createPost.CharacterId));

        var events = new List<EventType>(2) {EventType.NewPost};

        var pendingPostUpdates = room.Pendings
            .Where(p => p.PendingUser.UserId == identity.User.UserId)
            .Select(p => updateBuilderFactory.Create<PendingPost>(p.Id).Delete())
            .ToArray();
        if (pendingPostUpdates.Any())
        {
            events.Add(EventType.RoomPendingResponded);
        }

        var post = postFactory.Create(createPost, identity.User.UserId);

        var createdPost = await repository.Create(post, pendingPostUpdates);
        await unreadCountersRepository.Increment(createdPost.RoomId, UnreadEntryType.Message);
        await producer.Send(events, createdPost.Id);

        return createdPost;
    }
}