using System.Threading.Tasks;
using DM.Web.Core.Authentication.Credentials;
using Microsoft.AspNetCore.Http;

namespace DM.Web.Core.Authentication
{
    public interface IWebAuthenticationService
    {
        Task Authenticate<TCredentials>(HttpContext httpContext) where TCredentials : AuthCredentials;
    }
}