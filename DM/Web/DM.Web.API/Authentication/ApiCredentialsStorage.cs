using System.Linq;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Web.Core.Authentication.Credentials;
using Microsoft.AspNetCore.Http;

namespace DM.Web.API.Authentication
{
    public class ApiCredentialsStorage : ICredentialsStorage
    {
        private const string HttpAuthTokenHeader = "X-Dm-Auth-Token";

        public Task<TokenCredentials> ExtractToken(HttpContext httpContext)
        {
            if (!httpContext.Request.Headers.TryGetValue(HttpAuthTokenHeader, out var headerValues))
            {
                return Task.FromResult<TokenCredentials>(null);
            }
            var authToken = headerValues.FirstOrDefault();
            return string.IsNullOrEmpty(authToken)
                ? Task.FromResult<TokenCredentials>(null)
                : Task.FromResult(new TokenCredentials {Token = authToken});
        }

        public Task Load(HttpContext httpContext, IIdentity identity)
        {
            httpContext.Response.Headers.Add(HttpAuthTokenHeader, identity.Token);
            return Task.CompletedTask;
        }

        public Task Unload(HttpContext httpContext)
        {
            httpContext.Response.Headers.Remove(HttpAuthTokenHeader);
            return Task.CompletedTask;
        }
    }
}