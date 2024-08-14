using System.Threading.Tasks;
using DM.Services.Core.Dto;

namespace DM.Services.Community.BusinessProcesses.Account.EmailChange;

/// <summary>
/// Service for changing user email
/// </summary>
public interface IEmailChangeService
{
    /// <summary>
    /// Change user email
    /// </summary>
    /// <param name="emailChange"></param>
    /// <returns></returns>
    Task<GeneralUser> Change(UserEmailChange emailChange);
}