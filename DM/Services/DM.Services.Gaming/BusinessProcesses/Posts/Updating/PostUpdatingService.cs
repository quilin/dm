using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Authorization;
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
        private readonly IPostUpdatingRepository repository;
        private readonly IInvokedEventPublisher publisher;

        /// <inheritdoc />
        public PostUpdatingService(
            IValidator<UpdatePost> validator,
            IIntentionManager intentionManager,
            IUpdateBuilderFactory updateBuilderFactory,
            IPostUpdatingRepository repository,
            IInvokedEventPublisher publisher)
        {
            this.validator = validator;
            this.intentionManager = intentionManager;
            this.updateBuilderFactory = updateBuilderFactory;
            this.repository = repository;
            this.publisher = publisher;
        }

        /// <inheritdoc />
        public async Task<Post> Update(UpdatePost updatePost)
        {
            await validator.ValidateAndThrowAsync(updatePost);
            var postToUpdate = await repository.Get(updatePost.PostId);
            await intentionManager.ThrowIfForbidden(PostIntention.Edit, postToUpdate);

            var updateBuilder = updateBuilderFactory.Create<DbPost>(updatePost.PostId)
                .MaybeField(p => p.Text, updatePost.Text)
                .MaybeField(p => p.Commentary, updatePost.Commentary)
                .MaybeField(p => p.MasterMessage, updatePost.MasterMessage);

            if (await intentionManager.IsAllowed(PostIntention.EditCharacter, postToUpdate, updatePost))
            {
                updateBuilder.MaybeField(p => p.CharacterId, updatePost.CharacterId);
            }

            var updatedPost = await repository.Update(updateBuilder);
            await publisher.Publish(EventType.ChangedPost, postToUpdate.Id);

            return updatedPost;
        }
    }
}