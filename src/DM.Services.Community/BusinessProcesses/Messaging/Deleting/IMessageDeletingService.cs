using System;
using System.Threading;
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
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task Delete(Guid messageId, CancellationToken cancellationToken);
}