using System;
using DM.Services.Core.Dto.Enums;

namespace DM.Web.Classic.Views.Shared.GameList.GameLink
{
    public class GameLinkViewModel
    {
        public Guid GameId { get; set; }
        public string Title { get; set; }
        public string MasterLogin { get; set; }
        public string SystemName { get; set; }
        public string SettingName { get; set; }
        public GameStatus Status { get; set; }
        public bool IsNew { get; set; }

        public bool HasUnreadCounters { get; set; }
        public int UnreadCountPosts { get; set; }
        public int UnreadCountComments { get; set; }
        public bool HasUnseenCharacterCounters { get; set; }
        public int UnseenCountCharacters { get; set; }

        public bool IsCurrentModule { get; set; }
    }
}