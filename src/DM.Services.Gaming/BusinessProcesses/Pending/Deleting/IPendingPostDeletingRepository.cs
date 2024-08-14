using System;
using System.Threading.Tasks;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Dto.Output;
using DbPendingPost = DM.Services.DataAccess.BusinessObjects.Games.Links.PendingPost;

namespace DM.Services.Gaming.BusinessProcesses.Pending.Deleting;

/// <summary>
/// Storage for pending post deleting
/// </summary>
internal interface IPendingPostDeletingRepository
{
    /// <summary>
    /// Get pending post
    /// </summary>
    /// <param name="pendingPostId">Identifier</param>
    /// <returns></returns>
    Task<PendingPost> Get(Guid pendingPostId);

    /// <summary>
    /// Delete pending post
    /// </summary>
    /// <param name="updateBuilder"></param>
    /// <returns></returns>
    Task Delete(IUpdateBuilder<DbPendingPost> updateBuilder);
}