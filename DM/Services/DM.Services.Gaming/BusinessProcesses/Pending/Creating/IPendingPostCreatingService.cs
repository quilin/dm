using System.Threading.Tasks;
using DM.Services.Gaming.Dto.Input;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.BusinessProcesses.Pending.Creating;

/// <summary>
/// Service for creating pending posts
/// </summary>
public interface IPendingPostCreatingService
{
    /// <summary>
    /// Create new pending
    /// </summary>
    /// <param name="createPendingPost">DTO model</param>
    /// <returns></returns>
    Task<PendingPost> Create(CreatePendingPost createPendingPost);
}