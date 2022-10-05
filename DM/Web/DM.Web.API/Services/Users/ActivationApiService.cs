using System;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Authentication.Implementation;
using DM.Services.Community.BusinessProcesses.Account.Activation;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Services.Users;

/// <inheritdoc />
internal class ActivationApiService : IActivationApiService
{
    private readonly IActivationService activationService;
    private readonly IAuthenticationService authenticationService;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public ActivationApiService(
        IActivationService activationService,
        IAuthenticationService authenticationService,
        IMapper mapper)
    {
        this.activationService = activationService;
        this.authenticationService = authenticationService;
        this.mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<Envelope<User>> Activate(Guid token)
    {
        var userId = await activationService.Activate(token);
        var identity = await authenticationService.Authenticate(userId);
        return new Envelope<User>(mapper.Map<User>(identity.User));
    }
}