using System;

namespace DM.Web.Classic.Views.GameSettings
{
    public class GameSettingsViewModel
    {
        public Guid GameId { get; set; }
        public string GameTitle { get; set; }
        public GameSettingsType DefaultSettings { get; set; }
    }
}