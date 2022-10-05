using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Community.BusinessProcesses.Account.EmailChange;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Services.Users;

/// <inheritdoc />
internal class EmailChangeApiService : IEmailChangeApiService
{
    private readonly IEmailChangeService service;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public EmailChangeApiService(
        IEmailChangeService service,
        IMapper mapper)
    {
        this.service = service;
        this.mapper = mapper;
    }
        
    /// <inheritdoc />
    public async Task<Envelope<User>> Change(ChangeEmail changeEmail)
    {
        var emailChange = mapper.Map<UserEmailChange>(changeEmail);
        var user = await service.Change(emailChange);
        return new Envelope<User>(mapper.Map<User>(user));
    }
}