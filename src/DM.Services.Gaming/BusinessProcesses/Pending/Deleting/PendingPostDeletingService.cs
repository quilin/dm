using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Core.Exceptions;
using DM.Services.DataAccess.BusinessObjects.Games.Links;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Authorization;

namespace DM.Services.Gaming.BusinessProcesses.Pending.Deleting;

/// <inheritdoc />
internal class PendingPostDeletingService(
    IIntentionManager intentionManager,
    IUpdateBuilderFactory updateBuilderFactory,
    IPendingPostDeletingRepository repository) : IPendingPostDeletingService
{
    /// <inheritdoc />
    public async Task Delete(Guid pendingPostId, CancellationToken cancellationToken)
    {
        var pendingPost = await repository.Get(pendingPostId, cancellationToken);
        if (pendingPost == null)
        {
            throw new HttpException(HttpStatusCode.Gone, "Pending post not found");
        }

        intentionManager.ThrowIfForbidden(RoomIntention.DeletePending, pendingPost);
        await repository.Delete(updateBuilderFactory.Create<PendingPost>(pendingPostId).Delete(), cancellationToken);
    }
}