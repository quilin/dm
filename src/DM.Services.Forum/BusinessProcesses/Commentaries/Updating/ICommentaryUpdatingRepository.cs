using System.Threading.Tasks;
using DM.Services.DataAccess.RelationalStorage;
using Comment = DM.Services.DataAccess.BusinessObjects.Common.Comment;

namespace DM.Services.Forum.BusinessProcesses.Commentaries.Updating;

/// <summary>
/// Updating commentary storage
/// </summary>
public interface ICommentaryUpdatingRepository
{
    /// <summary>
    /// Update single commentary
    /// </summary>
    /// <param name="update">Update commentary</param>
    /// <returns></returns>
    Task<Services.Common.Dto.Comment> Update(IUpdateBuilder<Comment> update);
}