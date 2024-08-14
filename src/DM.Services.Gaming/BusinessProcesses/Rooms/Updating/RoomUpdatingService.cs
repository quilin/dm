using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Core.Exceptions;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.Dto.Input;
using DM.Services.Gaming.Dto.Internal;
using DM.Services.Gaming.Dto.Output;
using FluentValidation;
using DbRoom = DM.Services.DataAccess.BusinessObjects.Games.Posts.Room;

namespace DM.Services.Gaming.BusinessProcesses.Rooms.Updating;

/// <inheritdoc />
internal class RoomUpdatingService : IRoomUpdatingService
{
    private readonly IValidator<UpdateRoom> validator;
    private readonly IIntentionManager intentionManager;
    private readonly IUpdateBuilderFactory updateBuilderFactory;
    private readonly IRoomOrderPull roomOrderPull;
    private readonly IRoomUpdatingRepository repository;
    private readonly IIdentityProvider identityProvider;

    /// <inheritdoc />
    public RoomUpdatingService(
        IValidator<UpdateRoom> validator,
        IIntentionManager intentionManager,
        IUpdateBuilderFactory updateBuilderFactory,
        IRoomOrderPull roomOrderPull,
        IRoomUpdatingRepository repository,
        IIdentityProvider identityProvider)
    {
        this.validator = validator;
        this.intentionManager = intentionManager;
        this.updateBuilderFactory = updateBuilderFactory;
        this.roomOrderPull = roomOrderPull;
        this.repository = repository;
        this.identityProvider = identityProvider;
    }

    /// <inheritdoc />
    public async Task<Room> Update(UpdateRoom updateRoom)
    {
        await validator.ValidateAndThrowAsync(updateRoom);
        var currentUserId = identityProvider.Current.User.UserId;
        var room = await repository.GetRoom(updateRoom.RoomId, currentUserId);
        if (room == null)
        {
            throw new HttpException(HttpStatusCode.Gone, "Room not found");
        }
            
        intentionManager.ThrowIfForbidden(GameIntention.Edit, room.Game);

        var roomUpdate = updateBuilderFactory.Create<DbRoom>(updateRoom.RoomId)
            .MaybeField(r => r.Title, updateRoom.Title)
            .MaybeField(r => r.Type, updateRoom.Type)
            .MaybeField(r => r.AccessType, updateRoom.AccessType);

        if (updateRoom.PreviousRoomId == null || updateRoom.PreviousRoomId.Value == room.PreviousRoomId)
        {
            return await repository.Update(roomUpdate);
        }

        if (!updateRoom.PreviousRoomId.Value.HasValue)
        {
            return await InsertFirst(room, roomUpdate);
        }

        var targetRoom = await repository.GetRoom(updateRoom.PreviousRoomId.Value.Value, currentUserId);
        if (targetRoom.Game.Id != room.Game.Id)
        {
            throw new HttpBadRequestException(new Dictionary<string, string>
            {
                [nameof(Room.PreviousRoomId)] = ValidationError.Invalid
            });
        }

        return await InsertAfter(room, targetRoom, roomUpdate);
    }

    private async Task<Room> InsertAfter(RoomToUpdate room, RoomToUpdate afterRoom,
        IUpdateBuilder<DbRoom> updateRoom)
    {
        var (updateOldPreviousRoom, updateOldNextRoom) = roomOrderPull.GetPullChanges(room);
        var updateNewPreviousRoom = updateBuilderFactory.Create<DbRoom>(afterRoom.Id)
            .Field(r => r.NextRoomId, room.Id);
        var updateNewNextRoom = afterRoom.NextRoom == null
            ? null
            : updateBuilderFactory.Create<DbRoom>(afterRoom.NextRoom.Id)
                .Field(r => r.PreviousRoomId, room.Id);

        var nextRoomOrderNumber = afterRoom.NextRoom?.OrderNumber;
        updateRoom
            .Field(r => r.PreviousRoomId, afterRoom.Id)
            .Field(r => r.NextRoomId, afterRoom.NextRoom?.Id)
            .Field(r => r.OrderNumber, nextRoomOrderNumber.HasValue
                ? (nextRoomOrderNumber.Value + afterRoom.OrderNumber) / 2
                : afterRoom.OrderNumber + 1);

        return await repository.Update(updateRoom,
            updateOldNextRoom, updateOldPreviousRoom, updateNewNextRoom, updateNewPreviousRoom);
    }

    private async Task<Room> InsertFirst(RoomToUpdate room, IUpdateBuilder<DbRoom> updateRoom)
    {
        var (updateOldPreviousRoom, updateOldNextRoom) = roomOrderPull.GetPullChanges(room);
        var firstRoomInfo = await repository.GetFirstRoomInfo(room.Game.Id);
        var updateNewNextRoom = updateBuilderFactory.Create<DbRoom>(firstRoomInfo.Id)
            .Field(r => r.PreviousRoomId, room.Id);
        updateRoom
            .Field(r => r.NextRoomId, firstRoomInfo.Id)
            .Field(r => r.OrderNumber, firstRoomInfo.OrderNumber - 1);

        return await repository.Update(updateRoom, updateOldNextRoom, updateOldPreviousRoom, updateNewNextRoom);
    }
}