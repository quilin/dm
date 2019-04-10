using System;
using System.Threading.Tasks;
using DM.Web.API.Dto.Common;
using DM.Web.API.Dto.Contracts;

namespace DM.Web.API.Services.Fora
{
    /// <summary>
    /// API service for forum commentaries
    /// </summary>
    public interface ICommentApiService
    {
        /// <summary>
        /// Get topics commentaries
        /// </summary>
        /// <param name="topicId">Topic identifier</param>
        /// <param name="entityNumber">Entity number</param>
        /// <returns>Envelope of commentaries list</returns>
        Task<ListEnvelope<Comment>> Get(Guid topicId, int entityNumber);

        /// <summary>
        /// Create new comment
        /// </summary>
        /// <param name="topicId">Topic identifier</param>
        /// <param name="comment">Comment model</param>
        /// <returns>Envelope of created comment</returns>
        Task<Envelope<Comment>> Create(Guid topicId, Comment comment);

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