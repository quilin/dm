using System.Threading;
using System.Threading.Tasks;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Community.BusinessProcesses.Account.Registration;

/// <summary>
/// Registration information storage
/// </summary>
internal interface IRegistrationRepository
{
    /// <summary>
    /// Tells if user with certain email is already registered
    /// </summary>
    /// <param name="email">Email</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> EmailFree(string email, CancellationToken cancellationToken);

    /// <summary>
    /// Tells if user with certain login is already registered
    /// </summary>
    /// <param name="login"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> LoginFree(string login, CancellationToken cancellationToken);

    /// <summary>
    /// Create new user
    /// </summary>
    /// <param name="user">User DAL</param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task AddUser(User user, Token token);
}