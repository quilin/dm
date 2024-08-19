using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Forum.BusinessProcesses.Moderation;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Services.Fora;

/// <inheritdoc />
internal class ModeratorsApiService(
    IModeratorsReadingService moderatorsReadingService,
    IMapper mapper) : IModeratorsApiService
{
    /// <inheritdoc />
    public async Task<ListEnvelope<User>> GetModerators(string id, CancellationToken cancellationToken)
    {
        var moderators = await moderatorsReadingService.GetModerators(id, cancellationToken);
        return new ListEnvelope<User>(moderators.Select(mapper.Map<User>));
    }
}