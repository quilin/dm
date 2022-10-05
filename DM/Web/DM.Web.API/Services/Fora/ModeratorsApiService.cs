using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Forum.BusinessProcesses.Moderation;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Services.Fora;

/// <inheritdoc />
internal class ModeratorsApiService : IModeratorsApiService
{
    private readonly IModeratorsReadingService moderatorsReadingService;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public ModeratorsApiService(
        IModeratorsReadingService moderatorsReadingService,
        IMapper mapper)
    {
        this.moderatorsReadingService = moderatorsReadingService;
        this.mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<ListEnvelope<User>> GetModerators(string id)
    {
        var moderators = await moderatorsReadingService.GetModerators(id);
        return new ListEnvelope<User>(moderators.Select(mapper.Map<User>));
    }
}