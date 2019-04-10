using System;
using System.Threading.Tasks;
using DM.Services.Common.Dto;
using DM.Services.DataAccess.RelationalStorage;
using DbComment = DM.Services.DataAccess.BusinessObjects.Common.Comment;

namespace DM.Services.Common.BusinessProcesses.Commentaries
{
    /// <summary>
    /// Comments storage
    /// </summary>
    public interface ICommentRepository
    {
        /// <summary>
        /// Get single comment by its identifier
        /// </summary>
        /// <param name="commentId">Commentary identifier</param>
        /// <returns>Found commentary</returns>
        Task<Comment> Get(Guid commentId);

        /// <summary>
        /// Create comment from DAL
        /// </summary>
        /// <param name="comment">DAL model for comment</param>
        /// <returns></returns>
        Task<Comment> Create(DbComment comment);

        /// <summary>
        /// Update existing commentary
        /// </summary>
        /// <param name="commentId">Commentary identifier</param>
        /// <param name="update">Updated fields</param>
        /// <returns></returns>
        Task<Comment> Update(Guid commentId, UpdateBuilder<DbComment> update);
    }
}