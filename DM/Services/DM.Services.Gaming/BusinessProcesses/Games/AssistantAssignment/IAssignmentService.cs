using System;
using System.Threading.Tasks;

namespace DM.Services.Gaming.BusinessProcesses.Games.AssistantAssignment
{
    /// <summary>
    /// Service to assign assistants to game
    /// </summary>
    public interface IAssignmentService
    {
        /// <summary>
        /// User accepts the request and becomes the game assistant
        /// </summary>
        /// <returns></returns>
        Task AcceptAssignment(Guid tokenId);

        /// <summary>
        /// User rejects the request
        /// </summary>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        Task RejectAssignment(Guid tokenId);

        /// <summary>
        /// Game master cancels pending assignment requests
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        Task CancelAssignments(Guid gameId);
    }
}