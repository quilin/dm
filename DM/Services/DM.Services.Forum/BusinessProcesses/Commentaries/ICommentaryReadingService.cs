using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using Comment = DM.Services.Common.Dto.Comment;

namespace DM.Services.Forum.BusinessProcesses.Commentaries
{
    /// <summary>
    /// Service for reading forum commentaries
    /// </summary>
    public interface ICommentaryReadingService
    {
        /// <summary>
        /// Get list of topic comments by topic id
        /// </summary>
        /// <param name="topicId">Topic identifier</param>
        /// <param name="entityNumber">Number of entity that page must contain</param>
        /// <returns>Pair of comments list and paging data</returns>
        Task<(IEnumerable<Comment> comments, PagingData paging)> GetCommentsList(Guid topicId, int entityNumber);

        /// <summary>
        /// Get topic comment by id
        /// </summary>
        /// <param name="commentId">Commentary identifier</param>
        /// <returns>Found comment</returns>
        Task<Comment> Get(Guid commentId);
    }
}