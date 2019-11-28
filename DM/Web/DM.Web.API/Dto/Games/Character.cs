using DM.Services.Core.Dto.Enums;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Dto.Games
{
    /// <summary>
    /// DTO model for game character
    /// </summary>
    public class Character
    {
        /// <summary>
        /// Character identifier
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Character author
        /// </summary>
        public User Author { get; set; }

        /// <summary>
        /// Character status
        /// </summary>
        public CharacterStatus? Status { get; set; }

        /// <summary>
        /// Character name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Character race
        /// </summary>
        public string Race { get; set; }

        /// <summary>
        /// Character class
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// Character picture URL
        /// </summary>
        public string PictureUrl { get; set; }

        /// <summary>
        /// Character appearance
        /// </summary>
        public string Appearance { get; set; }

        /// <summary>
        /// Character temper
        /// </summary>
        public string Temper { get; set; }

        /// <summary>
        /// Character story
        /// </summary>
        public string Story { get; set; }

        /// <summary>
        /// Character skills
        /// </summary>
        public string Skills { get; set; }

        /// <summary>
        /// Character inventory
        /// </summary>
        public string Inventory { get; set; }

        /// <summary>
        /// Character privacy settings
        /// </summary>
        public CharacterPrivacySettings Privacy { get; set; }
    }

    /// <summary>
    /// DTO model for character privacy settings
    /// </summary>
    public class CharacterPrivacySettings
    {
        /// <summary>
        /// Character is non-player-character
        /// </summary>
        public bool IsNpc { get; set; }

        /// <summary>
        /// Character may be edited by master or assistant
        /// </summary>
        public bool EditByMaster { get; set; }

        /// <summary>
        /// Character's posts may be edited by master or assistant
        /// </summary>
        public bool EditPostByMaster { get; set; }
    }
}