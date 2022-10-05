using System;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Games.Posts;
using DM.Services.Gaming.Dto.Input;

namespace DM.Services.Gaming.BusinessProcesses.Rooms.Creating;

/// <inheritdoc />
internal class RoomFactory : IRoomFactory
{
    private readonly IGuidFactory guidFactory;

    /// <inheritdoc />
    public RoomFactory(
        IGuidFactory guidFactory)
    {
        this.guidFactory = guidFactory;
    }

    /// <inheritdoc />
    public Room CreateFirst(CreateRoom createRoom)
    {
        return new Room
        {
            RoomId = guidFactory.Create(),
            GameId = createRoom.GameId,
            Title = createRoom.Title.Trim(),
            Type = createRoom.Type,
            AccessType = createRoom.AccessType,
            OrderNumber = 0
        };
    }

    /// <inheritdoc />
    public Room CreateAfter(CreateRoom createRoom, Guid lastRoomId, double lastRoomOrderNumber)
    {
        var result = CreateFirst(createRoom);
        result.PreviousRoomId = lastRoomId;
        result.OrderNumber = lastRoomOrderNumber + 1;
        return result;
    }
}