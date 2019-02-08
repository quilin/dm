using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Web.Core.Authentication.Credentials;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace DM.Web.API.Authentication
{
    public class ApiCredentialsExtractor :
        ICredentialsExtractor<LoginCredentials>,
        ICredentialsExtractor<TokenCredentials>,
        ICredentialsLoader
    {
        private const string HttpAuthTokenHeader = "X-Dm-Auth-Token";
        private const string LoginKey = "login";
        private const string PasswordKey = "password";
        private const string DoNotRememberKey = "doNotRemember";

        async Task<(bool success, LoginCredentials credentials)> ICredentialsExtractor<LoginCredentials>.Extract(
            HttpContext httpContext)
        {
            try
            {
                using (var reader = new StreamReader(httpContext.Request.Body))
                {
                    var content = JsonConvert.DeserializeObject<IDictionary<string, string>>(
                        await reader.ReadToEndAsync());
                    if (content.TryGetValue(LoginKey, out var login) &&
                        content.TryGetValue(PasswordKey, out var password))
                    {
                        return (true, new LoginCredentials
                        {
                            Login = login,
                            Password = password,
                            RememberMe = content.TryGetValue(DoNotRememberKey, out var doNotRememberString) &&
                                bool.TryParse(doNotRememberString, out var doNotRemember) && !doNotRemember
                        });
                    }

                    return default;
                }
            }
            catch
            {
                return default;
            }
        }

        Task<(bool success, TokenCredentials credentials)> ICredentialsExtractor<TokenCredentials>.Extract(
            HttpContext httpContext)
        {
            if (!httpContext.Request.Headers.TryGetValue(HttpAuthTokenHeader, out var headerValues))
            {
                return Task.FromResult<(bool success, TokenCredentials credentials)>((false, null));
            }
            var authToken = headerValues.FirstOrDefault();
            return string.IsNullOrEmpty(authToken)
                ? Task.FromResult<(bool success, TokenCredentials credentials)>((false, null))
                : Task.FromResult<(bool success, TokenCredentials credentials)>((true,
                    new TokenCredentials {Token = authToken}));
        }

        public Task Load(HttpContext httpContext, AuthenticationResult authenticationResult)
        {
            httpContext.Response.Headers.Add(HttpAuthTokenHeader, authenticationResult.Token);
            return Task.CompletedTask;
        }
    }
}