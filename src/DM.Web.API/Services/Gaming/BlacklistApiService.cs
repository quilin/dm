using System;
using System.Linq;
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
internal class BlacklistApiService : IBlacklistApiService
{
    private readonly IBlacklistReadingService readingService;
    private readonly IBlacklistCreatingService creatingService;
    private readonly IBlacklistDeletingService deletingService;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public BlacklistApiService(
        IBlacklistReadingService readingService,
        IBlacklistCreatingService creatingService,
        IBlacklistDeletingService deletingService,
        IMapper mapper)
    {
        this.readingService = readingService;
        this.creatingService = creatingService;
        this.deletingService = deletingService;
        this.mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<ListEnvelope<User>> Get(Guid gameId)
    {
        var users = await readingService.Get(gameId);
        return new ListEnvelope<User>(users.Select(mapper.Map<User>));
    }

    /// <inheritdoc />
    public async Task<Envelope<User>> Create(Guid gameId, User user)
    {
        var createBlacklistLink = mapper.Map<OperateBlacklistLink>(user);
        createBlacklistLink.GameId = gameId;
        var result = await creatingService.Create(createBlacklistLink);
        return new Envelope<User>(mapper.Map<User>(result));
    }

    /// <inheritdoc />
    public Task Delete(Guid gameId, string login) =>
        deletingService.Delete(new OperateBlacklistLink {GameId = gameId, Login = login});
}