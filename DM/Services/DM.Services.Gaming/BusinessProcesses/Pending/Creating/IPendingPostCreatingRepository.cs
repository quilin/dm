using System.Threading.Tasks;
using DM.Services.Gaming.Dto.Output;
using DbPendingPost = DM.Services.DataAccess.BusinessObjects.Games.Links.PendingPost;

namespace DM.Services.Gaming.BusinessProcesses.Pending.Creating;

/// <summary>
/// Storage for pending post creating
/// </summary>
internal interface IPendingPostCreatingRepository
{
    /// <summary>
    /// Save new pending post
    /// </summary>
    /// <param name="pendingPost">DAL model</param>
    /// <returns></returns>
    Task<PendingPost> Create(DbPendingPost pendingPost);
}