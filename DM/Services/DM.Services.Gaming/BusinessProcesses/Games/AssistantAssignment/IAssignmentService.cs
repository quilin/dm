using System;
using System.Threading.Tasks;

namespace DM.Services.Gaming.BusinessProcesses.Games.AssistantAssignment;

/// <summary>
/// Service to assign assistants to game
/// </summary>
public interface IAssignmentService
{
    /// <summary>
    /// Create new assignment request
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <param name="userId">User identifier</param>
    /// <returns></returns>
    Task CreateAssignment(Guid gameId, Guid userId);
        
    /// <summary>
    /// User accepts the request and becomes the game assistant
    /// </summary>
    /// <returns></returns>
    Task AcceptAssignment(Guid tokenId);

    /// <summary>
    /// User rejects the request
    /// </summary>
    /// <param name="tokenId"></param>
    /// <returns></returns>
    Task RejectAssignment(Guid tokenId);
}