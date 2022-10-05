using System;
using System.Threading.Tasks;
using DM.Services.Core.Dto;

namespace DM.Services.Community.BusinessProcesses.Account.Verification;

/// <summary>
/// Service for token verification
/// </summary>
public interface ITokenVerificationService
{
    /// <summary>
    /// Verify if the token is available
    /// </summary>
    /// <param name="token">Token</param>
    /// <returns></returns>
    Task<GeneralUser> Verify(Guid token);
}