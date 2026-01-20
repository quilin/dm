using System.Threading;
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
internal class PostUpdatingService(
    IValidator<UpdatePost> validator,
    IIntentionManager intentionManager,
    IUpdateBuilderFactory updateBuilderFactory,
    IDateTimeProvider dateTimeProvider,
    IPostReadingService postReadingService,
    IRoomUpdatingRepository roomUpdatingRepository,
    IPostUpdatingRepository repository,
    IInvokedEventProducer producer,
    IIdentityProvider identityProvider) : IPostUpdatingService
{
    /// <inheritdoc />
    public async Task<Post> Update(UpdatePost updatePost, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(updatePost, cancellationToken);
        var post = await postReadingService.Get(updatePost.PostId, cancellationToken);
        var currentUserId = identityProvider.Current.User.UserId;
        var room = await roomUpdatingRepository.GetRoom(post.RoomId, currentUserId, cancellationToken);

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

        var updatedPost = await repository.Update(updateBuilder, cancellationToken);
        await producer.Send(EventType.ChangedPost, post.Id);

        return updatedPost;
    }
}