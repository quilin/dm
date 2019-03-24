using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Community.Dto;
using DM.Services.Core.Dto;

namespace DM.Services.Community.Implementation
{
    /// <summary>
    /// Community-related service
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Get community users list
        /// </summary>
        /// <param name="entityNumber">Selected user number</param>
        /// <param name="withInactive">Search among inactive users</param>
        /// <returns>Pair of found users and paging data</returns>
        Task<(IEnumerable<GeneralUser> users, PagingData paging)> Get(int entityNumber, bool withInactive);

        /// <summary>
        /// Get community user by login
        /// </summary>
        /// <param name="login">User login</param>
        /// <returns>Found user</returns>
        Task<GeneralUser> Get(string login);

        /// <summary>
        /// Get extended user profile model
        /// </summary>
        /// <param name="login">User login</param>
        /// <returns>Extended user model</returns>
        Task<UserProfile> GetProfile(string login);
    }
}