using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using Microsoft.AspNetCore.Http;

namespace DM.Web.Core.Authentication.Credentials
{
    public interface ICredentialsStorage
    {
        Task<TokenCredentials> ExtractToken(HttpContext httpContext);
        Task Load(HttpContext httpContext, IIdentity identity);
        Task Unload(HttpContext httpContext);
    }
}