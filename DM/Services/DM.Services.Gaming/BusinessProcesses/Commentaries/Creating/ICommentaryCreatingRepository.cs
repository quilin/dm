using System.Threading.Tasks;
using Comment = DM.Services.DataAccess.BusinessObjects.Common.Comment;

namespace DM.Services.Gaming.BusinessProcesses.Commentaries.Creating
{
    /// <summary>
    /// Creating commentaries storage
    /// </summary>
    public interface ICommentaryCreatingRepository
    {
        /// <summary>
        /// Create comment from DAL
        /// </summary>
        /// <param name="comment">DAL model for comment</param>
        /// <returns></returns>
        Task<Services.Common.Dto.Comment> Create(Comment comment);
    }
}