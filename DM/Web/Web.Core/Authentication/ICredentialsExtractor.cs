using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DM.Web.Core.Authentication
{
    public interface ICredentialsExtractor
    {
        Task<(bool success, AuthCredentials credentials)> Extract(HttpContext httpContext);
    }
}