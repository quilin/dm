using System;
using System.Threading;
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
using DM.Services.MessageQueuing.GeneralBus;

namespace DM.Services.Gaming.BusinessProcesses.Posts.Deleting;

/// <inheritdoc />
internal class PostDeletingService(
    IPostReadingService postReadingService,
    IIntentionManager intentionManager,
    IUpdateBuilderFactory updateBuilderFactory,
    IPostUpdatingRepository repository,
    IUnreadCountersRepository unreadCountersRepository,
    IInvokedEventProducer producer) : IPostDeletingService
{
    /// <inheritdoc />
    public async Task Delete(Guid postId, CancellationToken cancellationToken)
    {
        var post = await postReadingService.Get(postId, cancellationToken);
        intentionManager.ThrowIfForbidden(PostIntention.Delete, post);
        var updateBuilder = updateBuilderFactory.Create<Post>(postId)
            .Field(p => p.IsRemoved, true);

        await repository.Update(updateBuilder,cancellationToken);
        await unreadCountersRepository.Decrement(
            post.RoomId, UnreadEntryType.Message, post.CreateDate, cancellationToken);
        await producer.Send(EventType.DeletedPost, postId);
    }
}