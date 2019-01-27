using System.Threading.Tasks;
using DM.Services.Authentication.Dto;

namespace DM.Services.Authentication.Implementation
{
    public interface IAuthenticationService
    {
        Task<(AuthenticationError Error, AuthenticatingUser User)> Authenticate(
            string login, string password, string persistent);

        Task<(AuthenticationError Error, AuthenticatingUser User)> Authenticate(string authToken);
    }
}