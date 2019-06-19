using System;
using DM.Web.Classic.Views.EditRoom;
using DM.Web.Classic.Views.RoomsList.Room;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.RoomControllers
{
    public class EditRoomController : DmControllerBase
    {
        private readonly IEditRoomFormBuilder editRoomFormBuilder;
        private readonly IRoomViewModelBuilder roomViewModelBuilder;
        private readonly IRoomService roomService;
        private readonly IModuleService moduleService;
        private readonly IIntentionsManager intentionsManager;

        public EditRoomController(
            IEditRoomFormBuilder editRoomFormBuilder,
            IRoomViewModelBuilder roomViewModelBuilder,
            IRoomService roomService,
            IModuleService moduleService,
            IIntentionsManager intentionsManager
            )
        {
            this.editRoomFormBuilder = editRoomFormBuilder;
            this.roomViewModelBuilder = roomViewModelBuilder;
            this.roomService = roomService;
            this.moduleService = moduleService;
            this.intentionsManager = intentionsManager;
        }

        [HttpGet]
        public ActionResult Edit(Guid roomId)
        {
            var room = roomService.ReadRoom(roomId);
            var module = moduleService.Read(room.ModuleId);
            intentionsManager.ThrowIfForbidden(RoomIntention.Edit, room, module);
            var editRoomForm = editRoomFormBuilder.Build(room);
            return View("~/Views/EditRoom/EditRoom.cshtml", editRoomForm);
        }

        [HttpPost, ValidationRequired]
        public ActionResult Edit(EditRoomForm editRoomForm)
        {
            var room = roomService.Update(editRoomForm.RoomId, editRoomForm.RoomTitle, editRoomForm.RoomAccess,
                editRoomForm.RoomType);
            var roomViewModel = roomViewModelBuilder.BuildForTitle(room);

            return PartialView("~/Views/RoomsList/Room/RoomTitle.cshtml", roomViewModel);
        }
    }
}