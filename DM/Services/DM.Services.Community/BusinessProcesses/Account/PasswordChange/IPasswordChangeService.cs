using System.Threading.Tasks;
using DM.Services.Core.Dto;

namespace DM.Services.Community.BusinessProcesses.Account.PasswordChange;

/// <summary>
/// Service for user password changing
/// </summary>
public interface IPasswordChangeService
{
    /// <summary>
    /// Change user password
    /// </summary>
    /// <param name="passwordChange"></param>
    /// <returns></returns>
    Task<GeneralUser> Change(UserPasswordChange passwordChange);
}