using System.Threading;
using System.Threading.Tasks;

namespace DM.Services.Community.BusinessProcesses.Account.Registration;

/// <summary>
/// Service for user registration
/// </summary>
public interface IRegistrationService
{
    /// <summary>
    /// Register new user
    /// </summary>
    /// <param name="registration">Registration info</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task Register(UserRegistration registration, CancellationToken cancellationToken);
}