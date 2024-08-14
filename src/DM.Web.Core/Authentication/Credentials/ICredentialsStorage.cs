using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using Microsoft.AspNetCore.Http;

namespace DM.Web.Core.Authentication.Credentials;

/// <summary>
/// Credentials storage
/// </summary>
public interface ICredentialsStorage
{
    /// <summary>
    /// Extract authentication token from request
    /// </summary>
    /// <param name="httpContext">HTTP context</param>
    /// <returns>Token</returns>
    Task<TokenCredentials> ExtractToken(HttpContext httpContext);

    /// <summary>
    /// Append authentication token to response
    /// </summary>
    /// <param name="httpContext">HTTP context</param>
    /// <param name="identity">Authenticated user identity</param>
    /// <returns></returns>
    Task Load(HttpContext httpContext, IIdentity identity);

    /// <summary>
    /// Remove authentication token from response
    /// </summary>
    /// <param name="httpContext">HTTP context</param>
    /// <returns></returns>
    Task Unload(HttpContext httpContext);
}