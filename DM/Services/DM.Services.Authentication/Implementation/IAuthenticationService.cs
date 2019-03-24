using System.Threading.Tasks;
using DM.Services.Authentication.Dto;

namespace DM.Services.Authentication.Implementation
{
    /// <summary>
    /// User authentication service
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Authenticate via login credentials
        /// </summary>
        /// <param name="login">User login</param>
        /// <param name="password">User password</param>
        /// <param name="persistent">Persistence flag</param>
        /// <returns>Authentication identity</returns>
        Task<IIdentity> Authenticate(string login, string password, bool persistent);

        /// <summary>
        /// Authenticate via token credentials
        /// </summary>
        /// <param name="authToken">Authentication token</param>
        /// <returns>Authentication identity</returns>
        Task<IIdentity> Authenticate(string authToken);

        /// <summary>
        /// Logout as a current user
        /// </summary>
        /// <returns></returns>
        Task Logout();

        /// <summary>
        /// Logout from all devices except this
        /// </summary>
        /// <returns>Newly created authentication identity</returns>
        Task<IIdentity> LogoutAll();
    }
}