using System;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.Gaming.BusinessProcesses.Characters.Reading;
using DM.Services.Gaming.BusinessProcesses.Claims.Reading;
using DM.Services.Gaming.BusinessProcesses.Rooms.Reading;
using DM.Web.Classic.Views.RoomsList.Character;
using DM.Web.Classic.Views.RoomsList.CreateRoom;
using DM.Web.Classic.Views.RoomsList.Room;

namespace DM.Web.Classic.Views.RoomsList
{
    public class RoomsListViewModelBuilder : IRoomsListViewModelBuilder
    {
        private readonly IRoomReadingService roomService;
        private readonly IRoomClaimsReadingService roomClaimsService;
        private readonly ICharacterReadingService characterService;
        private readonly IRoomViewModelBuilder roomViewModelBuilder;
        private readonly ICreateRoomFormBuilder createRoomFormBuilder;
        private readonly ICharacterViewModelBuilder characterViewModelBuilder;

        public RoomsListViewModelBuilder(
            IRoomReadingService roomService,
            IRoomClaimsReadingService roomClaimsService,
            ICharacterReadingService characterService,
            IRoomViewModelBuilder roomViewModelBuilder,
            ICreateRoomFormBuilder createRoomFormBuilder,
            ICharacterViewModelBuilder characterViewModelBuilder)
        {
            this.roomService = roomService;
            this.roomClaimsService = roomClaimsService;
            this.characterService = characterService;
            this.roomViewModelBuilder = roomViewModelBuilder;
            this.createRoomFormBuilder = createRoomFormBuilder;
            this.characterViewModelBuilder = characterViewModelBuilder;
        }

        public async Task<RoomsListViewModel> Build(Guid gameId)
        {
            var roomClaims = await roomClaimsService.GetGameClaims(gameId);
            var characters = (await characterService.GetCharacters(gameId))
                .Where(c => c.Status == CharacterStatus.Active)
                .Select(characterViewModelBuilder.Build)
                .ToArray();
            var rooms = await roomService.GetAll(gameId);

            return new RoomsListViewModel
            {
                GameId = gameId,
                Rooms = rooms
                    .Select(r => roomViewModelBuilder.Build(r, roomClaims, characters))
                    .ToArray(),
                Characters = characters,
                CreateRoomForm = createRoomFormBuilder.Build(gameId)
            };
        }
    }
}