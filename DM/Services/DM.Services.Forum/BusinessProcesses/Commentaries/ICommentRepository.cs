using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Common.Dto;
using DM.Services.Core.Dto;
using DbComment = DM.Services.DataAccess.BusinessObjects.Common.Comment;

namespace DM.Services.Forum.BusinessProcesses.Commentaries
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

        /// <summary>
        /// Create comment from DAL
        /// </summary>
        /// <param name="comment">DAL model for comment</param>
        /// <returns></returns>
        Task<Comment> Create(DbComment comment);
    }
}