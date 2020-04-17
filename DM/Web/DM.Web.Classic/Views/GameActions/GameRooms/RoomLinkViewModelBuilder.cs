using System;
using DM.Services.Gaming.Dto.Output;
using DM.Web.Classic.Views.Shared.GamesList.GameLink.GameNotification;

namespace DM.Web.Classic.Views.GameActions.GameRooms
{
    public class RoomLinkViewModelBuilder : IRoomLinkViewModelBuilder
    {
        private readonly IPostExpectationNotificationViewModelBuilder postExpectationNotificationViewModelBuilder;

        public RoomLinkViewModelBuilder(
            IPostExpectationNotificationViewModelBuilder postExpectationNotificationViewModelBuilder)
        {
            this.postExpectationNotificationViewModelBuilder = postExpectationNotificationViewModelBuilder;
        }

        public RoomLinkViewModel Build(Room room, PageType pageType, Guid? pageId)
        {
            var postExpectationNotificationViewModel = postExpectationNotificationViewModelBuilder.Build(room);

            return new RoomLinkViewModel
            {
                RoomId = room.Id,
                Title = room.Title,
                Disabled = pageType == PageType.Session && pageId == room.Id,
                HasNotification = postExpectationNotificationViewModel != null,
                Notification = postExpectationNotificationViewModel,
                UnreadCount = room.UnreadPostsCount
            };
        }
    }
}