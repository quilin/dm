using System;
using DM.Services.Gaming.Dto.Output;

namespace DM.Web.Classic.Views.GameActions.GameRooms
{
    public interface IRoomLinkViewModelBuilder
    {
        RoomLinkViewModel Build(Room room, PageType pageType, Guid? pageId);
    }
}