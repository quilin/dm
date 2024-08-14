using System;
using DM.Services.DataAccess.BusinessObjects.Games.Posts;
using DM.Services.Gaming.Dto.Input;

namespace DM.Services.Gaming.BusinessProcesses.Posts.Creating;

/// <summary>
/// Factory for post DAL model
/// </summary>
internal interface IPostFactory
{
    /// <summary>
    /// Create new game post
    /// </summary>
    /// <param name="createPost">DTO model</param>
    /// <param name="userId">User identifier</param>
    /// <returns></returns>
    Post Create(CreatePost createPost, Guid userId);
}