using System;
using System.Threading.Tasks;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Forum.Dto.Internal;

namespace DM.Services.Forum.BusinessProcesses.Commentaries.Deleting;

/// <summary>
/// Deleting commentary storage
/// </summary>
internal interface ICommentaryDeletingRepository
{
    /// <summary>
    /// Get single comment to delete by its identifier
    /// </summary>
    /// <param name="commentId"></param>
    /// <returns></returns>
    Task<CommentToDelete> GetForDelete(Guid commentId);

    /// <summary>
    /// Gets second last commentary identifier of the topic
    /// </summary>
    /// <param name="topicId">Topic identifier</param>
    /// <returns>Null if the only commentary of topic is the last one</returns>
    Task<Guid?> GetSecondLastCommentId(Guid topicId);

    /// <summary>
    /// Update existing commentary
    /// </summary>
    /// <param name="update">Updated fields</param>
    /// <param name="topicUpdate">Updating for parent topic (denormalize)</param>
    /// <returns></returns>
    Task Delete(IUpdateBuilder<Comment> update, IUpdateBuilder<ForumTopic> topicUpdate);
}