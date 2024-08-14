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
internal class CommentaryUpdatingService : ICommentaryUpdatingService
{
    private readonly IValidator<UpdateComment> validator;
    private readonly ICommentaryReadingService commentaryReadingService;
    private readonly IIntentionManager intentionManager;
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly IUpdateBuilderFactory updateBuilderFactory;
    private readonly ICommentaryUpdatingRepository repository;
    private readonly IInvokedEventProducer invokedEventProducer;

    /// <inheritdoc />
    public CommentaryUpdatingService(
        IValidator<UpdateComment> validator,
        ICommentaryReadingService commentaryReadingService,
        IIntentionManager intentionManager,
        IDateTimeProvider dateTimeProvider,
        IUpdateBuilderFactory updateBuilderFactory,
        ICommentaryUpdatingRepository repository,
        IInvokedEventProducer invokedEventProducer)
    {
        this.validator = validator;
        this.commentaryReadingService = commentaryReadingService;
        this.intentionManager = intentionManager;
        this.dateTimeProvider = dateTimeProvider;
        this.updateBuilderFactory = updateBuilderFactory;
        this.repository = repository;
        this.invokedEventProducer = invokedEventProducer;
    }

    /// <inheritdoc />
    public async Task<Services.Common.Dto.Comment> Update(UpdateComment updateComment)
    {
        await validator.ValidateAndThrowAsync(updateComment);
        var comment = await commentaryReadingService.Get(updateComment.CommentId);

        intentionManager.ThrowIfForbidden(CommentIntention.Edit, comment);
        var updateBuilder = updateBuilderFactory.Create<Comment>(updateComment.CommentId)
            .MaybeField(f => f.Text, updateComment.Text?.Trim());

        if (updateBuilder.HasChanges())
        {
            updateBuilder.Field(f => f.LastUpdateDate, dateTimeProvider.Now);
        }

        var updatedComment = await repository.Update(updateBuilder);
        await invokedEventProducer.Send(EventType.ChangedForumComment, updateComment.CommentId);
        return updatedComment;
    }
}