using System;
using System.Threading.Tasks;

namespace DM.Services.Community.BusinessProcesses.Account.Activation;

/// <summary>
/// Service for user activation
/// </summary>
public interface IActivationService
{
    /// <summary>
    /// Activate user by token identifier
    /// </summary>
    /// <param name="tokenId">Activation token identifier</param>
    /// <returns>Activated user identifier</returns>
    Task<Guid> Activate(Guid tokenId);
}