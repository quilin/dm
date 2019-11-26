using System;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.Gaming.Dto.Input
{
    /// <summary>
    /// DTO model for character updating
    /// </summary>
    public class UpdateCharacter
    {
        /// <summary>
        /// Character identifier
        /// </summary>
        public Guid CharacterId { get; set; }

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
        /// Character alignment
        /// </summary>
        public Alignment? Alignment { get; set; }

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
        /// Character is NPC
        /// </summary>
        public bool? IsNpc { get; set; }

        /// <summary>
        /// Character access policy
        /// </summary>
        public CharacterAccessPolicy? AccessPolicy { get; set; }
    }
}