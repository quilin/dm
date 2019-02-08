using System.Threading.Tasks;
using DM.Services.Authentication.Implementation;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;
using DM.Web.Core.Authentication;
using DM.Web.Core.Authentication.Credentials;
using Microsoft.AspNetCore.Http;

namespace DM.Web.API.Services.Users
{
    public class AccountService : IAccountService
    {
        private readonly IWebAuthenticationService authenticationService;
        private readonly IUserProvider userProvider;

        public AccountService(
            IWebAuthenticationService authenticationService,
            IUserProvider userProvider)
        {
            this.authenticationService = authenticationService;
            this.userProvider = userProvider;
        }
        
        public async Task<Envelope<User>> Login(LoginCredentials credentials, HttpContext httpContext)
        {
            await authenticationService.Authenticate(credentials, httpContext);
            return new Envelope<User>
            {
                Resource = new User
                {
                    Login = userProvider.Current.Login
                }
            };
        }
    }
}