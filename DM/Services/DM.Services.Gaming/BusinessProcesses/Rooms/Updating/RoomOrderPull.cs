using DM.Services.DataAccess.BusinessObjects.Games.Posts;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Dto.Internal;

namespace DM.Services.Gaming.BusinessProcesses.Rooms.Updating;

/// <inheritdoc />
internal class RoomOrderPull : IRoomOrderPull
{
    private readonly IUpdateBuilderFactory updateBuilderFactory;

    /// <inheritdoc />
    public RoomOrderPull(
        IUpdateBuilderFactory updateBuilderFactory)
    {
        this.updateBuilderFactory = updateBuilderFactory;
    }

    /// <inheritdoc />
    public (IUpdateBuilder<Room> updateOldPrevious, IUpdateBuilder<Room> updateOldNext) GetPullChanges(
        RoomToUpdate room)
    {
        var updateOldPreviousRoom = room.PreviousRoom == null
            ? null
            : updateBuilderFactory.Create<Room>(room.PreviousRoom.Id)
                .Field(r => r.NextRoomId, room.NextRoom?.Id);
        var updateOldNextRoom = room.NextRoom == null
            ? null
            : updateBuilderFactory.Create<Room>(room.NextRoom.Id)
                .Field(r => r.PreviousRoomId, room.PreviousRoom?.Id);
        return (updateOldPreviousRoom, updateOldNextRoom);
    }
}