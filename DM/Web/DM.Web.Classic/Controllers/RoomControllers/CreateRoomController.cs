using System.Linq;
using System.Threading.Tasks;
using DM.Services.Gaming.BusinessProcesses.Rooms.Creating;
using DM.Services.Gaming.Dto.Input;
using DM.Services.Gaming.Dto.Output;
using DM.Web.Classic.Middleware;
using DM.Web.Classic.Views.RoomsList.Character;
using DM.Web.Classic.Views.RoomsList.CreateRoom;
using DM.Web.Classic.Views.RoomsList.Room;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.RoomControllers
{
    public class CreateRoomController : DmControllerBase
    {
        private readonly IRoomCreatingService roomService;
        private readonly IRoomViewModelBuilder roomViewModelBuilder;

        public CreateRoomController(
            IRoomCreatingService roomService,
            IRoomViewModelBuilder roomViewModelBuilder)
        {
            this.roomService = roomService;
            this.roomViewModelBuilder = roomViewModelBuilder;
        }

        [HttpPost, ValidationRequired]
        public async Task<IActionResult> Create(CreateRoomForm createRoomForm)
        {
            var room = await roomService.Create(new CreateRoom
            {
                GameId = createRoomForm.GameId,
                Title = createRoomForm.Title
            });
            var roomViewModel = roomViewModelBuilder.Build(
                room, Enumerable.Empty<RoomClaim>(), Enumerable.Empty<CharacterViewModel>());
            return PartialView("~/Views/RoomsList/Room/Room.cshtml", roomViewModel);
        }
    }
}