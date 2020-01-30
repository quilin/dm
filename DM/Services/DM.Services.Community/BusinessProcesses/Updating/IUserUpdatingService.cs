using System.Threading.Tasks;
using DM.Services.Community.Dto;

namespace DM.Services.Community.BusinessProcesses.Updating
{
    /// <summary>
    /// Service for user update
    /// </summary>
    public interface IUserUpdatingService
    {
        /// <summary>
        /// Update user details
        /// </summary>
        /// <param name="updateUser"></param>
        /// <returns></returns>
        Task<UserDetails> Update(UpdateUser updateUser);
    }
}