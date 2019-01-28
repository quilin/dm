using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DM.Web.Core.Authentication.Credentials
{
    public interface ICredentialsExtractor
    {
        Task<(bool success, AuthCredentials credentials)> Extract(HttpContext httpContext);
    }
}