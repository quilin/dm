using System.Threading;
using System.Threading.Tasks;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Services.Users;

/// <summary>
/// API service for password reseting
/// </summary>
public interface IPasswordResetApiService
{
    /// <summary>
    /// Reset user password
    /// </summary>
    /// <param name="resetPassword"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Envelope<User>> Reset(ResetPassword resetPassword, CancellationToken cancellationToken);

    /// <summary>
    /// Change user password
    /// </summary>
    /// <param name="changePassword"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Envelope<User>> Change(ChangePassword changePassword, CancellationToken cancellationToken);
}