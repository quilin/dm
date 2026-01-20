using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Forum.BusinessProcesses.Likes;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Services.Fora;

/// <inheritdoc />
internal class LikeApiService(
    ILikeService likeService,
    IMapper mapper) : ILikeApiService
{
    /// <inheritdoc />
    public async Task<Envelope<User>> LikeTopic(Guid topicId, CancellationToken cancellationToken)
    {
        var likedByUser = await likeService.LikeTopic(topicId, cancellationToken);
        return new Envelope<User>(mapper.Map<User>(likedByUser));
    }

    /// <inheritdoc />
    public Task DislikeTopic(Guid topicId, CancellationToken cancellationToken) =>
        likeService.DislikeTopic(topicId, cancellationToken);

    /// <inheritdoc />
    public async Task<Envelope<User>> LikeComment(Guid commentId, CancellationToken cancellationToken)
    {
        var likedByUser = await likeService.LikeComment(commentId, cancellationToken);
        return new Envelope<User>(mapper.Map<User>(likedByUser));
    }

    /// <inheritdoc />
    public Task DislikeComment(Guid commentId, CancellationToken cancellationToken) =>
        likeService.DislikeComment(commentId, cancellationToken);
}