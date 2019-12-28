using System;
using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.BusinessObjects.Games.Posts;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Posts.Reading;
using DM.Services.Gaming.BusinessProcesses.Posts.Updating;
using DM.Services.MessageQueuing.Publish;

namespace DM.Services.Gaming.BusinessProcesses.Posts.Deleting
{
    /// <inheritdoc />
    public class PostDeletingService : IPostDeletingService
    {
        private readonly IPostReadingService postReadingService;
        private readonly IIntentionManager intentionManager;
        private readonly IUpdateBuilderFactory updateBuilderFactory;
        private readonly IPostUpdatingRepository repository;
        private readonly IUnreadCountersRepository unreadCountersRepository;
        private readonly IInvokedEventPublisher publisher;

        /// <inheritdoc />
        public PostDeletingService(
            IPostReadingService postReadingService,
            IIntentionManager intentionManager,
            IUpdateBuilderFactory updateBuilderFactory,
            IPostUpdatingRepository repository,
            IUnreadCountersRepository unreadCountersRepository,
            IInvokedEventPublisher publisher)
        {
            this.postReadingService = postReadingService;
            this.intentionManager = intentionManager;
            this.updateBuilderFactory = updateBuilderFactory;
            this.repository = repository;
            this.unreadCountersRepository = unreadCountersRepository;
            this.publisher = publisher;
        }
        
        /// <inheritdoc />
        public async Task Delete(Guid postId)
        {
            var post = await postReadingService.Get(postId);
            intentionManager.ThrowIfForbidden(PostIntention.Delete, post);
            var updateBuilder = updateBuilderFactory.Create<Post>(postId)
                .Field(p => p.IsRemoved, true);

            await repository.Update(updateBuilder);
            await unreadCountersRepository.Decrement(post.RoomId, UnreadEntryType.Message, post.CreateDate);
            await publisher.Publish(EventType.DeletedPost, postId);
        }
    }
}