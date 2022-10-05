using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Web.Core.Authentication.Credentials;
using Microsoft.AspNetCore.Http;

namespace DM.Web.Core.Authentication;

/// <summary>
/// Authentication service
/// </summary>
public interface IWebAuthenticationService
{
    /// <summary>
    /// Authenticate
    /// </summary>
    /// <param name="credentials">Credentials</param>
    /// <param name="httpContext">HTTP context</param>
    /// <returns></returns>
    Task<IIdentity> Authenticate(AuthCredentials credentials, HttpContext httpContext);

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
    Task<IIdentity> LogoutElsewhere(HttpContext httpContext);
}