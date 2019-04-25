using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Forum.Dto.Output;

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
        /// Get single comment by its identifier
        /// </summary>
        /// <param name="commentId">Commentary identifier</param>
        /// <returns>Found commentary</returns>
        Task<Comment> Get(Guid commentId);

        /// <summary>
        /// Create comment from DAL
        /// </summary>
        /// <param name="forumComment">DAL model for comment</param>
        /// <param name="topicUpdate">Updating for parent topic (denormalize)</param>
        /// <returns></returns>
        Task<Comment> Create(ForumComment forumComment, UpdateBuilder<ForumTopic> topicUpdate);

        /// <summary>
        /// Update existing commentary
        /// </summary>
        /// <param name="update">Updated fields</param>
        /// <returns></returns>
        Task<Comment> Update(UpdateBuilder<ForumComment> update);
    }
}