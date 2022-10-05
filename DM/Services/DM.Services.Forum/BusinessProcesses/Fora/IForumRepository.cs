using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.Forum.BusinessProcesses.Fora;

/// <summary>
/// Fora storage
/// </summary>
internal interface IForumRepository
{
    /// <summary>
    /// Get list of available fora by access policy
    /// </summary>
    /// <param name="accessPolicy">Forum access policy</param>
    /// <returns></returns>
    Task<IEnumerable<Dto.Output.Forum>> SelectFora(ForumAccessPolicy? accessPolicy);
}