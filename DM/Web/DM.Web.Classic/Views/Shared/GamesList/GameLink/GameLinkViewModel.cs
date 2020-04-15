using System;
using DM.Services.Core.Dto.Enums;
using DM.Web.Classic.Views.Shared.GamesList.GameLink.GameNotification;
using DM.Web.Classic.Views.Shared.User;

namespace DM.Web.Classic.Views.Shared.GamesList.GameLink
{
    public class GameLinkViewModel
    {
        public Guid GameId { get; set; }
        public string Title { get; set; }
        public UserViewModel Master { get; set; }
        public string SystemName { get; set; }
        public string SettingName { get; set; }
        public GameStatus Status { get; set; }
        public bool IsNew { get; set; }

        public bool HasPostNotification { get; set; }
        public PostExpectationNotificationViewModel Notification { get; set; }

        public bool HasUnreadCounters { get; set; }
        public int UnreadPostsCount { get; set; }
        public int UnreadCommentsCount { get; set; }
        public int UnreadCharactersCount { get; set; }

        public bool IsCurrentGame { get; set; }
    }
}