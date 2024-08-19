using System;
using System.Threading;
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
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task Delete(Guid claimId, CancellationToken cancellationToken);
}