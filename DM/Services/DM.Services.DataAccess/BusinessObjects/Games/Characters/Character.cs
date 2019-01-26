using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.BusinessObjects.DataContracts;
using DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes;
using DM.Services.DataAccess.BusinessObjects.Games.Links;
using DM.Services.DataAccess.BusinessObjects.Games.Posts;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.DataAccess.BusinessObjects.Games.Characters
{
    [Table("Characters")]
    public class Character : IRemovable
    {
        [Key]
        public Guid CharacterId { get; set; }

        public Guid GameId { get; set; }
        public Guid UserId { get; set; }

        public CharacterStatus Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }

        public string Name { get; set; }
        public string Race { get; set; }
        public string Class { get; set; }
        public Alignment Alignment { get; set; }

        public string Appearance { get; set; }
        public string Temper { get; set; }
        public string Story { get; set; }
        public string Skills { get; set; }
        public string Inventory { get; set; }

        public bool IsNpc { get; set; }
        public CharacterAccessPolicy AccessPolicy { get; set; }

        public bool IsRemoved { get; set; }

        [ForeignKey(nameof(GameId))]
        public virtual Game Game { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        [InverseProperty(nameof(Upload.Character))]
        public virtual ICollection<Upload> Pictures { get; set; }

        [InverseProperty(nameof(CharacterAttribute.Character))]
        public virtual ICollection<CharacterAttribute> Attributes { get; set; }

        [InverseProperty(nameof(CharacterRoomLink.Character))]
        public virtual ICollection<CharacterRoomLink> RoomLinks { get; set; }

        [InverseProperty(nameof(Post.Character))]
        public virtual ICollection<Post> Posts { get; set; }
    }
}