using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.BusinessProcesses.Claims.Reading
{
    /// <summary>
    /// Service for reading room claims
    /// </summary>
    public interface IRoomClaimsReadingService
    {
        /// <summary>
        /// Get all room claims
        /// </summary>
        /// <param name="roomId">Room identifier</param>
        /// <returns></returns>
        Task<IEnumerable<RoomClaim>> GetAll(Guid roomId);

        /// <summary>
        /// Get existing claim
        /// </summary>
        /// <param name="claimId">Claim identifier</param>
        /// <returns></returns>
        Task<RoomClaim> Get(Guid claimId);
    }
}