using System;
using System.Threading.Tasks;

namespace DM.Services.Community.BusinessProcesses.Messaging.Deleting;

/// <summary>
/// Service for message deleting
/// </summary>
public interface IMessageDeletingService
{
    /// <summary>
    /// Delete message
    /// </summary>
    /// <param name="messageId">Message identifier</param>
    /// <returns></returns>
    Task Delete(Guid messageId);
}