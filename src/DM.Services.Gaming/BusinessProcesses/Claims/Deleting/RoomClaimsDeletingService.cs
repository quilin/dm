using System;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Games.Links;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Claims.Reading;
using DM.Services.Gaming.BusinessProcesses.Rooms.Updating;
using DM.Services.MessageQueuing.GeneralBus;

namespace DM.Services.Gaming.BusinessProcesses.Claims.Deleting;

/// <inheritdoc />
internal class RoomClaimsDeletingService(
    IRoomClaimsDeletingRepository repository,
    IRoomClaimsReadingRepository readingRepository,
    IRoomUpdatingRepository roomUpdatingRepository,
    IIntentionManager intentionManager,
    IUpdateBuilderFactory updateBuilderFactory,
    IInvokedEventProducer producer,
    IIdentityProvider identityProvider) : IRoomClaimsDeletingService
{
    /// <inheritdoc />
    public async Task Delete(Guid claimId, CancellationToken cancellationToken)
    {
        var currentUserId = identityProvider.Current.User.UserId;
        var oldClaim = await readingRepository.GetClaim(claimId, currentUserId, cancellationToken);
        var room = await roomUpdatingRepository.GetRoom(oldClaim.RoomId, currentUserId, cancellationToken);
        intentionManager.ThrowIfForbidden(GameIntention.Edit, room.Game);

        var updateBuilder = updateBuilderFactory.Create<RoomClaim>(claimId).Delete();
        await repository.Delete(updateBuilder, cancellationToken);
        await producer.Send(EventType.ChangedRoom, room.Id);
    }
}