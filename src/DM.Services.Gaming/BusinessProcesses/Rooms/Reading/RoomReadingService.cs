using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Common.Extensions;
using DM.Services.Core.Exceptions;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.BusinessProcesses.Rooms.Reading;

/// <inheritdoc />
internal class RoomReadingService(
    IGameReadingService gameReadingService,
    IRoomReadingRepository repository,
    IUnreadCountersRepository unreadCountersRepository,
    IIdentityProvider identityProvider) : IRoomReadingService
{
    /// <inheritdoc />
    public async Task<IEnumerable<Room>> GetAll(Guid gameId, CancellationToken cancellationToken)
    {
        await gameReadingService.GetGame(gameId, cancellationToken);
        var currentUserId = identityProvider.Current.User.UserId;
        var rooms = (await repository.GetAllAvailable(gameId, currentUserId, cancellationToken)).ToArray();
        await unreadCountersRepository.FillEntityCounters(rooms, currentUserId,
            r => r.Id, r => r.UnreadPostsCount, UnreadEntryType.Message, cancellationToken);
        return rooms;
    }

    /// <inheritdoc />
    public async Task<Room> Get(Guid roomId, CancellationToken cancellationToken)
    {
        var currentUserId = identityProvider.Current.User.UserId;
        var room = await repository.GetAvailable(roomId, currentUserId, cancellationToken);
        if (room == null)
        {
            throw new HttpException(HttpStatusCode.Gone, "Room not found");
        }

        await unreadCountersRepository.FillEntityCounters(new[] {room}, currentUserId,
            r => r.Id, r => r.UnreadPostsCount, UnreadEntryType.Message, cancellationToken);
        return room;
    }
}