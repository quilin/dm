using System;
using DM.Services.Core.Dto;

namespace DM.Services.Gaming.Dto.Output
{
    /// <summary>
    /// Shortened character info
    /// </summary>
    public class CharacterShort
    {
        /// <summary>
        /// Identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Character owner
        /// </summary>
        public GeneralUser Author { get; set; }

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
        /// Picture URL
        /// </summary>
        public string PictureUrl { get; set; }
    }
}