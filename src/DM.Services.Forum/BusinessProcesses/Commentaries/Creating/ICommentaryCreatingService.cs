using System.Threading;
using System.Threading.Tasks;
using DM.Services.Common.Dto;

namespace DM.Services.Forum.BusinessProcesses.Commentaries.Creating;

/// <summary>
/// Service to create new commentaries on forum
/// </summary>
public interface ICommentaryCreatingService
{
    /// <summary>
    /// Create new commentary
    /// </summary>
    /// <param name="createComment">Create commentary DTO model</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Comment> Create(CreateComment createComment, CancellationToken cancellationToken);
}