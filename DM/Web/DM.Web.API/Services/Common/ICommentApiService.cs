using System;
using System.Threading.Tasks;
using DM.Web.API.Dto.Contracts;
using Comment = DM.Web.API.Dto.Common.Comment;

namespace DM.Web.API.Services.Common
{
    /// <summary>
    /// API service for comment resources
    /// </summary>
    public interface ICommentApiService
    {
        /// <summary>
        /// Get comment by identifier
        /// </summary>
        /// <param name="commentId">Comment identifier</param>
        /// <returns></returns>
        Task<Envelope<Comment>> Get(Guid commentId);

        /// <summary>
        /// Update comment by API DTO model
        /// </summary>
        /// <param name="commentId">Comment identifier</param>
        /// <param name="comment">Comment DTO model</param>
        /// <returns></returns>
        Task<Envelope<Comment>> Update(Guid commentId, Comment comment);

        /// <summary>
        /// Delete comment
        /// </summary>
        /// <param name="commentId">Comment identifier</param>
        /// <returns></returns>
        Task Delete(Guid commentId);
    }
}