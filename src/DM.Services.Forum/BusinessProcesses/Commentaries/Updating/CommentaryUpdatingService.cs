using System.Threading;
using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Common.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Forum.Authorization;
using DM.Services.Forum.BusinessProcesses.Commentaries.Reading;
using DM.Services.MessageQueuing.GeneralBus;
using FluentValidation;
using Comment = DM.Services.DataAccess.BusinessObjects.Common.Comment;

namespace DM.Services.Forum.BusinessProcesses.Commentaries.Updating;

/// <inheritdoc />
internal class CommentaryUpdatingService(
    IValidator<UpdateComment> validator,
    ICommentaryReadingService commentaryReadingService,
    IIntentionManager intentionManager,
    IDateTimeProvider dateTimeProvider,
    IUpdateBuilderFactory updateBuilderFactory,
    ICommentaryUpdatingRepository repository,
    IInvokedEventProducer invokedEventProducer) : ICommentaryUpdatingService
{
    /// <inheritdoc />
    public async Task<Services.Common.Dto.Comment> Update(
        UpdateComment updateComment, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(updateComment, cancellationToken);
        var comment = await commentaryReadingService.Get(updateComment.CommentId, cancellationToken);

        intentionManager.ThrowIfForbidden(CommentIntention.Edit, comment);
        var updateBuilder = updateBuilderFactory.Create<Comment>(updateComment.CommentId)
            .MaybeField(f => f.Text, updateComment.Text?.Trim());

        if (updateBuilder.HasChanges())
        {
            updateBuilder.Field(f => f.LastUpdateDate, dateTimeProvider.Now);
        }

        var updatedComment = await repository.Update(updateBuilder, cancellationToken);
        await invokedEventProducer.Send(EventType.ChangedForumComment, updateComment.CommentId);
        return updatedComment;
    }
}