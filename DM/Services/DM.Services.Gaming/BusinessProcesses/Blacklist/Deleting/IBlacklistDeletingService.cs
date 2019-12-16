using System;
using System.Threading.Tasks;

namespace DM.Services.Gaming.BusinessProcesses.Blacklist.Deleting
{
    /// <summary>
    /// Service for deleting blacklist links
    /// </summary>
    public interface IBlacklistDeletingService
    {
        /// <summary>
        /// Delete existing blacklist link
        /// </summary>
        /// <param name="gameId">Game identifier</param>
        /// <param name="login">Login</param>
        /// <returns></returns>
        Task Delete(Guid gameId, string login);
    }
}