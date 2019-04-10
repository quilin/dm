using System;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.Forum.Dto.Input;

namespace DM.Services.Forum.BusinessProcesses.Commentaries
{
    /// <summary>
    /// Factory for commentary DAL
    /// </summary>
    public interface ICommentFactory
    {
        /// <summary>
        /// Creates new commentary DAL from DTO and current user identifier
        /// </summary>
        /// <param name="createComment">New commentary DTO model</param>
        /// <param name="userId">Current user identifier</param>
        /// <returns>Commentary DAL model</returns>
        ForumComment Create(CreateComment createComment, Guid userId);
    }
}