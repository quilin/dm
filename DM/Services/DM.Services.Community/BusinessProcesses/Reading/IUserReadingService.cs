using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Community.Dto;
using DM.Services.Core.Dto;

namespace DM.Services.Community.BusinessProcesses.Reading
{
    /// <summary>
    /// Community-related service
    /// </summary>
    public interface IUserReadingService
    {
        /// <summary>
        /// Get community users list
        /// </summary>
        /// <param name="query">Paging query</param>
        /// <param name="withInactive">Search among inactive users</param>
        /// <returns>Pair of found users and paging data</returns>
        Task<(IEnumerable<GeneralUser> users, PagingResult paging)> Get(PagingQuery query, bool withInactive);

        /// <summary>
        /// Get community user by login
        /// </summary>
        /// <param name="login">User login</param>
        /// <returns>Found user</returns>
        Task<UserDetails> Get(string login);
    }
}