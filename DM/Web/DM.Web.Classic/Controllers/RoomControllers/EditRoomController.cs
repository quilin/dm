using System;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;
using DM.Services.Gaming.BusinessProcesses.Rooms.Reading;
using DM.Services.Gaming.BusinessProcesses.Rooms.Updating;
using DM.Services.Gaming.Dto.Input;
using DM.Services.Gaming.Dto.Output;
using DM.Web.Classic.Middleware;
using DM.Web.Classic.Views.EditRoom;
using DM.Web.Classic.Views.RoomsList.Character;
using DM.Web.Classic.Views.RoomsList.Room;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.RoomControllers
{
    public class EditRoomController : DmControllerBase
    {
        private readonly IRoomReadingService roomReadingService;
        private readonly IRoomUpdatingService roomUpdatingService;
        private readonly IGameReadingService gameReadingService;
        private readonly IEditRoomFormBuilder editRoomFormBuilder;
        private readonly IRoomViewModelBuilder roomViewModelBuilder;
        private readonly IIntentionManager intentionManager;

        public EditRoomController(
            IRoomReadingService roomReadingService,
            IRoomUpdatingService roomUpdatingService,
            IGameReadingService gameReadingService,
            IEditRoomFormBuilder editRoomFormBuilder,
            IRoomViewModelBuilder roomViewModelBuilder,
            IIntentionManager intentionManager)
        {
            this.roomReadingService = roomReadingService;
            this.roomUpdatingService = roomUpdatingService;
            this.gameReadingService = gameReadingService;
            this.editRoomFormBuilder = editRoomFormBuilder;
            this.roomViewModelBuilder = roomViewModelBuilder;
            this.intentionManager = intentionManager;
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid roomId)
        {
            var room = await roomReadingService.Get(roomId);
            var game = await gameReadingService.GetGame(room.GameId);
            await intentionManager.ThrowIfForbidden(GameIntention.AdministrateRooms, game);
            var editRoomForm = editRoomFormBuilder.Build(room);
            return View("~/Views/EditRoom/EditRoom.cshtml", editRoomForm);
        }

        [HttpPost, ValidationRequired]
        public async Task<IActionResult> Edit(EditRoomForm editRoomForm)
        {
            var room = await roomUpdatingService.Update(new UpdateRoom
            {
                RoomId = editRoomForm.RoomId,
                Title = editRoomForm.RoomTitle,
                Type = editRoomForm.RoomType,
                AccessType = editRoomForm.RoomAccess,
                PreviousRoomId = null
            });

            var roomViewModel = roomViewModelBuilder.Build(room,
                Enumerable.Empty<RoomClaim>(), Enumerable.Empty<CharacterViewModel>());

            return PartialView("~/Views/RoomsList/Room/RoomTitle.cshtml", roomViewModel);
        }
    }
}