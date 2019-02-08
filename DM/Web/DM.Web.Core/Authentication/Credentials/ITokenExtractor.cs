using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DM.Web.Core.Authentication.Credentials
{
    public interface ITokenExtractor
    {
        Task<TokenCredentials> Extract(HttpContext httpContext);
    }
}