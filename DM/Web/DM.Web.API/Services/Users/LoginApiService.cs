using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Community.BusinessProcesses.Users.Reading;
using DM.Services.Core.Exceptions;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;
using DM.Web.Core.Authentication;
using DM.Web.Core.Authentication.Credentials;
using Microsoft.AspNetCore.Http;
using UserDetails = DM.Web.API.Dto.Users.UserDetails;

namespace DM.Web.API.Services.Users;

/// <inheritdoc />
internal class LoginApiService : ILoginApiService
{
    private readonly IWebAuthenticationService authenticationService;
    private readonly IIdentityProvider identityProvider;
    private readonly IUserReadingService userReadingService;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public LoginApiService(
        IWebAuthenticationService authenticationService,
        IIdentityProvider identityProvider,
        IUserReadingService userReadingService,
        IMapper mapper)
    {
        this.authenticationService = authenticationService;
        this.identityProvider = identityProvider;
        this.userReadingService = userReadingService;
        this.mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<Envelope<User>> Login(LoginCredentials credentials, HttpContext httpContext)
    {
        var identity = await authenticationService.Authenticate(credentials, httpContext);
        switch (identity.Error)
        {
            case AuthenticationError.NoError:
                var userDetails = await userReadingService.GetDetails(identityProvider.Current.User.Login);
                return new Envelope<User>(mapper.Map<User>(userDetails));
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
                var userState = identity.Error.ToString().ToLower();
                throw new HttpException(HttpStatusCode.Forbidden,
                    $"User is {userState}. Address the technical support for more details");
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    /// <inheritdoc />
    public Task Logout(HttpContext httpContext) => authenticationService.Logout(httpContext);

    /// <inheritdoc />
    public Task LogoutAll(HttpContext httpContext) => authenticationService.LogoutElsewhere(httpContext);

    /// <inheritdoc />
    public async Task<Envelope<UserDetails>> GetCurrent()
    {
        var userDetails = await userReadingService.GetDetails(identityProvider.Current.User.Login);
        return new Envelope<UserDetails>(mapper.Map<UserDetails>(userDetails));
    }
}