using DM.Services.Core.Dto.Enums;

namespace DM.Services.Game.Dto.Input
{
    /// <summary>
    /// DTO model for new game
    /// </summary>
    public class CreateGame
    {
        /// <summary>
        /// Game title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Game RPG system
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// Game RPG setting
        /// </summary>
        public string SettingName { get; set; }

        /// <summary>
        /// Game public information
        /// </summary>
        public string Info { get; set; }

        /// <summary>
        /// Game assistant login
        /// </summary>
        public string AssistantLogin { get; set; }

        /// <summary>
        /// Only GM and character author can see character temper
        /// </summary>
        public bool HideTemper { get; set; }

        /// <summary>
        /// Only GM and character author can see character skills
        /// </summary>
        public bool HideSkills { get; set; }

        /// <summary>
        /// Only GM and character author can see character inventory
        /// </summary>
        public bool HideInventory { get; set; }

        /// <summary>
        /// Only GM and character author can see character story
        /// </summary>
        public bool HideStory { get; set; }

        /// <summary>
        /// Characters has no alignment
        /// </summary>
        public bool DisableAlignment { get; set; }

        /// <summary>
        /// Only GM and post author can see dice roll result
        /// </summary>
        public bool HideDiceResult { get; set; }

        /// <summary>
        /// Desired game status
        /// </summary>
        public GameStatus Status { get; set; }
    }
}