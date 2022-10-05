using System;
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
internal class RoomClaimsDeletingService : IRoomClaimsDeletingService
{
    private readonly IRoomClaimsDeletingRepository repository;
    private readonly IRoomClaimsReadingRepository readingRepository;
    private readonly IRoomUpdatingRepository roomUpdatingRepository;
    private readonly IIntentionManager intentionManager;
    private readonly IUpdateBuilderFactory updateBuilderFactory;
    private readonly IInvokedEventProducer producer;
    private readonly IIdentityProvider identityProvider;

    /// <inheritdoc />
    public RoomClaimsDeletingService(
        IRoomClaimsDeletingRepository repository,
        IRoomClaimsReadingRepository readingRepository,
        IRoomUpdatingRepository roomUpdatingRepository,
        IIntentionManager intentionManager,
        IUpdateBuilderFactory updateBuilderFactory,
        IInvokedEventProducer producer,
        IIdentityProvider identityProvider)
    {
        this.repository = repository;
        this.readingRepository = readingRepository;
        this.roomUpdatingRepository = roomUpdatingRepository;
        this.intentionManager = intentionManager;
        this.updateBuilderFactory = updateBuilderFactory;
        this.producer = producer;
        this.identityProvider = identityProvider;
    }

    /// <inheritdoc />
    public async Task Delete(Guid claimId)
    {
        var currentUserId = identityProvider.Current.User.UserId;
        var oldClaim = await readingRepository.GetClaim(claimId, currentUserId);
        var room = await roomUpdatingRepository.GetRoom(oldClaim.RoomId, currentUserId);
        intentionManager.ThrowIfForbidden(GameIntention.Edit, room.Game);

        var updateBuilder = updateBuilderFactory.Create<RoomClaim>(claimId).Delete();
        await repository.Delete(updateBuilder);
        await producer.Send(EventType.ChangedRoom, room.Id);
    }
}