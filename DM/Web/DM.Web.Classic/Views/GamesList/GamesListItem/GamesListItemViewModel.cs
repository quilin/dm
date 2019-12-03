using System;
using System.Collections.Generic;
using DM.Services.Core.Dto.Enums;
using DM.Services.Gaming.Dto.Output;
using DM.Web.Classic.Views.Shared.User;

namespace DM.Web.Classic.Views.GamesList.GamesListItem
{
    public class GamesListItemViewModel
    {
        public int Number { get; set; }

        public Guid GameId { get; set; }

        public string Title { get; set; }
        public string SystemName { get; set; }
        public string SettingName { get; set; }
        public GameStatus Status { get; set; }
        public UserViewModel Master { get; set; }

        public int UnreadCountPosts { get; set; }
        public int UnreadCountComments { get; set; }

        public IEnumerable<GameTag> Tags { get; set; }
        public Guid? SearchedTagId { get; set; }
    }
}