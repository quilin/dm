using System;
using DM.Services.DataAccess.BusinessObjects.Common;

namespace DM.Services.Community.BusinessProcesses.Chat.Creating;

/// <summary>
/// Factory for chat message DAL model
/// </summary>
internal interface IChatMessageFactory
{
    /// <summary>
    /// Create new DAL model
    /// </summary>
    /// <param name="createChatMessage">Creating DTO model</param>
    /// <param name="userId">User identifier</param>
    /// <returns></returns>
    ChatMessage Create(CreateChatMessage createChatMessage, Guid userId);
}