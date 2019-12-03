using System.Collections.Generic;
using System.Linq;
using DM.Services.Gaming.Dto.Output;
using DM.Web.Classic.Views.RoomsList.Character;
using DM.Web.Classic.Views.RoomsList.Room.RoomActions;
using DtoRoom = DM.Services.Gaming.Dto.Output.Room;

namespace DM.Web.Classic.Views.RoomsList.Room
{
    public class RoomViewModelBuilder : IRoomViewModelBuilder
    {
        private readonly IRoomActionsViewModelBuilder roomActionsViewModelBuilder;

        public RoomViewModelBuilder(
            IRoomActionsViewModelBuilder roomActionsViewModelBuilder)
        {
            this.roomActionsViewModelBuilder = roomActionsViewModelBuilder;
        }

        public RoomViewModel Build(DtoRoom room, IEnumerable<RoomClaim> roomClaims,
            IEnumerable<CharacterViewModel> characters)
        {
            return new RoomViewModel
            {
                RoomId = room.Id,
                Title = room.Title,
                RoomType = room.Type,
                AccessType = room.AccessType,

                Characters = characters
                    .Where(c => roomClaims.Any(x => x.Character?.Id == c.CharacterId && x.RoomId == room.Id))
                    .ToArray(),
                RoomActions = roomActionsViewModelBuilder.Build(room)
            };
        }
    }
}