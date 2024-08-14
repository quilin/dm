using System;
using DM.Services.DataAccess.BusinessObjects.Messaging;

namespace DM.Services.Community.BusinessProcesses.Messaging.Creating;

/// <summary>
/// Factory for message DAL model
/// </summary>
internal interface IMessageFactory
{
    /// <summary>
    /// Create new message DAL model
    /// </summary>
    /// <param name="createMessage"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Message Create(CreateMessage createMessage, Guid userId);
}