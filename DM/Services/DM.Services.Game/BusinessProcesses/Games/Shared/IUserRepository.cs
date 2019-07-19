using System.Threading;
using System.Threading.Tasks;

namespace DM.Services.Game.BusinessProcesses.Games.Shared
{
    /// <summary>
    /// User storage
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// User with login exists
        /// </summary>
        /// <param name="login">User login</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> UserExists(string login, CancellationToken cancellationToken);
    }
}