using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using Microsoft.AspNetCore.Http;

namespace DM.Web.Core.Authentication.Credentials
{
    public interface ICredentialsStorage
    {
        Task<(bool success, AuthCredentials credentials)> Extract(HttpContext httpContext);
        Task Load(HttpContext httpContext, AuthenticationResult authenticationResult);
    }
}