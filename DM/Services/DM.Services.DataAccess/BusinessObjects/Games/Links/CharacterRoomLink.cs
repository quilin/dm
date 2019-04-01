using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.DataAccess.BusinessObjects.Games.Characters;
using DM.Services.DataAccess.BusinessObjects.Games.Posts;

namespace DM.Services.DataAccess.BusinessObjects.Games.Links
{
    /// <summary>
    /// DAL model for character room link
    /// </summary>
    [Table("CharacterRoomLinks")]
    public class CharacterRoomLink
    {
        /// <summary>
        /// Link identifier
        /// </summary>
        [Key]
        public Guid CharacterRoomLinkId { get; set; }

        /// <summary>
        /// Character identifier
        /// </summary>
        public Guid CharacterId { get; set; }

        /// <summary>
        /// Room identifier
        /// </summary>
        public Guid RoomId { get; set; }

        /// <summary>
        /// Character
        /// </summary>
        [ForeignKey(nameof(CharacterId))]
        public virtual Character Character { get; set; }

        /// <summary>
        /// Room
        /// </summary>
        [ForeignKey(nameof(RoomId))]
        public virtual Room Room { get; set; }
    }
}