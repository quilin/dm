using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Core.Dto;

namespace DM.Services.Community.BusinessProcesses.Users.Reading;

/// <summary>
/// Community users storage
/// </summary>
public interface IUserReadingRepository
{
    /// <summary>
    /// Count community users by filter
    /// </summary>
    /// <param name="withInactive">Count inactive users too</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Number of the users</returns>
    Task<int> CountUsers(bool withInactive, CancellationToken cancellationToken);

    /// <summary>
    /// Get users list on paging data
    /// </summary>
    /// <param name="paging">Paging data</param>
    /// <param name="withInactive">Search among inactive users</param>
    /// <param name="cancellationToken"></param>
    /// <returns>List of users found</returns>
    Task<IEnumerable<GeneralUser>> GetUsers(PagingData paging, bool withInactive, CancellationToken cancellationToken);

    /// <summary>
    /// Get user by login
    /// </summary>
    /// <param name="login"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<GeneralUser> GetUser(string login, CancellationToken cancellationToken);

    /// <summary>
    /// Get user details by login
    /// </summary>
    /// <param name="login">User login</param>
    /// <param name="cancellationToken"></param>
    /// <returns>User found. Null if none found</returns>
    Task<UserDetails> GetUserDetails(string login, CancellationToken cancellationToken);
}