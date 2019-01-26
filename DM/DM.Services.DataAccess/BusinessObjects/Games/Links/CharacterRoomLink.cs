using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.DataAccess.BusinessObjects.Games.Characters;
using DM.Services.DataAccess.BusinessObjects.Games.Posts;

namespace DM.Services.DataAccess.BusinessObjects.Games.Links
{
    [Table("CharacterRoomLinks")]
    public class CharacterRoomLink
    {
        [Key]
        public Guid CharacterRoomLinkId { get; set; }

        public Guid CharacterId { get; set; }
        public Guid RoomId { get; set; }

        [ForeignKey(nameof(CharacterId))]
        public Character Character { get; set; }

        [ForeignKey(nameof(RoomId))]
        public Room Room { get; set; }
    }
}