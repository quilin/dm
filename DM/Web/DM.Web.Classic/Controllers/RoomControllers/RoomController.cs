using System;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Services.Gaming.BusinessProcesses.Rooms.Deleting;
using DM.Services.Gaming.BusinessProcesses.Rooms.Updating;
using DM.Services.Gaming.Dto.Input;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.RoomControllers
{
    public class RoomController : DmControllerBase
    {
        private readonly IRoomUpdatingService roomUpdatingService;
        private readonly IRoomDeletingService roomDeletingService;

        public RoomController(
            IRoomUpdatingService roomUpdatingService,
            IRoomDeletingService roomDeletingService)
        {
            this.roomUpdatingService = roomUpdatingService;
            this.roomDeletingService = roomDeletingService;
        }

        public Task Remove(Guid roomId) => roomDeletingService.Delete(roomId);

        [HttpPost]
        public Task InsertFirst(Guid movedRoomId) => roomUpdatingService.Update(new UpdateRoom
        {
            RoomId = movedRoomId,
            PreviousRoomId = Optional<Guid>.WithValue(null)
        });

        [HttpPost]
        public Task InsertAfter(Guid movedRoomId, Guid roomId) => roomUpdatingService.Update(new UpdateRoom
        {
            RoomId = movedRoomId,
            PreviousRoomId = Optional<Guid>.WithValue(roomId)
        });
    }
}