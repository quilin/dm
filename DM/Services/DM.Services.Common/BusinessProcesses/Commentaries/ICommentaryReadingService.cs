using System;
using System.Threading.Tasks;
using DM.Services.Common.Dto;
using DbComment = DM.Services.DataAccess.BusinessObjects.Common.Comment;

namespace DM.Services.Common.BusinessProcesses.Commentaries
{
    /// <summary>
    /// Service for reading commentaries
    /// </summary>
    public interface ICommentaryReadingService
    {
        /// <summary>
        /// Get topic comment by id
        /// </summary>
        /// <param name="commentId">Commentary identifier</param>
        /// <returns>Found comment</returns>
        Task<Comment> Get(Guid commentId);
    }
}