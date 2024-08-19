using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Community.BusinessProcesses.Account.EmailChange;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Services.Users;

/// <inheritdoc />
internal class EmailChangeApiService(
    IEmailChangeService service,
    IMapper mapper) : IEmailChangeApiService
{
    /// <inheritdoc />
    public async Task<Envelope<User>> Change(ChangeEmail changeEmail, CancellationToken cancellationToken)
    {
        var emailChange = mapper.Map<UserEmailChange>(changeEmail);
        var user = await service.Change(emailChange, cancellationToken);
        return new Envelope<User>(mapper.Map<User>(user));
    }
}