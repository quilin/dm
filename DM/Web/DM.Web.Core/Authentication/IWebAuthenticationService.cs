using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Web.Core.Authentication.Credentials;
using Microsoft.AspNetCore.Http;

namespace DM.Web.Core.Authentication
{
    /// <summary>
    /// Authentication service
    /// </summary>
    public interface IWebAuthenticationService
    {
        /// <summary>
        /// Authenticate via DM authentication token
        /// </summary>
        /// <param name="httpContext">HTTP context</param>
        /// <returns></returns>
        Task<IIdentity> Authenticate(HttpContext httpContext);

        /// <summary>
        /// Authenticate via login-password
        /// </summary>
        /// <param name="credentials">Login credentials</param>
        /// <param name="httpContext">HTTP context</param>
        /// <returns></returns>
        Task<IIdentity> Authenticate(LoginCredentials credentials, HttpContext httpContext);

        /// <summary>
        /// Logout as current user
        /// </summary>
        /// <param name="httpContext">HTTP context</param>
        /// <returns></returns>
        Task Logout(HttpContext httpContext);

        /// <summary>
        /// Logout as current user from every device
        /// </summary>
        /// <param name="httpContext">HTTP context</param>
        /// <returns></returns>
        Task<IIdentity> LogoutAll(HttpContext httpContext);
    }
}