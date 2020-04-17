using System;
using DM.Web.Classic.Views.Shared.GamesList.GameLink.GameNotification;

namespace DM.Web.Classic.Views.GameActions.GameRooms
{
    public class RoomLinkViewModel
    {
        public Guid RoomId { get; set; }
        public string Title { get; set; }
        public bool Disabled { get; set; }

        public bool HasNotification { get; set; }
        public PostExpectationNotificationViewModel Notification { get; set; }

        public int UnreadCount { get; set; }
    }
}