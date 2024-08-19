using System.Threading;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.DataAccess.RelationalStorage;

namespace DM.Services.Community.BusinessProcesses.Account.EmailChange;

/// <summary>
/// Storage for email changing
/// </summary>
public interface IEmailChangeRepository
{
    /// <summary>
    /// Find user by login
    /// </summary>
    /// <param name="login"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<AuthenticatedUser> FindUser(string login, CancellationToken cancellationToken);

    /// <summary>
    /// Update user email
    /// </summary>
    /// <param name="updateUser"></param>
    /// <param name="token"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task Update(IUpdateBuilder<User> updateUser, Token token, CancellationToken cancellationToken);
}