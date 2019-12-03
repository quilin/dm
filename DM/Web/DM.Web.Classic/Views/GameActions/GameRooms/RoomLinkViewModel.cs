using System;

namespace DM.Web.Classic.Views.GameActions.GameRooms
{
    public class RoomLinkViewModel
    {
        public Guid RoomId { get; set; }
        public string Title { get; set; }
        public bool Disabled { get; set; }

        public int UnreadCount { get; set; }
    }
}