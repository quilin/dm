using System;
using DM.Services.DataAccess.BusinessObjects.Games.Links;
using DM.Services.Gaming.Dto.Input;

namespace DM.Services.Gaming.BusinessProcesses.Pending.Creating;

/// <summary>
/// Factory for pending post DAL model
/// </summary>
internal interface IPendingPostFactory
{
    /// <summary>
    /// Create new
    /// </summary>
    /// <param name="createPendingPost">DTO model</param>
    /// <param name="awaitingUserId">Current user identifier</param>
    /// <param name="pendingUserId">User who needs to write a post</param>
    /// <returns></returns>
    PendingPost Create(CreatePendingPost createPendingPost, Guid awaitingUserId, Guid pendingUserId);
}