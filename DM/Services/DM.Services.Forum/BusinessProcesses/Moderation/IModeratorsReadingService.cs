using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto;

namespace DM.Services.Forum.BusinessProcesses.Moderation;

/// <summary>
/// Service for reading forum moderators
/// </summary>
public interface IModeratorsReadingService
{
    /// <summary>
    /// Get list of forum moderators by forum title
    /// </summary>
    /// <param name="forumTitle">Forum title</param>
    /// <returns></returns>
    Task<IEnumerable<GeneralUser>> GetModerators(string forumTitle);
}