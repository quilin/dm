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
    /// <returns></returns>
    Task<Envelope<User>> Reset(ResetPassword resetPassword);

    /// <summary>
    /// Change user password
    /// </summary>
    /// <param name="changePassword"></param>
    /// <returns></returns>
    Task<Envelope<User>> Change(ChangePassword changePassword);
}