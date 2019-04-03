using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.Forum.Repositories
{
    /// <summary>
    /// Fora storage
    /// </summary>
    public interface IForumRepository
    {
        /// <summary>
        /// Get list of available fora by access policy
        /// </summary>
        /// <param name="accessPolicy">Forum access policy</param>
        /// <returns></returns>
        Task<IEnumerable<Dto.Forum>> SelectFora(ForumAccessPolicy? accessPolicy);
    }
}