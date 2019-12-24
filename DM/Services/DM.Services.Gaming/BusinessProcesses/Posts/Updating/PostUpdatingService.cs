using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Rooms.Updating;
using DM.Services.Gaming.Dto.Input;
using DM.Services.Gaming.Dto.Output;
using DM.Services.MessageQueuing.Publish;
using FluentValidation;
using DbPost = DM.Services.DataAccess.BusinessObjects.Games.Posts.Post;

namespace DM.Services.Gaming.BusinessProcesses.Posts.Updating
{
    /// <inheritdoc />
    public class PostUpdatingService : IPostUpdatingService
    {
        private readonly IValidator<UpdatePost> validator;
        private readonly IIntentionManager intentionManager;
        private readonly IUpdateBuilderFactory updateBuilderFactory;
        private readonly IRoomUpdatingRepository roomUpdatingRepository;
        private readonly IPostUpdatingRepository repository;
        private readonly IInvokedEventPublisher publisher;
        private readonly IIdentity identity;

        /// <inheritdoc />
        public PostUpdatingService(
            IValidator<UpdatePost> validator,
            IIntentionManager intentionManager,
            IUpdateBuilderFactory updateBuilderFactory,
            IRoomUpdatingRepository roomUpdatingRepository,
            IPostUpdatingRepository repository,
            IInvokedEventPublisher publisher,
            IIdentityProvider identityProvider)
        {
            this.validator = validator;
            this.intentionManager = intentionManager;
            this.updateBuilderFactory = updateBuilderFactory;
            this.roomUpdatingRepository = roomUpdatingRepository;
            this.repository = repository;
            this.publisher = publisher;
            identity = identityProvider.Current;
        }

        /// <inheritdoc />
        public async Task<Post> Update(UpdatePost updatePost)
        {
            await validator.ValidateAndThrowAsync(updatePost);
            var postToUpdate = await repository.Get(updatePost.PostId);
            var room = await roomUpdatingRepository.GetRoom(postToUpdate.RoomId, identity.User.UserId);
            await intentionManager.ThrowIfForbidden(PostIntention.Edit, postToUpdate, (room, updatePost));

            var updateBuilder = updateBuilderFactory.Create<DbPost>(updatePost.PostId)
                .MaybeField(p => p.Text, updatePost.Text)
                .MaybeField(p => p.Commentary, updatePost.Commentary)
                .MaybeField(p => p.MasterMessage, updatePost.MasterMessage);

            if (updatePost.CharacterId != default && updatePost.CharacterId.Value != postToUpdate.CharacterId &&
                await intentionManager.IsAllowed(PostIntention.EditCharacter, postToUpdate, updatePost))
            {
                updateBuilder.MaybeField(p => p.CharacterId, updatePost.CharacterId);
            }

            var updatedPost = await repository.Update(updateBuilder);
            await publisher.Publish(EventType.ChangedPost, postToUpdate.Id);

            return updatedPost;
        }
    }
}