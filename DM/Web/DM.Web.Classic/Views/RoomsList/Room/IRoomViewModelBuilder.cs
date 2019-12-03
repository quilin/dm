using System.Collections.Generic;
using DM.Services.Gaming.Dto.Output;
using DM.Web.Classic.Views.RoomsList.Character;
using DtoRoom = DM.Services.Gaming.Dto.Output.Room;

namespace DM.Web.Classic.Views.RoomsList.Room
{
    public interface IRoomViewModelBuilder
    {
        RoomViewModel Build(DtoRoom room, IEnumerable<RoomClaim> characterRoomLinks,
            IEnumerable<CharacterViewModel> characters);
    }
}