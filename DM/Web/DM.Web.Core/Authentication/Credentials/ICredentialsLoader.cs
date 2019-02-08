using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using Microsoft.AspNetCore.Http;

namespace DM.Web.Core.Authentication.Credentials
{
    public interface ICredentialsLoader
    {
        Task Load(HttpContext httpContext, AuthenticationResult authenticationResult);
    }
}