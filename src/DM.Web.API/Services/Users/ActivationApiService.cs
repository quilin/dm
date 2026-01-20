using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Authentication.Implementation;
using DM.Services.Community.BusinessProcesses.Account.Activation;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Services.Users;

/// <inheritdoc />
internal class ActivationApiService(
    IActivationService activationService,
    IAuthenticationService authenticationService,
    IMapper mapper) : IActivationApiService
{
    /// <inheritdoc />
    public async Task<Envelope<User>> Activate(Guid token, CancellationToken cancellationToken)
    {
        var userId = await activationService.Activate(token, cancellationToken);
        var identity = await authenticationService.Authenticate(userId, cancellationToken);
        return new Envelope<User>(mapper.Map<User>(identity.User));
    }
}