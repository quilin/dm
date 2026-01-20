using System.Threading;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Core.Dto.Enums;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Rooms.Updating;
using DM.Services.Gaming.Dto.Input;
using DM.Services.Gaming.Dto.Output;
using DM.Services.MessageQueuing.GeneralBus;
using FluentValidation;

namespace DM.Services.Gaming.BusinessProcesses.Claims.Creating;

/// <inheritdoc />
internal class RoomClaimsCreatingService(
    IValidator<CreateRoomClaim> validator,
    IRoomUpdatingRepository updatingRepository,
    IIntentionManager intentionManager,
    IRoomClaimFactory factory,
    ICharacterClaimApprove characterClaimApprove,
    IReaderClaimApprove readerClaimApprove,
    IRoomClaimsCreatingRepository repository,
    IInvokedEventProducer producer,
    IIdentityProvider identityProvider) : IRoomClaimsCreatingService
{
    /// <inheritdoc />
    public async Task<RoomClaim> Create(CreateRoomClaim createRoomClaim, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(createRoomClaim, cancellationToken);
        var room = await updatingRepository.GetRoom(
            createRoomClaim.RoomId, identityProvider.Current.User.UserId, cancellationToken);
        intentionManager.ThrowIfForbidden(GameIntention.Edit, room.Game);

        var participantId = createRoomClaim.CharacterId.HasValue
            ? await characterClaimApprove.GetParticipantId(createRoomClaim.CharacterId.Value, room, cancellationToken)
            : await readerClaimApprove.GetParticipantId(createRoomClaim.ReaderLogin.Trim(), room, cancellationToken);

        var link = factory.Create(createRoomClaim, participantId);
        var result = await repository.Create(link, cancellationToken);
        await producer.Send(EventType.ChangedRoom, link.RoomId);

        return result;
    }
}