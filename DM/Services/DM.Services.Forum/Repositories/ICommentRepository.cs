using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Common.Dto;
using DM.Services.Core.Dto;

namespace DM.Services.Forum.Repositories
{
    /// <summary>
    /// Forum comments storage
    /// </summary>
    public interface ICommentRepository
    {
        /// <summary>
        /// Count comments of the topic
        /// </summary>
        /// <param name="topicId">Topic id</param>
        /// <returns>Number of topic comments</returns>
        Task<int> Count(Guid topicId);

        /// <summary>
        /// Get comments list of the topic
        /// </summary>
        /// <param name="topicId">Topic id</param>
        /// <param name="paging">Paging data</param>
        /// <returns></returns>
        Task<IEnumerable<Comment>> Get(Guid topicId, PagingData paging);
    }
}