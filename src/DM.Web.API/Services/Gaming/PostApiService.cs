using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Core.Dto;
using DM.Services.Gaming.BusinessProcesses.Posts.Creating;
using DM.Services.Gaming.BusinessProcesses.Posts.Deleting;
using DM.Services.Gaming.BusinessProcesses.Posts.Reading;
using DM.Services.Gaming.BusinessProcesses.Posts.Updating;
using DM.Services.Gaming.Dto.Input;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Games;

namespace DM.Web.API.Services.Gaming;

/// <inheritdoc />
internal class PostApiService(
    IPostReadingService readingService,
    IPostCreatingService creatingService,
    IPostUpdatingService updatingService,
    IPostDeletingService deletingService,
    IMapper mapper) : IPostApiService
{
    /// <inheritdoc />
    public async Task<ListEnvelope<Post>> Get(
        Guid roomId, PagingQuery query, CancellationToken cancellationToken)
    {
        var (posts, paging) = await readingService.Get(roomId, query, cancellationToken);
        return new ListEnvelope<Post>(posts.Select(mapper.Map<Post>), new Paging(paging));
    }

    /// <inheritdoc />
    public async Task<Envelope<Post>> Get(Guid postId, CancellationToken cancellationToken)
    {
        var post = await readingService.Get(postId, cancellationToken);
        return new Envelope<Post>(mapper.Map<Post>(post));
    }

    /// <inheritdoc />
    public async Task<Envelope<Post>> Create(Guid roomId, Post post, CancellationToken cancellationToken)
    {
        var createPost = mapper.Map<CreatePost>(post);
        createPost.RoomId = roomId;
        var createdPost = await creatingService.Create(createPost, cancellationToken);
        return new Envelope<Post>(mapper.Map<Post>(createdPost));
    }

    /// <inheritdoc />
    public async Task<Envelope<Post>> Update(Guid postId, Post post, CancellationToken cancellationToken)
    {
        var updatePost = mapper.Map<UpdatePost>(post);
        updatePost.PostId = postId;
        var updatedPost = await updatingService.Update(updatePost, cancellationToken);
        return new Envelope<Post>(mapper.Map<Post>(updatedPost));
    }

    /// <inheritdoc />
    public Task Delete(Guid postId, CancellationToken cancellationToken) =>
        deletingService.Delete(postId, cancellationToken);

    /// <inheritdoc />
    public Task MarkAsRead(Guid roomId, CancellationToken cancellationToken) =>
        readingService.MarkAsRead(roomId, cancellationToken);
}