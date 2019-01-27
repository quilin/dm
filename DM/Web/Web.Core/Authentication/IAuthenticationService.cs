using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Web.Core.Authentication
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResult> Authenticate(HttpContext httpContext);
        void SetUserCredentials(AuthenticationResult authenticationResult);
    }
}