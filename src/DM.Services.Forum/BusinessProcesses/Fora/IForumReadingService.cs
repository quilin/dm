using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DM.Services.Forum.BusinessProcesses.Fora;

/// <summary>
/// Service for reading fora
/// </summary>
public interface IForumReadingService
{
    /// <summary>
    /// Get list of available fora
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<Dto.Output.Forum>> GetForaList(CancellationToken cancellationToken);

    /// <summary>
    /// Get available forum by title with counters
    /// </summary>
    /// <param name="forumTitle">Forum title</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Dto.Output.Forum> GetSingleForum(string forumTitle, CancellationToken cancellationToken);

    /// <summary>
    /// Get available forum by title with no counters
    /// </summary>
    /// <param name="forumTitle">Forum title</param>
    /// <param name="cancellationToken"></param>
    /// <param name="onlyAvailable">Only search in forums that are available for display for current user</param>
    /// <returns></returns>
    Task<Dto.Output.Forum> GetForum(string forumTitle, bool onlyAvailable, CancellationToken cancellationToken);
}