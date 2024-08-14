using System;
using DM.Services.Common.Dto;
using DM.Services.Core.Implementation;
using Comment = DM.Services.DataAccess.BusinessObjects.Common.Comment;

namespace DM.Services.Common.BusinessProcesses.Commentaries;

/// <inheritdoc />
internal class CommentaryFactory : ICommentaryFactory
{
    private readonly IGuidFactory guidFactory;
    private readonly IDateTimeProvider dateTimeProvider;

    /// <inheritdoc />
    public CommentaryFactory(
        IGuidFactory guidFactory,
        IDateTimeProvider dateTimeProvider)
    {
        this.guidFactory = guidFactory;
        this.dateTimeProvider = dateTimeProvider;
    }
        
    /// <inheritdoc />
    public Comment Create(CreateComment createComment, Guid userId)
    {
        return new Comment
        {
            CommentId = guidFactory.Create(),
            EntityId = createComment.EntityId,
            UserId = userId,
            CreateDate = dateTimeProvider.Now,
            Text = createComment.Text.Trim(),
            IsRemoved = false
        };
    }
}