using System;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Core.Exceptions;
using DM.Services.DataAccess.BusinessObjects.Games.Links;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Authorization;

namespace DM.Services.Gaming.BusinessProcesses.Pending.Deleting;

/// <inheritdoc />
internal class PendingPostDeletingService : IPendingPostDeletingService
{
    private readonly IIntentionManager intentionManager;
    private readonly IUpdateBuilderFactory updateBuilderFactory;
    private readonly IPendingPostDeletingRepository repository;

    /// <inheritdoc />
    public PendingPostDeletingService(
        IIntentionManager intentionManager,
        IUpdateBuilderFactory updateBuilderFactory,
        IPendingPostDeletingRepository repository)
    {
        this.intentionManager = intentionManager;
        this.updateBuilderFactory = updateBuilderFactory;
        this.repository = repository;
    }
        
    /// <inheritdoc />
    public async Task Delete(Guid pendingPostId)
    {
        var pendingPost = await repository.Get(pendingPostId);
        if (pendingPost == null)
        {
            throw new HttpException(HttpStatusCode.Gone, "Pending post not found");
        }

        intentionManager.ThrowIfForbidden(RoomIntention.DeletePending, pendingPost);
        await repository.Delete(updateBuilderFactory.Create<PendingPost>(pendingPostId).Delete());
    }
}