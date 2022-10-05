using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Web.Core.Authentication.Credentials;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DM.Web.Core.Authentication;

/// <inheritdoc />
internal class WebAuthenticationService : IWebAuthenticationService
{
    private readonly IAuthenticationService authenticationService;
    private readonly ICredentialsStorage credentialsStorage;
    private readonly IIdentitySetter identitySetter;
    private readonly ILogger<WebAuthenticationService> logger;

    /// <inheritdoc />
    public WebAuthenticationService(
        IAuthenticationService authenticationService,
        ICredentialsStorage credentialsStorage,
        IIdentitySetter identitySetter,
        ILogger<WebAuthenticationService> logger)
    {
        this.authenticationService = authenticationService;
        this.credentialsStorage = credentialsStorage;
        this.identitySetter = identitySetter;
        this.logger = logger;
    }

    private async Task<IIdentity> GetAuthenticationResult(AuthCredentials credentials) => credentials switch
    {
        LoginCredentials loginCredentials => await authenticationService.Authenticate(loginCredentials.Login,
            loginCredentials.Password, loginCredentials.RememberMe),
        TokenCredentials tokenCredentials => await authenticationService.Authenticate(tokenCredentials.Token),
        UnconditionalCredentials unconditionalCredentials => await authenticationService.Authenticate(
            unconditionalCredentials.UserId),
        _ => Identity.Guest()
    };

    /// <inheritdoc />
    public async Task<IIdentity> Authenticate(AuthCredentials credentials, HttpContext httpContext)
    {
        var identity = identitySetter.Current = await GetAuthenticationResult(credentials);
        await TryLoadAuthenticationResult(httpContext, identity);
        return identity;
    }

    /// <inheritdoc />
    public async Task Logout(HttpContext httpContext)
    {
        var identity = identitySetter.Current = await authenticationService.Logout();
        await TryLoadAuthenticationResult(httpContext, identity);
    }

    /// <inheritdoc />
    public async Task<IIdentity> LogoutElsewhere(HttpContext httpContext)
    {
        var identity = identitySetter.Current = await authenticationService.LogoutElsewhere();
        await TryLoadAuthenticationResult(httpContext, identity);
        return identity;
    }

    private Task TryLoadAuthenticationResult(HttpContext httpContext, IIdentity identity)
    {
        if (identity.Error == AuthenticationError.ForgedToken)
        {
            logger.LogError($"Seems like someone is trying to forge the token for {identity.User.Login}");
        }

        return identity.Error == AuthenticationError.NoError && identity.User.IsAuthenticated
            ? credentialsStorage.Load(httpContext, identity)
            : credentialsStorage.Unload(httpContext);
    }
}