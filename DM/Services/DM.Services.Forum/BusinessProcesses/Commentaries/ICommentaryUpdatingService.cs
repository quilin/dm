using System.Threading.Tasks;
using DM.Services.Forum.Dto.Input;
using DM.Services.Forum.Dto.Output;

namespace DM.Services.Forum.BusinessProcesses.Commentaries
{
    /// <summary>
    /// Service for updating forum commentaries
    /// </summary>
    public interface ICommentaryUpdatingService
    {
        /// <summary>
        /// Update existing comment
        /// </summary>
        /// <param name="updateComment">Update comment model</param>
        /// <returns></returns>
        Task<Comment> Update(UpdateComment updateComment);
    }
}