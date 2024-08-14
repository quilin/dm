using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DM.Web.Core.Authentication.Credentials;

/// <summary>
/// Authentication token extractor
/// </summary>
internal interface ITokenExtractor
{
    /// <summary>
    /// Extract token from request
    /// </summary>
    /// <param name="httpContext">HTTP context</param>
    /// <returns>Token</returns>
    Task<TokenCredentials> Extract(HttpContext httpContext);
}