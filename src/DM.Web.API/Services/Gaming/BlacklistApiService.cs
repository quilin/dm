using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Gaming.BusinessProcesses.Blacklist.Creating;
using DM.Services.Gaming.BusinessProcesses.Blacklist.Deleting;
using DM.Services.Gaming.BusinessProcesses.Blacklist.Reading;
using DM.Services.Gaming.Dto.Input;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Services.Gaming;

/// <inheritdoc />
internal class BlacklistApiService(
    IBlacklistReadingService readingService,
    IBlacklistCreatingService creatingService,
    IBlacklistDeletingService deletingService,
    IMapper mapper) : IBlacklistApiService
{
    /// <inheritdoc />
    public async Task<ListEnvelope<User>> Get(Guid gameId, CancellationToken cancellationToken)
    {
        var users = await readingService.Get(gameId, cancellationToken);
        return new ListEnvelope<User>(users.Select(mapper.Map<User>));
    }

    /// <inheritdoc />
    public async Task<Envelope<User>> Create(Guid gameId, User user, CancellationToken cancellationToken)
    {
        var createBlacklistLink = mapper.Map<OperateBlacklistLink>(user);
        createBlacklistLink.GameId = gameId;
        var result = await creatingService.Create(createBlacklistLink, cancellationToken);
        return new Envelope<User>(mapper.Map<User>(result));
    }

    /// <inheritdoc />
    public Task Delete(Guid gameId, string login, CancellationToken cancellationToken) =>
        deletingService.Delete(new OperateBlacklistLink {GameId = gameId, Login = login}, cancellationToken);
}