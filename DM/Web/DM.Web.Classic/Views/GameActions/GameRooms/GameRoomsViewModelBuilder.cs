using System;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Gaming.BusinessProcesses.Rooms.Reading;
using DM.Services.Gaming.Dto.Output;

namespace DM.Web.Classic.Views.GameActions.GameRooms
{
    public class GameRoomsViewModelBuilder : IGameRoomsViewModelBuilder
    {
        private readonly IRoomReadingService roomService;
        private readonly IRoomLinkViewModelBuilder roomLinkViewModelBuilder;

        public GameRoomsViewModelBuilder(
            IRoomReadingService roomService,
            IRoomLinkViewModelBuilder roomLinkViewModelBuilder)
        {
            this.roomService = roomService;
            this.roomLinkViewModelBuilder = roomLinkViewModelBuilder;
        }

        public async Task<GameRoomsViewModel> Build(Guid gameId, PageType pageType, Guid? pageId)
        {
            var rooms = (await roomService.GetAll(gameId)).ToArray();
            return new GameRoomsViewModel
            {
                IsDefaultRoom = rooms.Length == 1 && rooms[0].Title == Room.DefaultRoomName,
                Rooms = rooms
                    .Select(r => roomLinkViewModelBuilder.Build(r, pageType, pageId))
                    .ToArray(),
                PageType = pageType
            };
        }
    }
}