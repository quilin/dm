using System.Threading.Tasks;
using DM.Services.Gaming.Dto.Output;
using DbRoom = DM.Services.DataAccess.BusinessObjects.Games.Posts.Room;

namespace DM.Services.Gaming.BusinessProcesses.Rooms.Creating
{
    /// <summary>
    /// Game room creating storage
    /// </summary>
    public interface IRoomCreatingRepository
    {
        /// <summary>
        /// Create new room
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        Task<Room> Create(DbRoom room);
    }
}