using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Gaming.BusinessProcesses.Likes;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Services.Gaming;

/// <inheritdoc />
internal class LikeApiService(
    ILikeService likeService,
    IMapper mapper) : ILikeApiService
{
    /// <inheritdoc />
    public async Task<Envelope<User>> LikeComment(Guid commentId, CancellationToken cancellationToken)
    {
        var user = await likeService.LikeComment(commentId, cancellationToken);
        return new Envelope<User>(mapper.Map<User>(user));
    }

    /// <inheritdoc />
    public Task DislikeComment(Guid commentId, CancellationToken cancellationToken) =>
        likeService.DislikeComment(commentId, cancellationToken);
}