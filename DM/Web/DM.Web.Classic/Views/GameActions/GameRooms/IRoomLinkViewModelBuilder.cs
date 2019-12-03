using System;
using DtoRoom = DM.Services.Gaming.Dto.Output.Room;

namespace DM.Web.Classic.Views.GameActions.GameRooms
{
    public interface IRoomLinkViewModelBuilder
    {
        RoomLinkViewModel Build(DtoRoom room, PageType pageType, Guid? pageId);
    }
}