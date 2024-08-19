using System;
using System.Threading;
using System.Threading.Tasks;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Games;

namespace DM.Web.API.Services.Gaming.Rooms;

/// <summary>
/// API service for pending post
/// </summary>
public interface IPendingPostApiService
{
    /// <summary>
    /// Create new pending post
    /// </summary>
    /// <param name="roomId">Room identifier</param>
    /// <param name="pendingPost">API DTO model</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Envelope<PendingPost>> Create(Guid roomId, PendingPost pendingPost,
        CancellationToken cancellationToken);

    /// <summary>
    /// Delete existing pending post
    /// </summary>
    /// <param name="pendingPostId">Pending post identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task Delete(Guid pendingPostId, CancellationToken cancellationToken);
}