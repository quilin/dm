using System;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Gaming.BusinessProcesses.Likes;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Services.Gaming;

/// <inheritdoc />
internal class LikeApiService : ILikeApiService
{
    private readonly ILikeService likeService;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public LikeApiService(
        ILikeService likeService,
        IMapper mapper)
    {
        this.likeService = likeService;
        this.mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<Envelope<User>> LikeComment(Guid commentId)
    {
        var user = await likeService.LikeComment(commentId);
        return new Envelope<User>(mapper.Map<User>(user));
    }

    /// <inheritdoc />
    public Task DislikeComment(Guid commentId) => likeService.DislikeComment(commentId);
}