using System;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Games.Links;
using DM.Services.Gaming.Dto.Input;

namespace DM.Services.Gaming.BusinessProcesses.Pending.Creating;

/// <inheritdoc />
internal class PendingPostFactory : IPendingPostFactory
{
    private readonly IGuidFactory guidFactory;
    private readonly IDateTimeProvider dateTimeProvider;

    /// <inheritdoc />
    public PendingPostFactory(
        IGuidFactory guidFactory,
        IDateTimeProvider dateTimeProvider)
    {
        this.guidFactory = guidFactory;
        this.dateTimeProvider = dateTimeProvider;
    }

    /// <inheritdoc />
    public PendingPost Create(CreatePendingPost createPendingPost, Guid awaitingUserId, Guid pendingUserId)
    {
        return new PendingPost
        {
            PendingPostId = guidFactory.Create(),
            RoomId = createPendingPost.RoomId,
            AwaitingUserId = awaitingUserId,
            PendingUserId = pendingUserId,
            CreateDate = dateTimeProvider.Now
        };
    }
}