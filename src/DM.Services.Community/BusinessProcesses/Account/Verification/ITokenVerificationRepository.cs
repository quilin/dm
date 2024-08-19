using System;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Core.Dto;

namespace DM.Services.Community.BusinessProcesses.Account.Verification;

/// <summary>
/// Storage for token verification
/// </summary>
internal interface ITokenVerificationRepository
{
    /// <summary>
    /// Get user that token was generated for
    /// </summary>
    /// <param name="tokenId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<GeneralUser> GetTokenOwner(Guid tokenId, CancellationToken cancellationToken);
}