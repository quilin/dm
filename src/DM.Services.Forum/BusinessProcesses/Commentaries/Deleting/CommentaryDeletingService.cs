using System;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Forum.Authorization;
using DM.Services.MessageQueuing.GeneralBus;
using Comment = DM.Services.DataAccess.BusinessObjects.Common.Comment;

namespace DM.Services.Forum.BusinessProcesses.Commentaries.Deleting;

/// <inheritdoc />
internal class CommentaryDeletingService(
    IIntentionManager intentionManager,
    IUpdateBuilderFactory updateBuilderFactory,
    ICommentaryDeletingRepository repository,
    IUnreadCountersRepository unreadCountersRepository,
    IInvokedEventProducer invokedEventProducer) : ICommentaryDeletingService
{
    /// <inheritdoc />
    public async Task Delete(Guid commentId, CancellationToken cancellationToken)
    {
        var comment = await repository.GetForDelete(commentId, cancellationToken);
        intentionManager.ThrowIfForbidden(CommentIntention.Delete, (Services.Common.Dto.Comment) comment);

        var updateTopic = updateBuilderFactory.Create<ForumTopic>(comment.EntityId);
        if (comment.IsLastCommentOfTopic)
        {
            var previousCommentaryId = await repository.GetSecondLastCommentId(comment.EntityId, cancellationToken);
            updateTopic = updateTopic.Field(t => t.LastCommentId, previousCommentaryId);
        }

        var updateComment = updateBuilderFactory.Create<Comment>(commentId)
            .Field(c => c.IsRemoved, true);
        await repository.Delete(updateComment, updateTopic, cancellationToken);
        await unreadCountersRepository.Decrement(
            comment.EntityId, UnreadEntryType.Message, comment.CreateDate, cancellationToken);

        await invokedEventProducer.Send(EventType.DeletedForumComment, commentId);
    }
}