using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Common.Extensions;
using DM.Services.Core.Exceptions;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.BusinessProcesses.Rooms.Reading;

/// <inheritdoc />
internal class RoomReadingService : IRoomReadingService
{
    private readonly IGameReadingService gameReadingService;
    private readonly IRoomReadingRepository repository;
    private readonly IUnreadCountersRepository unreadCountersRepository;
    private readonly IIdentityProvider identityProvider;

    /// <inheritdoc />
    public RoomReadingService(
        IGameReadingService gameReadingService,
        IRoomReadingRepository repository,
        IUnreadCountersRepository unreadCountersRepository,
        IIdentityProvider identityProvider)
    {
        this.gameReadingService = gameReadingService;
        this.repository = repository;
        this.unreadCountersRepository = unreadCountersRepository;
        this.identityProvider = identityProvider;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Room>> GetAll(Guid gameId)
    {
        await gameReadingService.GetGame(gameId);
        var currentUserId = identityProvider.Current.User.UserId;
        var rooms = (await repository.GetAllAvailable(gameId, currentUserId)).ToArray();
        await unreadCountersRepository.FillEntityCounters(rooms, currentUserId,
            r => r.Id, r => r.UnreadPostsCount);
        return rooms;
    }

    /// <inheritdoc />
    public async Task<Room> Get(Guid roomId)
    {
        var currentUserId = identityProvider.Current.User.UserId;
        var room = await repository.GetAvailable(roomId, currentUserId);
        if (room == null)
        {
            throw new HttpException(HttpStatusCode.Gone, "Room not found");
        }

        await unreadCountersRepository.FillEntityCounters(new[] {room}, currentUserId,
            r => r.Id, r => r.UnreadPostsCount);
        return room;
    }
}