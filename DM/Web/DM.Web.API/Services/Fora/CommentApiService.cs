using System;
using System.Linq;
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
internal class CommentApiService : ICommentApiService
{
    private readonly ICommentaryReadingService readingService;
    private readonly ICommentaryCreatingService creatingService;
    private readonly ICommentaryUpdatingService updatingService;
    private readonly ICommentaryDeletingService deletingService;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public CommentApiService(
        ICommentaryReadingService readingService,
        ICommentaryCreatingService creatingService,
        ICommentaryUpdatingService updatingService,
        ICommentaryDeletingService deletingService,
        IMapper mapper)
    {
        this.readingService = readingService;
        this.creatingService = creatingService;
        this.updatingService = updatingService;
        this.deletingService = deletingService;
        this.mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<ListEnvelope<Comment>> Get(Guid topicId, PagingQuery query)
    {
        var (comments, paging) = await readingService.Get(topicId, query);
        return new ListEnvelope<Comment>(comments.Select(mapper.Map<Comment>), new Paging(paging));
    }

    /// <inheritdoc />
    public async Task<Envelope<Comment>> Create(Guid topicId, Comment comment)
    {
        var createComment = mapper.Map<CreateComment>(comment);
        createComment.EntityId = topicId;
        var createdComment = await creatingService.Create(createComment);
        return new Envelope<Comment>(mapper.Map<Comment>(createdComment));
    }

    /// <inheritdoc />
    public async Task<Envelope<Comment>> Get(Guid commentId)
    {
        var comment = await readingService.Get(commentId);
        return new Envelope<Comment>(mapper.Map<Comment>(comment));
    }

    /// <inheritdoc />
    public async Task<Envelope<Comment>> Update(Guid commentId, Comment comment)
    {
        var updateComment = mapper.Map<UpdateComment>(comment);
        updateComment.CommentId = commentId;
        var updatedComment = await updatingService.Update(updateComment);
        return new Envelope<Comment>(mapper.Map<Comment>(updatedComment));
    }

    /// <inheritdoc />
    public Task Delete(Guid commentId) => deletingService.Delete(commentId);

    /// <inheritdoc />
    public Task MarkAsRead(Guid topicId) => readingService.MarkAsRead(topicId);

    /// <inheritdoc />
    public Task MarkAsRead(string forumId) => readingService.MarkAsRead(forumId);
}