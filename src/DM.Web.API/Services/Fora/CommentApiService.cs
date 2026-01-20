using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Common.Dto;
using DM.Services.Core.Dto;
using DM.Services.Forum.BusinessProcesses.Commentaries.Creating;
using DM.Services.Forum.BusinessProcesses.Commentaries.Deleting;
using DM.Services.Forum.BusinessProcesses.Commentaries.Reading;
using DM.Services.Forum.BusinessProcesses.Commentaries.Updating;
using DM.Web.API.Dto.Contracts;
using Comment = DM.Web.API.Dto.Shared.Comment;

namespace DM.Web.API.Services.Fora;

/// <inheritdoc />
internal class CommentApiService(
    ICommentaryReadingService readingService,
    ICommentaryCreatingService creatingService,
    ICommentaryUpdatingService updatingService,
    ICommentaryDeletingService deletingService,
    IMapper mapper) : ICommentApiService
{
    /// <inheritdoc />
    public async Task<ListEnvelope<Comment>> Get(
        Guid topicId, PagingQuery query, CancellationToken cancellationToken)
    {
        var (comments, paging) = await readingService.Get(topicId, query, cancellationToken);
        return new ListEnvelope<Comment>(comments.Select(mapper.Map<Comment>), new Paging(paging));
    }

    /// <inheritdoc />
    public async Task<Envelope<Comment>> Create(
        Guid topicId, Comment comment, CancellationToken cancellationToken)
    {
        var createComment = mapper.Map<CreateComment>(comment);
        createComment.EntityId = topicId;
        var createdComment = await creatingService.Create(createComment, cancellationToken);
        return new Envelope<Comment>(mapper.Map<Comment>(createdComment));
    }

    /// <inheritdoc />
    public async Task<Envelope<Comment>> Get(Guid commentId, CancellationToken cancellationToken)
    {
        var comment = await readingService.Get(commentId, cancellationToken);
        return new Envelope<Comment>(mapper.Map<Comment>(comment));
    }

    /// <inheritdoc />
    public async Task<Envelope<Comment>> Update(
        Guid commentId, Comment comment, CancellationToken cancellationToken)
    {
        var updateComment = mapper.Map<UpdateComment>(comment);
        updateComment.CommentId = commentId;
        var updatedComment = await updatingService.Update(updateComment, cancellationToken);
        return new Envelope<Comment>(mapper.Map<Comment>(updatedComment));
    }

    /// <inheritdoc />
    public Task Delete(Guid commentId, CancellationToken cancellationToken) =>
        deletingService.Delete(commentId, cancellationToken);

    /// <inheritdoc />
    public Task MarkAsRead(Guid topicId, CancellationToken cancellationToken) =>
        readingService.MarkAsRead(topicId, cancellationToken);

    /// <inheritdoc />
    public Task MarkAsRead(string forumId, CancellationToken cancellationToken) =>
        readingService.MarkAsRead(forumId, cancellationToken);
}