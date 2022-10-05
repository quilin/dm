using System;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Rooms.Updating;
using DM.Services.MessageQueuing.GeneralBus;
using DbRoom = DM.Services.DataAccess.BusinessObjects.Games.Posts.Room;

namespace DM.Services.Gaming.BusinessProcesses.Rooms.Deleting;

/// <inheritdoc />
internal class RoomDeletingService : IRoomDeletingService
{
    private readonly IIntentionManager intentionManager;
    private readonly IUpdateBuilderFactory updateBuilderFactory;
    private readonly IRoomOrderPull roomOrderPull;
    private readonly IRoomUpdatingRepository repository;
    private readonly IUnreadCountersRepository unreadCountersRepository;
    private readonly IInvokedEventProducer producer;
    private readonly IIdentityProvider identityProvider;

    /// <inheritdoc />
    public RoomDeletingService(
        IIntentionManager intentionManager,
        IUpdateBuilderFactory updateBuilderFactory,
        IRoomOrderPull roomOrderPull,
        IRoomUpdatingRepository repository,
        IUnreadCountersRepository unreadCountersRepository,
        IInvokedEventProducer producer,
        IIdentityProvider identityProvider)
    {
        this.intentionManager = intentionManager;
        this.updateBuilderFactory = updateBuilderFactory;
        this.roomOrderPull = roomOrderPull;
        this.repository = repository;
        this.unreadCountersRepository = unreadCountersRepository;
        this.producer = producer;
        this.identityProvider = identityProvider;
    }
        
    /// <inheritdoc />
    public async Task Delete(Guid roomId)
    {
        var room = await repository.GetRoom(roomId, identityProvider.Current.User.UserId);
        intentionManager.ThrowIfForbidden(GameIntention.Edit, room.Game);

        var updateRoom = updateBuilderFactory.Create<DbRoom>(roomId).Field(r => r.IsRemoved, true);
        var (updateOldPreviousRoom, updateOldNextRoom) = roomOrderPull.GetPullChanges(room);

        await repository.Update(updateRoom, updateOldNextRoom, updateOldPreviousRoom);
        await unreadCountersRepository.Delete(roomId, UnreadEntryType.Message);
        await producer.Send(EventType.DeletedRoom, roomId);
    }
}