using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.BusinessObjects.DataContracts;
using DM.Services.DataAccess.BusinessObjects.Games.Characters;
using DM.Services.DataAccess.BusinessObjects.Games.Links;
using DM.Services.DataAccess.BusinessObjects.Games.Posts;
using DM.Services.DataAccess.BusinessObjects.Games.Rating;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.DataAccess.BusinessObjects.Games
{
    [Table("Modules")]
    public class Game : IRemovable
    {
        [Key]
        public Guid ModuleId { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public GameStatus Status { get; set; }

        public Guid MasterId { get; set; }
        public Guid? AssistantId { get; set; }
        public Guid? NannyId { get; set; }

        public Guid AttributeSchemeId { get; set; }

        public string Title { get; set; }
        public string SystemName { get; set; }
        public string SettingName { get; set; }
        public string Info { get; set; }

        public bool HideTemper { get; set; }
        public bool HideSkills { get; set; }
        public bool HideInventory { get; set; }
        public bool HideStory { get; set; }
        public bool DisableAlignment { get; set; }
        public bool HideDiceResult { get; set; }
        public bool ShowPrivateMessages { get; set; }

        public CommentariesAccessMode CommentariesAccessMode { get; set; }
        public string Notepad { get; set; }

        public bool IsRemoved { get; set; }

        [ForeignKey(nameof(MasterId))]
        public User Master { get; set; }

        [ForeignKey(nameof(AssistantId))]
        public User Assistant { get; set; }

        [ForeignKey(nameof(NannyId))]
        public User Nanny { get; set; }

        [InverseProperty(nameof(BlackListLink.Game))]
        public virtual ICollection<BlackListLink> BlackList { get; set; }

        [InverseProperty(nameof(GameTag.Game))]
        public virtual ICollection<GameTag> GameTags { get; set; }

        [InverseProperty(nameof(Reader.Game))]
        public virtual ICollection<Reader> Readers { get; set; }

        [InverseProperty(nameof(Character.Game))]
        public virtual ICollection<Character> Characters { get; set; }

        [InverseProperty(nameof(Room.Game))]
        public virtual ICollection<Room> Rooms { get; set; }

        [InverseProperty(nameof(Vote.Game))]
        public virtual ICollection<Vote> Votes { get; set; }

        [InverseProperty(nameof(Comment.Game))]
        public virtual ICollection<Comment> Comments { get; set; }

        [InverseProperty(nameof(Upload.Game))]
        public virtual ICollection<Upload> Pictures { get; set; }
    }
}