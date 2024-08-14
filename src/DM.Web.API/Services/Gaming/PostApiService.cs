using System;
using System.Linq;
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
internal class PostApiService : IPostApiService
{
    private readonly IPostReadingService readingService;
    private readonly IPostCreatingService creatingService;
    private readonly IPostUpdatingService updatingService;
    private readonly IPostDeletingService deletingService;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public PostApiService(
        IPostReadingService readingService,
        IPostCreatingService creatingService,
        IPostUpdatingService updatingService,
        IPostDeletingService deletingService,
        IMapper mapper)
    {
        this.readingService = readingService;
        this.creatingService = creatingService;
        this.updatingService = updatingService;
        this.deletingService = deletingService;
        this.mapper = mapper;
    }
        
    /// <inheritdoc />
    public async Task<ListEnvelope<Post>> Get(Guid roomId, PagingQuery query)
    {
        var (posts, paging) = await readingService.Get(roomId, query);
        return new ListEnvelope<Post>(posts.Select(mapper.Map<Post>), new Paging(paging));
    }

    /// <inheritdoc />
    public async Task<Envelope<Post>> Get(Guid postId)
    {
        var post = await readingService.Get(postId);
        return new Envelope<Post>(mapper.Map<Post>(post));
    }

    /// <inheritdoc />
    public async Task<Envelope<Post>> Create(Guid roomId, Post post)
    {
        var createPost = mapper.Map<CreatePost>(post);
        createPost.RoomId = roomId;
        var createdPost = await creatingService.Create(createPost);
        return new Envelope<Post>(mapper.Map<Post>(createdPost));
    }

    /// <inheritdoc />
    public async Task<Envelope<Post>> Update(Guid postId, Post post)
    {
        var updatePost = mapper.Map<UpdatePost>(post);
        updatePost.PostId = postId;
        var updatedPost = await updatingService.Update(updatePost);
        return new Envelope<Post>(mapper.Map<Post>(updatedPost));
    }

    /// <inheritdoc />
    public Task Delete(Guid postId) => deletingService.Delete(postId);

    /// <inheritdoc />
    public Task MarkAsRead(Guid roomId) => readingService.MarkAsRead(roomId);
}