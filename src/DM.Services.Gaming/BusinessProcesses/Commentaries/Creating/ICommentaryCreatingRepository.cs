using System.Threading.Tasks;
using DM.Services.Common.Dto;
using DbComment = DM.Services.DataAccess.BusinessObjects.Common.Comment;

namespace DM.Services.Gaming.BusinessProcesses.Commentaries.Creating;

/// <summary>
/// Creating commentaries storage
/// </summary>
internal interface ICommentaryCreatingRepository
{
    /// <summary>
    /// Create comment from DAL
    /// </summary>
    /// <param name="comment">DAL model for comment</param>
    /// <returns></returns>
    Task<Comment> Create(DbComment comment);
}