using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Posts.Reading;
using DM.Services.Gaming.BusinessProcesses.Rooms.Updating;
using DM.Services.Gaming.Dto.Input;
using DM.Services.Gaming.Dto.Output;
using DM.Services.MessageQueuing.GeneralBus;
using FluentValidation;
using DbPost = DM.Services.DataAccess.BusinessObjects.Games.Posts.Post;

namespace DM.Services.Gaming.BusinessProcesses.Posts.Updating;

/// <inheritdoc />
internal class PostUpdatingService : IPostUpdatingService
{
    private readonly IValidator<UpdatePost> validator;
    private readonly IIntentionManager intentionManager;
    private readonly IUpdateBuilderFactory updateBuilderFactory;
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly IPostReadingService postReadingService;
    private readonly IRoomUpdatingRepository roomUpdatingRepository;
    private readonly IPostUpdatingRepository repository;
    private readonly IInvokedEventProducer producer;
    private readonly IIdentityProvider identityProvider;

    /// <inheritdoc />
    public PostUpdatingService(
        IValidator<UpdatePost> validator,
        IIntentionManager intentionManager,
        IUpdateBuilderFactory updateBuilderFactory,
        IDateTimeProvider dateTimeProvider,
        IPostReadingService postReadingService,
        IRoomUpdatingRepository roomUpdatingRepository,
        IPostUpdatingRepository repository,
        IInvokedEventProducer producer,
        IIdentityProvider identityProvider)
    {
        this.validator = validator;
        this.intentionManager = intentionManager;
        this.updateBuilderFactory = updateBuilderFactory;
        this.dateTimeProvider = dateTimeProvider;
        this.postReadingService = postReadingService;
        this.roomUpdatingRepository = roomUpdatingRepository;
        this.repository = repository;
        this.producer = producer;
        this.identityProvider = identityProvider;
    }

    /// <inheritdoc />
    public async Task<Post> Update(UpdatePost updatePost)
    {
        await validator.ValidateAndThrowAsync(updatePost);
        var post = await postReadingService.Get(updatePost.PostId);
        var currentUserId = identityProvider.Current.User.UserId;
        var room = await roomUpdatingRepository.GetRoom(post.RoomId, currentUserId);

        var updateBuilder = updateBuilderFactory.Create<DbPost>(updatePost.PostId);

        if (intentionManager.IsAllowed(PostIntention.EditText, (post, room)))
        {
            updateBuilder
                .MaybeField(p => p.Text, updatePost.Text)
                .MaybeField(p => p.Commentary, updatePost.Commentary);

            if (intentionManager.IsAllowed(PostIntention.EditMasterMessage, (post, room)))
            {
                updateBuilder.MaybeField(p => p.MasterMessage, updatePost.MasterMessage);
            }
        }

        if (updatePost.CharacterId.HasChanged(post.Character?.Id) &&
            intentionManager.IsAllowed(RoomIntention.CreatePost, (room, updatePost.CharacterId.Value)) &&
            intentionManager.IsAllowed(PostIntention.EditCharacter, (post, room)))
        {
            updateBuilder.MaybeField(p => p.CharacterId, updatePost.CharacterId);
        }

        if (updateBuilder.HasChanges())
        {
            updateBuilder
                .Field(p => p.LastUpdateDate, dateTimeProvider.Now)
                .Field(p => p.LastUpdateUserId, currentUserId);
        }

        var updatedPost = await repository.Update(updateBuilder);
        await producer.Send(EventType.ChangedPost, post.Id);

        return updatedPost;
    }
}