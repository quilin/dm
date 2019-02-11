using System.Threading.Tasks;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;
using DM.Web.Core.Authentication.Credentials;
using Microsoft.AspNetCore.Http;

namespace DM.Web.API.Services.Users
{
    public interface IAccountApiService
    {
        Task<Envelope<User>> Login(LoginCredentials credentials, HttpContext httpContext);
        Task Logout(HttpContext httpContext);
        Task LogoutAll(HttpContext httpContext);
    }
}