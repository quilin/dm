using System;
using System.Threading;
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
internal class RoomDeletingService(
    IIntentionManager intentionManager,
    IUpdateBuilderFactory updateBuilderFactory,
    IRoomOrderPull roomOrderPull,
    IRoomUpdatingRepository repository,
    IUnreadCountersRepository unreadCountersRepository,
    IInvokedEventProducer producer,
    IIdentityProvider identityProvider) : IRoomDeletingService
{
    /// <inheritdoc />
    public async Task Delete(Guid roomId, CancellationToken cancellationToken)
    {
        var room = await repository.GetRoom(roomId, identityProvider.Current.User.UserId, cancellationToken);
        intentionManager.ThrowIfForbidden(GameIntention.Edit, room.Game);

        var updateRoom = updateBuilderFactory.Create<DbRoom>(roomId).Field(r => r.IsRemoved, true);
        var (updateOldPreviousRoom, updateOldNextRoom) = roomOrderPull.GetPullChanges(room);

        await repository.Update(updateRoom, updateOldNextRoom, updateOldPreviousRoom,
            null, null, cancellationToken);
        await unreadCountersRepository.Delete(roomId, UnreadEntryType.Message, cancellationToken);
        await producer.Send(EventType.DeletedRoom, roomId);
    }
}