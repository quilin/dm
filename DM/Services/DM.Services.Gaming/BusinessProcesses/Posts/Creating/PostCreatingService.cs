using System.Linq;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Rooms.Reading;
using DM.Services.Gaming.Dto.Input;
using DM.Services.Gaming.Dto.Output;
using DM.Services.MessageQueuing.Publish;
using FluentValidation;
using PendingPost = DM.Services.DataAccess.BusinessObjects.Games.Links.PendingPost;

namespace DM.Services.Gaming.BusinessProcesses.Posts.Creating
{
    /// <inheritdoc />
    public class PostCreatingService : IPostCreatingService
    {
        private readonly IValidator<CreatePost> validator;
        private readonly IRoomReadingService roomReadingService;
        private readonly IIntentionManager intentionManager;
        private readonly IPostFactory postFactory;
        private readonly IUpdateBuilderFactory updateBuilderFactory;
        private readonly IPostCreatingRepository repository;
        private readonly IUnreadCountersRepository unreadCountersRepository;
        private readonly IInvokedEventPublisher publisher;
        private readonly IIdentity identity;

        /// <inheritdoc />
        public PostCreatingService(
            IValidator<CreatePost> validator,
            IRoomReadingService roomReadingService,
            IIntentionManager intentionManager,
            IPostFactory postFactory,
            IUpdateBuilderFactory updateBuilderFactory,
            IPostCreatingRepository repository,
            IUnreadCountersRepository unreadCountersRepository,
            IInvokedEventPublisher publisher,
            IIdentityProvider identityProvider)
        {
            this.validator = validator;
            this.roomReadingService = roomReadingService;
            this.intentionManager = intentionManager;
            this.postFactory = postFactory;
            this.updateBuilderFactory = updateBuilderFactory;
            this.repository = repository;
            this.unreadCountersRepository = unreadCountersRepository;
            this.publisher = publisher;
            identity = identityProvider.Current;
        }

        /// <inheritdoc />
        public async Task<Post> Create(CreatePost createPost)
        {
            await validator.ValidateAndThrowAsync(createPost);
            var room = await roomReadingService.Get(createPost.RoomId);
            await intentionManager.ThrowIfForbidden(RoomIntention.CreatePost, room, createPost);

            var pendingPostUpdates = room.Pendings
                .Where(p => p.PendingUser.UserId == identity.User.UserId)
                .Select(p => updateBuilderFactory.Create<PendingPost>(p.Id).Delete());

            var post = postFactory.Create(createPost, identity.User.UserId);

            var createdPost = await repository.Create(post, pendingPostUpdates);
            await unreadCountersRepository.Increment(createdPost.RoomId, UnreadEntryType.Message);
            await publisher.Publish(new [] {EventType.NewPost, EventType.RoomPendingResponded}, createdPost.Id);

            return createdPost;
        }
    }
}