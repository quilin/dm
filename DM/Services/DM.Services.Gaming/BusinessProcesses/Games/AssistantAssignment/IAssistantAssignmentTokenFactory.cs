using System;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Gaming.BusinessProcesses.Games.AssistantAssignment;

/// <summary>
/// Factory for assistant assignment token
/// </summary>
internal interface IAssistantAssignmentTokenFactory
{
    /// <summary>
    /// Create token for game assistant request
    /// </summary>
    /// <param name="userId">User identifier</param>
    /// <param name="gameId">Game identifier</param>
    /// <returns></returns>
    Token Create(Guid userId, Guid gameId);
}