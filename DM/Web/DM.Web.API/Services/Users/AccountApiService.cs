using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation;
using DM.Services.Core.Exceptions;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;
using DM.Web.Core.Authentication;
using DM.Web.Core.Authentication.Credentials;
using Microsoft.AspNetCore.Http;

namespace DM.Web.API.Services.Users
{
    public class AccountApiService : IAccountApiService
    {
        private readonly IWebAuthenticationService authenticationService;
        private readonly IIdentityProvider identityProvider;
        private readonly IMapper mapper;

        public AccountApiService(
            IWebAuthenticationService authenticationService,
            IIdentityProvider identityProvider,
            IMapper mapper)
        {
            this.authenticationService = authenticationService;
            this.identityProvider = identityProvider;
            this.mapper = mapper;
        }
        
        public async Task<Envelope<User>> Login(LoginCredentials credentials, HttpContext httpContext)
        {
            await authenticationService.Authenticate(credentials, httpContext);
            var authenticationResult = identityProvider.Current;
            switch (authenticationResult.Error)
            {
                case AuthenticationError.NoError:
                    return new Envelope<User>(mapper.Map<User>(authenticationResult.User));
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
                    var userState = authenticationResult.Error.ToString().ToLower();
                    throw new HttpException(HttpStatusCode.Forbidden,
                        $"User is {userState}. Address the technical support for more details");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public Task Logout(HttpContext httpContext) => authenticationService.Logout(httpContext);

        public Task LogoutAll(HttpContext httpContext) => authenticationService.LogoutAll(httpContext);
    }
}