using System;
using System.Collections.Generic;
using DbConversation = DM.Services.DataAccess.BusinessObjects.Messaging.Conversation;
using DbConversationLink = DM.Services.DataAccess.BusinessObjects.Messaging.UserConversationLink;

namespace DM.Services.Community.BusinessProcesses.Messaging.Reading;

/// <summary>
/// Factory for DAL conversation models
/// </summary>
internal interface IConversationFactory
{
    /// <summary>
    /// Create DAL entities for visavi conversation
    /// </summary>
    /// <param name="userId">User identifier</param>
    /// <param name="visaviId">Visavi user identifier</param>
    /// <returns></returns>
    (DbConversation conversation, IEnumerable<DbConversationLink>) CreateVisavi(Guid userId, Guid visaviId);
}