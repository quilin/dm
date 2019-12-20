using System;
using System.Collections.Generic;
using DM.Services.Gaming.Dto.Output;

namespace DM.Web.Classic.Views.GameActions.GameRooms
{
    public class RoomLinkViewModel
    {
        public Guid RoomId { get; set; }
        public string Title { get; set; }
        public bool Disabled { get; set; }
        public IEnumerable<PendingPost> PendingPosts { get; set; }

        public int UnreadCount { get; set; }
        public int TotalCount { get; set; }
    }
}