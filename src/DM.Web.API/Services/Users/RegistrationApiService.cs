using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Community.BusinessProcesses.Account.Registration;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Services.Users;

/// <inheritdoc />
internal class RegistrationApiService(
    IRegistrationService registrationService,
    IMapper mapper) : IRegistrationApiService
{
    /// <inheritdoc />
    public Task Register(Registration registration, CancellationToken cancellationToken) =>
        registrationService.Register(mapper.Map<UserRegistration>(registration), cancellationToken);
}