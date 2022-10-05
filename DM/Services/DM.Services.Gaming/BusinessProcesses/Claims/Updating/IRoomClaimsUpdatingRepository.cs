using System.Threading.Tasks;
using DM.Services.DataAccess.RelationalStorage;
using RoomClaim = DM.Services.DataAccess.BusinessObjects.Games.Links.RoomClaim;

namespace DM.Services.Gaming.BusinessProcesses.Claims.Updating;

/// <summary>
/// Storage for room claims updating
/// </summary>
internal interface IRoomClaimsUpdatingRepository
{
    /// <summary>
    /// Update room claim
    /// </summary>
    /// <param name="updateClaim">Update rules</param>
    /// <returns></returns>
    Task<Dto.Output.RoomClaim> Update(IUpdateBuilder<RoomClaim> updateClaim);
}