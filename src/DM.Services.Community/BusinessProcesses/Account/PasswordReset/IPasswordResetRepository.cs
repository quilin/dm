using System.Threading;
using System.Threading.Tasks;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Community.BusinessProcesses.Account.PasswordReset;

/// <summary>
/// Storage for password resetting
/// </summary>
internal interface IPasswordResetRepository
{
    /// <summary>
    /// Create password restoration token
    /// </summary>
    /// <param name="token"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task CreateToken(Token token, CancellationToken cancellationToken);
}