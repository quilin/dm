using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Dto.Output;
using DbRoom = DM.Services.DataAccess.BusinessObjects.Games.Posts.Room;

namespace DM.Services.Gaming.BusinessProcesses.Rooms.Updating
{
    /// <summary>
    /// Factory for room neighbours changes on room order move
    /// </summary>
    public interface IRoomOrderPull
    {
        /// <summary>
        /// Get room neighbours changes on pull
        /// </summary>
        /// <param name="room">Room</param>
        /// <returns></returns>
        (IUpdateBuilder<DbRoom> updateOldPrevious, IUpdateBuilder<DbRoom> updateOldNext) GetPullChanges(Room room);
    }
}