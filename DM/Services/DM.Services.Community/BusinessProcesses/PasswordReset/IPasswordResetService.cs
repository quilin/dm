using System.Threading.Tasks;
using DM.Services.Core.Dto;

namespace DM.Services.Community.BusinessProcesses.PasswordReset
{
    /// <summary>
    /// Service for password resetting
    /// </summary>
    public interface IPasswordResetService
    {
        /// <summary>
        /// Reset user password
        /// </summary>
        /// <param name="login">Login</param>
        /// <param name="email">Email</param>
        /// <returns></returns>
        Task<GeneralUser> Reset(string login, string email);
    }
}