using System.Threading.Tasks;
using DM.Services.Community.Dto;
using DM.Services.DataAccess.BusinessObjects.Users;
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
        /// <returns></returns>
        Task<UserDetails> Update(IUpdateBuilder<User> updateUser);
    }
}