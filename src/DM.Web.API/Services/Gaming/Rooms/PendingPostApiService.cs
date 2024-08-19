using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Gaming.BusinessProcesses.Pending.Creating;
using DM.Services.Gaming.BusinessProcesses.Pending.Deleting;
using DM.Services.Gaming.Dto.Input;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Games;

namespace DM.Web.API.Services.Gaming.Rooms;

/// <inheritdoc />
internal class PendingPostApiService(
    IPendingPostCreatingService creatingService,
    IPendingPostDeletingService deletingService,
    IMapper mapper) : IPendingPostApiService
{
    /// <inheritdoc />
    public async Task<Envelope<PendingPost>> Create(
        Guid roomId, PendingPost pendingPost, CancellationToken cancellationToken)
    {
        var createPendingPost = mapper.Map<CreatePendingPost>(pendingPost);
        createPendingPost.RoomId = roomId;
        var createdPendingPost = await creatingService.Create(createPendingPost, cancellationToken);
        return new Envelope<PendingPost>(mapper.Map<PendingPost>(createdPendingPost));
    }

    /// <inheritdoc />
    public Task Delete(Guid pendingPostId, CancellationToken cancellationToken) =>
        deletingService.Delete(pendingPostId, cancellationToken);
}