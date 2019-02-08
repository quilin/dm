using System.Threading.Tasks;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;
using Microsoft.AspNetCore.Http;

namespace DM.Web.API.Services.Users
{
    public interface IAccountService
    {
        Task<Envelope<User>> Login(HttpContext httpContext);
    }
}