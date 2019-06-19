using DM.Web.Classic.Views.RoomsList.Character;
using DM.Web.Classic.Views.RoomsList.CreateRoom;
using DM.Web.Classic.Views.RoomsList.Room;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.RoomControllers
{
    public class CreateRoomController : DmControllerBase
    {
        private readonly IRoomService roomService;
        private readonly IRoomViewModelBuilder roomViewModelBuilder;

        public CreateRoomController(
            IRoomService roomService,
            IRoomViewModelBuilder roomViewModelBuilder)
        {
            this.roomService = roomService;
            this.roomViewModelBuilder = roomViewModelBuilder;
        }

        [HttpPost, ValidationRequired]
        public ActionResult Create(CreateRoomForm createRoomForm)
        {
            var room = roomService.Create(createRoomForm.ModuleId, createRoomForm.Title);
            return PartialView("~/Views/RoomsList/Room/Room.cshtml", roomViewModelBuilder.Build(room, new CharacterRoomLink[0], new CharacterViewModel[0]));
        }
    }
}