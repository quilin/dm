using System.Threading.Tasks;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Forum.Dto.Output;

namespace DM.Services.Forum.BusinessProcesses.Commentaries.Updating
{
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
        Task<Comment> Update(UpdateBuilder<ForumComment> update);
    }
}