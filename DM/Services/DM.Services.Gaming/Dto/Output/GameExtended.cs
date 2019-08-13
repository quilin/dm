using System.Collections.Generic;

namespace DM.Services.Gaming.Dto.Output
{
    /// <summary>
    /// Extended DTO model for game
    /// </summary>
    public class GameExtended : Game
    {
        /// <summary>
        /// Game tags
        /// </summary>
        public IEnumerable<string> Tags { get; set; }

        /// <summary>
        /// Game public information
        /// </summary>
        public string Info { get; set; }

        /// <summary>
        /// Game private master information
        /// </summary>
        public string Notepad { get; set; }

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
        /// Only GM and post author can see dice roll result
        /// </summary>
        public bool HideDiceResult { get; set; }

        /// <summary>
        /// Any user can read private messages within posts
        /// </summary>
        public bool ShowPrivateMessages { get; set; }
    }
}