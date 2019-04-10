using System.Threading.Tasks;
using DM.Services.Common.Dto;
using DM.Services.Forum.Dto.Input;

namespace DM.Services.Forum.BusinessProcesses.Commentaries
{
    /// <summary>
    /// Service to create new commentaries on forum
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