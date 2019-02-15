using System.Threading.Tasks;
using DM.Services.Authentication.Dto;

namespace DM.Services.Authentication.Implementation
{
    public interface IAuthenticationService
    {
        Task<IIdentity> Authenticate(string login, string password, bool persistent);
        Task<IIdentity> Authenticate(string authToken);

        Task Logout();
        Task<IIdentity> LogoutAll();
    }
}