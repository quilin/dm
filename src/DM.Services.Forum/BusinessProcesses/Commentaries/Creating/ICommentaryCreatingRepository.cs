using System.Threading.Tasks;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.DataAccess.RelationalStorage;
using Comment = DM.Services.DataAccess.BusinessObjects.Common.Comment;

namespace DM.Services.Forum.BusinessProcesses.Commentaries.Creating;

/// <summary>
/// Creating commentaries storage
/// </summary>
public interface ICommentaryCreatingRepository
{
    /// <summary>
    /// Create comment from DAL
    /// </summary>
    /// <param name="comment">DAL model for comment</param>
    /// <param name="topicUpdate">Updating for parent topic (denormalize)</param>
    /// <returns></returns>
    Task<Services.Common.Dto.Comment> Create(Comment comment, IUpdateBuilder<ForumTopic> topicUpdate);
}