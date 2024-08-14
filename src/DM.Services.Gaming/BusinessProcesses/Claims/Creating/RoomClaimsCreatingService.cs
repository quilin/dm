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
internal class RoomClaimsCreatingService : IRoomClaimsCreatingService
{
    private readonly IValidator<CreateRoomClaim> validator;
    private readonly IRoomUpdatingRepository updatingRepository;
    private readonly IIntentionManager intentionManager;
    private readonly IRoomClaimFactory factory;
    private readonly ICharacterClaimApprove characterClaimApprove;
    private readonly IReaderClaimApprove readerClaimApprove;
    private readonly IRoomClaimsCreatingRepository repository;
    private readonly IInvokedEventProducer producer;
    private readonly IIdentityProvider identityProvider;

    /// <inheritdoc />
    public RoomClaimsCreatingService(
        IValidator<CreateRoomClaim> validator,
        IRoomUpdatingRepository updatingRepository,
        IIntentionManager intentionManager,
        IRoomClaimFactory factory,
        ICharacterClaimApprove characterClaimApprove,
        IReaderClaimApprove readerClaimApprove,
        IRoomClaimsCreatingRepository repository,
        IInvokedEventProducer producer,
        IIdentityProvider identityProvider)
    {
        this.validator = validator;
        this.updatingRepository = updatingRepository;
        this.intentionManager = intentionManager;
        this.factory = factory;
        this.characterClaimApprove = characterClaimApprove;
        this.readerClaimApprove = readerClaimApprove;
        this.repository = repository;
        this.producer = producer;
        this.identityProvider = identityProvider;
    }

    /// <inheritdoc />
    public async Task<RoomClaim> Create(CreateRoomClaim createRoomClaim)
    {
        await validator.ValidateAndThrowAsync(createRoomClaim);
        var room = await updatingRepository.GetRoom(createRoomClaim.RoomId, identityProvider.Current.User.UserId);
        intentionManager.ThrowIfForbidden(GameIntention.Edit, room.Game);

        var participantId = createRoomClaim.CharacterId.HasValue
            ? await characterClaimApprove.GetParticipantId(createRoomClaim.CharacterId.Value, room)
            : await readerClaimApprove.GetParticipantId(createRoomClaim.ReaderLogin.Trim(), room);
        ;
        var link = factory.Create(createRoomClaim, participantId);
        var result = await repository.Create(link);
        await producer.Send(EventType.ChangedRoom, link.RoomId);

        return result;
    }
}