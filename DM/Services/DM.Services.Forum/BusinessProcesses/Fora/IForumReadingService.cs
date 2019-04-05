using System.Collections.Generic;
using System.Threading.Tasks;

namespace DM.Services.Forum.BusinessProcesses.Fora
{
    /// <summary>
    /// Service for reading fora
    /// </summary>
    public interface IForumReadingService
    {
        /// <summary>
        /// Get list of available fora
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Dto.Forum>> GetForaList();

        /// <summary>
        /// Get available forum by title with unread topics counters
        /// </summary>
        /// <param name="forumTitle">Forum title</param>
        /// <returns></returns>
        Task<Dto.Forum> GetForumWithCounters(string forumTitle);

        /// <summary>
        /// Get available forum by title with no counters
        /// </summary>
        /// <param name="forumTitle">Forum title</param>
        /// <param name="onlyAvailable">Only search in forums that are available for display for current user</param>
        /// <returns></returns>
        Task<Dto.Forum> GetForum(string forumTitle, bool onlyAvailable = true);
    }
}