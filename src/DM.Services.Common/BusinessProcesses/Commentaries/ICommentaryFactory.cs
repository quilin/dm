using System;
using DM.Services.Common.Dto;
using Comment = DM.Services.DataAccess.BusinessObjects.Common.Comment;

namespace DM.Services.Common.BusinessProcesses.Commentaries;

/// <summary>
/// Factory for commentary DAL
/// </summary>
public interface ICommentaryFactory
{
    /// <summary>
    /// Creates new commentary DAL from DTO and current user identifier
    /// </summary>
    /// <param name="createComment">New commentary DTO model</param>
    /// <param name="userId">Current user identifier</param>
    /// <returns>Commentary DAL model</returns>
    Comment Create(CreateComment createComment, Guid userId);
}