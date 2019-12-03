using System;
using DtoRoom = DM.Services.Gaming.Dto.Output.Room;

namespace DM.Web.Classic.Views.GameActions.GameRooms
{
    public class RoomLinkViewModelBuilder : IRoomLinkViewModelBuilder
    {
        public RoomLinkViewModel Build(DtoRoom room, PageType pageType, Guid? pageId)
        {
            return new RoomLinkViewModel
            {
                RoomId = room.Id,
                Title = room.Title,
                Disabled = pageType == PageType.Session && pageId == room.Id,
                UnreadCount = room.UnreadPostsCount
            };
        }
    }
}