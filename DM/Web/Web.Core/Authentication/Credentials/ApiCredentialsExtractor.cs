using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace DM.Web.Core.Authentication.Credentials
{
    public class ApiCredentialsExtractor : ICredentialsExtractor
    {
        private const string HttpAuthTokenHeader = "X-Dm-Auth-Token";
        private const string LoginKey = "login";
        private const string PasswordKey = "password";
        private const string DoNotRememberKey = "doNotRemember";

        public async Task<(bool success, AuthCredentials credentials)> Extract(HttpContext httpContext)
        {
            if (httpContext.Request.Headers.TryGetValue(HttpAuthTokenHeader, out var headerValues))
            {
                var authToken = headerValues.FirstOrDefault();
                return string.IsNullOrEmpty(authToken)
                    ? default
                    : (true, new TokenCredentials {Token = authToken});
            }

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
    }
}