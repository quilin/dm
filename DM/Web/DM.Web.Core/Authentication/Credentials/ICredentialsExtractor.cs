using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DM.Web.Core.Authentication.Credentials
{
    public interface ICredentialsExtractor {}
    
    public interface ICredentialsExtractor<TCredentials> : ICredentialsExtractor where TCredentials : AuthCredentials
    {
        Task<(bool success, TCredentials credentials)> Extract(HttpContext httpContext);
    }
}