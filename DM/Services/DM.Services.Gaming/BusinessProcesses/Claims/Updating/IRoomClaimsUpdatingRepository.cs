using System.Threading.Tasks;
using DM.Services.DataAccess.BusinessObjects.Games.Links;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.BusinessProcesses.Claims.Updating
{
    /// <summary>
    /// Storage for room claims updating
    /// </summary>
    public interface IRoomClaimsUpdatingRepository
    {
        /// <summary>
        /// Update room claim
        /// </summary>
        /// <param name="updateClaim">Update rules</param>
        /// <returns></returns>
        Task<RoomClaim> Update(IUpdateBuilder<ParticipantRoomLink> updateClaim);
    }
}