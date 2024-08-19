using System.Threading;
using System.Threading.Tasks;
using DM.Services.DataAccess.RelationalStorage;
using Comment = DM.Services.DataAccess.BusinessObjects.Common.Comment;

namespace DM.Services.Gaming.BusinessProcesses.Commentaries.Updating;

/// <summary>
/// Updating commentary storage
/// </summary>
public interface ICommentaryUpdatingRepository
{
    /// <summary>
    /// Update single commentary
    /// </summary>
    /// <param name="update">Update commentary</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Common.Dto.Comment> Update(IUpdateBuilder<Comment> update, CancellationToken cancellationToken);
}