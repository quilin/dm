using System.Threading.Tasks;
using DM.Services.Core.Dto;

namespace DM.Services.Community.BusinessProcesses.Account.PasswordReset;

/// <summary>
/// Service for password resetting
/// </summary>
public interface IPasswordResetService
{
    /// <summary>
    /// Reset user password
    /// </summary>
    /// <param name="passwordReset"></param>
    /// <returns></returns>
    Task<GeneralUser> Reset(UserPasswordReset passwordReset);
}