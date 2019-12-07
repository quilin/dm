using System;
using DM.Services.Core.Dto.Enums;
using DM.Services.Gaming.Dto.Output;
using DM.Web.Classic.Views.GameInfo.Characters;
using DM.Web.Classic.Views.Shared.User;

namespace DM.Web.Classic.Views.GameInfo
{
    public class GameViewModel
    {
        public Guid GameId { get; set; }
        public GameStatus Status { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset? ReleaseDate { get; set; }

        public string Title { get; set; }
        public string SystemName { get; set; }
        public string SettingName { get; set; }
        public string Info { get; set; }

        public UserViewModel Master { get; set; }
        public UserViewModel Assistant { get; set; }

        public UserViewModel[] Readers { get; set; }
        public GameTag[] Tags { get; set; }

        public int ActiveCharactersCount { get; set; }
        public int RegisteredCharactersCount { get; set; }
        public CharacterViewModel[] Characters { get; set; }
        public CharacterViewModel[] Npcs { get; set; }

        public bool CanRemove { get; set; }
    }
}