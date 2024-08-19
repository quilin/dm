using System;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Commentaries.Reading;
using DM.Services.Gaming.BusinessProcesses.Commentaries.Updating;
using DM.Services.MessageQueuing.GeneralBus;
using Comment = DM.Services.DataAccess.BusinessObjects.Common.Comment;

namespace DM.Services.Gaming.BusinessProcesses.Commentaries.Deleting;

/// <inheritdoc />
internal class CommentaryDeletingService(
    IIntentionManager intentionManager,
    IUpdateBuilderFactory updateBuilderFactory,
    ICommentaryReadingService readingService,
    ICommentaryUpdatingRepository updatingRepository,
    IUnreadCountersRepository unreadCountersRepository,
    IInvokedEventProducer invokedEventProducer) : ICommentaryDeletingService
{
    /// <inheritdoc />
    public async Task Delete(Guid commentId, CancellationToken cancellationToken)
    {
        var comment = await readingService.Get(commentId, cancellationToken);
        intentionManager.ThrowIfForbidden(CommentIntention.Delete, comment);

        var updateComment = updateBuilderFactory.Create<Comment>(commentId)
            .Field(c => c.IsRemoved, true);
        await updatingRepository.Update(updateComment, cancellationToken);
        await unreadCountersRepository.Decrement(
            comment.EntityId, UnreadEntryType.Message, comment.CreateDate, cancellationToken);

        await invokedEventProducer.Send(EventType.DeletedGameComment, commentId);
    }
}