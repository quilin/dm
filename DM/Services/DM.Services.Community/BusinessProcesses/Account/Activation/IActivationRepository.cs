using System;
using System.Threading.Tasks;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.DataAccess.RelationalStorage;

namespace DM.Services.Community.BusinessProcesses.Account.Activation;

/// <summary>
/// Activation storage
/// </summary>
internal interface IActivationRepository
{
    /// <summary>
    /// Find user identifier by its activation token identifier
    /// </summary>
    /// <param name="tokenId">Token identifier</param>
    /// <param name="createdSince">Created since</param>
    /// <returns></returns>
    Task<Guid?> FindUserToActivate(Guid tokenId, DateTimeOffset createdSince);

    /// <summary>
    /// Update user and its token
    /// </summary>
    /// <param name="updateUser">User update</param>
    /// <param name="updateToken">Token update</param>
    /// <returns></returns>
    Task ActivateUser(IUpdateBuilder<User> updateUser, IUpdateBuilder<Token> updateToken);
}