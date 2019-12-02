using System.Threading.Tasks;
using DM.Services.DataAccess.BusinessObjects.Games.Links;
using DM.Services.DataAccess.RelationalStorage;

namespace DM.Services.Gaming.BusinessProcesses.Claims.Deleting
{
    /// <summary>
    /// Storage for room claims deleting
    /// </summary>
    public interface IRoomClaimsDeletingRepository
    {
        /// <summary>
        /// Delete existing link
        /// </summary>
        /// <param name="deleteLink"></param>
        /// <returns></returns>
        Task Delete(IUpdateBuilder<ParticipantRoomLink> deleteLink);
    }
}