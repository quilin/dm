using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Core.Dto;

namespace DM.Services.Community.BusinessProcesses.Users.Reading;

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
    /// <param name="cancellationToken"></param>
    /// <returns>Pair of found users and paging data</returns>
    Task<(IEnumerable<GeneralUser> users, PagingResult paging)> Get(
        PagingQuery query, bool withInactive, CancellationToken cancellationToken);

    /// <summary>
    /// Get community user short info by login
    /// </summary>
    /// <param name="login">User login</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<GeneralUser> Get(string login, CancellationToken cancellationToken);

    /// <summary>
    /// Get community user details by login
    /// </summary>
    /// <param name="login">User login</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Found user</returns>
    Task<UserDetails> GetDetails(string login, CancellationToken cancellationToken);
}