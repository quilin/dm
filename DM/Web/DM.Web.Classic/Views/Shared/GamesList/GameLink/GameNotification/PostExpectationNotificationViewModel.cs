using System;
using System.Collections.Generic;
using DM.Web.Classic.Views.Shared.User;

namespace DM.Web.Classic.Views.Shared.GamesList.GameLink.GameNotification
{
    public class PostExpectationNotificationViewModel
    {
        public Guid RoomId { get; set; }
        public IEnumerable<UserViewModel> Authors { get; set; }
    }
}