using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Core.Exceptions;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Rooms.Reading;
using DM.Services.Gaming.Dto.Input;
using DM.Services.Gaming.Dto.Output;
using FluentValidation;
using DbRoom = DM.Services.DataAccess.BusinessObjects.Games.Posts.Room;

namespace DM.Services.Gaming.BusinessProcesses.Rooms.Updating
{
    /// <inheritdoc />
    public class RoomUpdatingService : IRoomUpdatingService
    {
        private readonly IValidator<UpdateRoom> validator;
        private readonly IRoomReadingService readingService;
        private readonly IIntentionManager intentionManager;
        private readonly IUpdateBuilderFactory updateBuilderFactory;
        private readonly IRoomUpdatingRepository repository;

        /// <inheritdoc />
        public RoomUpdatingService(
            IValidator<UpdateRoom> validator,
            IRoomReadingService readingService,
            IIntentionManager intentionManager,
            IUpdateBuilderFactory updateBuilderFactory,
            IRoomUpdatingRepository repository)
        {
            this.validator = validator;
            this.readingService = readingService;
            this.intentionManager = intentionManager;
            this.updateBuilderFactory = updateBuilderFactory;
            this.repository = repository;
        }

        /// <inheritdoc />
        public async Task<Room> Update(UpdateRoom updateRoom)
        {
            await validator.ValidateAndThrowAsync(updateRoom);
            var room = await readingService.Get(updateRoom.RoomId);
            await intentionManager.ThrowIfForbidden(GameIntention.AdministrateRooms, room.Game);

            var roomUpdate = updateBuilderFactory.Create<DbRoom>(updateRoom.RoomId)
                .MaybeField(r => r.Title, updateRoom.Title)
                .MaybeField(r => r.Type, updateRoom.Type)
                .MaybeField(r => r.AccessType, updateRoom.AccessType);

            if (updateRoom.PreviousRoomId == room.PreviousRoomId)
            {
                return await repository.Update(roomUpdate);
            }

            if (!updateRoom.PreviousRoomId.HasValue)
            {
                return await InsertFirst(room, roomUpdate);
            }

            var targetRoom = await readingService.Get(updateRoom.PreviousRoomId.Value);
            if (targetRoom.Game.Id != room.Game.Id)
            {
                throw new HttpBadRequestException(new Dictionary<string, string>
                {
                    [nameof(Room.PreviousRoomId)] = ValidationError.Invalid
                });
            }

            return await InsertAfter(room, targetRoom, roomUpdate);
        }

        private async Task<Room> InsertAfter(Room room, Room afterRoom, IUpdateBuilder<DbRoom> updateRoom)
        {
            var (updateOldPreviousRoom, updateOldNextRoom) = await Pull(room);
            var updateNewPreviousRoom = updateBuilderFactory.Create<DbRoom>(afterRoom.Id)
                .Field(r => r.NextRoomId, room.Id);
            var updateNewNextRoom = afterRoom.NextRoomId.HasValue
                ? updateBuilderFactory.Create<DbRoom>(afterRoom.NextRoomId.Value)
                    .Field(r => r.PreviousRoomId, room.Id)
                : null;
            updateRoom
                .Field(r => r.PreviousRoomId, afterRoom.Id)
                .MaybeField(r => r.NextRoomId, afterRoom.NextRoomId)
                .Field(r => r.OrderNumber, afterRoom.NextRoomOrderNumber.HasValue
                    ? (afterRoom.NextRoomOrderNumber + afterRoom.OrderNumber) / 2
                    : afterRoom.OrderNumber + 1);

            return await repository.Update(updateRoom,
                updateOldNextRoom, updateOldPreviousRoom, updateNewNextRoom, updateNewPreviousRoom);
        }

        private async Task<Room> InsertFirst(Room room, IUpdateBuilder<DbRoom> updateRoom)
        {
            var (updateOldPreviousRoom, updateOldNextRoom) = await Pull(room);
            var firstRoomInfo = await repository.GetFirstRoomInfo(room.Game.Id);
            var updateNewNextRoom = updateBuilderFactory.Create<DbRoom>(firstRoomInfo.RoomId)
                .Field(r => r.PreviousRoomId, room.Id);
            updateRoom
                .Field(r => r.NextRoomId, firstRoomInfo.RoomId)
                .Field(r => r.OrderNumber, firstRoomInfo.OrderNumber - 1);

            return await repository.Update(updateRoom, updateOldNextRoom, updateOldPreviousRoom, updateNewNextRoom);
        }

        private async Task<(IUpdateBuilder<DbRoom> updateOldPrevious, IUpdateBuilder<DbRoom> updateOldNext)> Pull(
            Room room)
        {
            var roomOldNeighbours = await repository.GetNeighbours(room.Id);

            var updateOldPreviousRoom = roomOldNeighbours.PreviousRoom == null
                ? null
                : updateBuilderFactory.Create<DbRoom>(roomOldNeighbours.PreviousRoom.RoomId)
                    .Field(r => r.NextRoomId, room.NextRoomId);
            var updateOldNextRoom = roomOldNeighbours.NextRoom == null
                ? null
                : updateBuilderFactory.Create<DbRoom>(roomOldNeighbours.NextRoom.RoomId)
                    .Field(r => r.PreviousRoomId, room.PreviousRoomId);
            return (updateOldPreviousRoom, updateOldNextRoom);
        }
    }
}