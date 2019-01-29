using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DM.Web.Core.Authentication
{
    public interface IWebAuthenticationService
    {
        Task Authenticate(HttpContext httpContext);
    }
}