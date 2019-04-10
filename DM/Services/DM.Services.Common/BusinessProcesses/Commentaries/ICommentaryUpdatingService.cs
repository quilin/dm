using System.Threading.Tasks;
using DM.Services.Common.Dto;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.Common.BusinessProcesses.Commentaries
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
        /// <param name="eventType">Event type</param>
        /// <returns></returns>
        Task<Comment> Update(UpdateComment updateComment, EventType eventType);
    }
}