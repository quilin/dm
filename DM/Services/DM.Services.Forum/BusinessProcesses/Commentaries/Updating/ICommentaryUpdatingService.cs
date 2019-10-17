using System.Threading.Tasks;
using DM.Services.Common.Dto;
using DM.Services.Forum.Dto.Input;

namespace DM.Services.Forum.BusinessProcesses.Commentaries.Updating
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