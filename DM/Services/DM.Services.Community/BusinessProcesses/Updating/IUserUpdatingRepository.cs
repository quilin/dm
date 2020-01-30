using System.Threading.Tasks;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.DataAccess.BusinessObjects.Users.Settings;
using DM.Services.DataAccess.RelationalStorage;

namespace DM.Services.Community.BusinessProcesses.Updating
{
    /// <summary>
    /// Storage for user updating
    /// </summary>
    public interface IUserUpdatingRepository
    {
        /// <summary>
        /// Save user changes
        /// </summary>
        /// <param name="updateUser"></param>
        /// <param name="settingsUpdate"></param>
        /// <returns></returns>
        Task Update(IUpdateBuilder<User> updateUser, IUpdateBuilder<UserSettings> settingsUpdate);
    }
}