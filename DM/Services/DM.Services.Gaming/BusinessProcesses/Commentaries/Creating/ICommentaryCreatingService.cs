using System.Threading.Tasks;
using DM.Services.Common.Dto;

namespace DM.Services.Gaming.BusinessProcesses.Commentaries.Creating
{
    /// <summary>
    /// Service to create new commentaries in game
    /// </summary>
    public interface ICommentaryCreatingService
    {
        /// <summary>
        /// Create new commentary
        /// </summary>
        /// <param name="createComment">Create commentary DTO model</param>
        /// <returns></returns>
        Task<Comment> Create(CreateComment createComment);
    }
}