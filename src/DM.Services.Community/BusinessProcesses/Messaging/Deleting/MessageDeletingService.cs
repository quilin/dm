using System;
using System.Threading;
using System.Threading.Tasks;

namespace DM.Services.Community.BusinessProcesses.Messaging.Deleting;

/// <inheritdoc />
internal class MessageDeletingService : IMessageDeletingService
{
    /// <inheritdoc />
    public Task Delete(Guid messageId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException("Messages deleting is not available so far");
    }
}