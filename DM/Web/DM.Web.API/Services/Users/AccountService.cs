using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation;
using DM.Services.Core.Exceptions;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;
using DM.Web.Core.Authentication;
using DM.Web.Core.Authentication.Credentials;
using DM.Web.Core.Extensions.EnumExtensions;
using Microsoft.AspNetCore.Http;

namespace DM.Web.API.Services.Users
{
    public class AccountService : IAccountService
    {
        private readonly IWebAuthenticationService authenticationService;
        private readonly IIdentityProvider identityProvider;

        public AccountService(
            IWebAuthenticationService authenticationService,
            IIdentityProvider identityProvider)
        {
            this.authenticationService = authenticationService;
            this.identityProvider = identityProvider;
        }
        
        public async Task<Envelope<User>> Login(LoginCredentials credentials, HttpContext httpContext)
        {
            await authenticationService.Authenticate(credentials, httpContext);
            var authenticationResult = identityProvider.Current;
            switch (authenticationResult.Error)
            {
                case AuthenticationError.NoError:
                    return new Envelope<User>(new User
                    {
                        Login = authenticationResult.User.Login,
                        Roles = authenticationResult.User.Role.GetUserRoleDescription(),
                        Rating = new Rating(authenticationResult.User),
                        Online = authenticationResult.User.LastVisitDate,
                        ProfilePictureUrl = authenticationResult.User.ProfilePictureUrl
                    });
                case AuthenticationError.WrongLogin:
                    throw new HttpBadRequestException(new Dictionary<string, string>
                    {
                        ["login"] = "There are no users found with this login. Maybe there was a typo?"
                    });
                case AuthenticationError.WrongPassword:
                    throw new HttpBadRequestException(new Dictionary<string, string>
                    {
                        ["login"] = "The password is incorrect. Did you forget to switch the keyboard?"
                    });
                case AuthenticationError.Banned:
                case AuthenticationError.Inactive:
                case AuthenticationError.Removed:
                case AuthenticationError.Forbidden:
                    throw new HttpException(HttpStatusCode.Forbidden, $"User is {authenticationResult.Error.ToString().ToLower()}. Address the technical support for more details");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}