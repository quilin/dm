using System;
using System.Threading.Tasks;

namespace DM.Services.Gaming.BusinessProcesses.Claims.Deleting;

/// <summary>
/// Service for deleting room claims
/// </summary>
public interface IRoomClaimsDeletingService
{
    /// <summary>
    /// Delete existing claim
    /// </summary>
    /// <param name="claimId">Claim identifier</param>
    /// <returns></returns>
    Task Delete(Guid claimId);
}