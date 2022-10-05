using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.DataAccess.BusinessObjects.Games;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.DataAccess.RelationalStorage;

namespace DM.Services.Gaming.BusinessProcesses.Games.AssistantAssignment;

/// <summary>
/// Assistant assignment storage
/// </summary>
internal interface IAssignmentRepository
{
    /// <summary>
    /// Get token for assignment
    /// </summary>
    /// <param name="tokenId">Token identifier</param>
    /// <param name="userId">User identifier</param>
    /// <returns></returns>
    Task<Guid?> FindGameToAssign(Guid tokenId, Guid userId);

    /// <summary>
    /// Assign assistant to game
    /// </summary>
    /// <param name="updateGame"></param>
    /// <param name="updateToken"></param>
    /// <returns></returns>
    Task AssignAssistant(IUpdateBuilder<Game> updateGame, IUpdateBuilder<Token> updateToken);

    /// <summary>
    /// Find all pending game assignment tokens
    /// </summary>
    /// <param name="gameId">Game identifiers</param>
    /// <returns></returns>
    Task<IEnumerable<Guid>> FindAssignments(Guid gameId);

    /// <summary>
    /// Invalidate pending and create new assignment token
    /// </summary>
    /// <param name="updates"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task InvalidateAndCreate(IEnumerable<IUpdateBuilder<Token>> updates, Token token);
}