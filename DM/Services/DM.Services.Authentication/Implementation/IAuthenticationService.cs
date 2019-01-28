using System.Threading.Tasks;
using DM.Services.Authentication.Dto;

namespace DM.Services.Authentication.Implementation
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResult> Authenticate(string login, string password, bool persistent);

        Task<AuthenticationResult> Authenticate(string authToken);
    }
}