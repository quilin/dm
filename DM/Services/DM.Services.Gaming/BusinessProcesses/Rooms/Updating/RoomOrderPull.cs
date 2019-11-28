using DM.Services.DataAccess.BusinessObjects.Games.Posts;
using DM.Services.DataAccess.RelationalStorage;

namespace DM.Services.Gaming.BusinessProcesses.Rooms.Updating
{
    /// <inheritdoc />
    public class RoomOrderPull : IRoomOrderPull
    {
        private readonly IUpdateBuilderFactory updateBuilderFactory;

        /// <inheritdoc />
        public RoomOrderPull(
            IUpdateBuilderFactory updateBuilderFactory)
        {
            this.updateBuilderFactory = updateBuilderFactory;
        }

        /// <inheritdoc />
        public (IUpdateBuilder<Room> updateOldPrevious, IUpdateBuilder<Room> updateOldNext)
            GetPullChanges(Dto.Output.Room room)
        {
            var updateOldPreviousRoom = room.PreviousRoomId.HasValue
                ? updateBuilderFactory.Create<Room>(room.PreviousRoomId.Value)
                    .Field(r => r.NextRoomId, room.NextRoomId)
                : null;
            var updateOldNextRoom = room.NextRoomId.HasValue
                ? updateBuilderFactory.Create<Room>(room.NextRoomId.Value)
                    .Field(r => r.PreviousRoomId, room.PreviousRoomId)
                : null;
            return (updateOldPreviousRoom, updateOldNextRoom);
        }
    }
}