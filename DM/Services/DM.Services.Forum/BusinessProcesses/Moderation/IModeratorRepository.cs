using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto;

namespace DM.Services.Forum.BusinessProcesses.Moderation;

/// <summary>
/// Forum moderators storage
/// </summary>
internal interface IModeratorRepository
{
    /// <summary>
    /// Get list of forum moderators
    /// </summary>
    /// <param name="forumId">Forum id</param>
    /// <returns></returns>
    Task<IEnumerable<GeneralUser>> Get(Guid forumId);
}