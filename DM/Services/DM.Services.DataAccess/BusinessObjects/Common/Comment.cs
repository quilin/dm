using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.DataAccess.BusinessObjects.Administration;
using DM.Services.DataAccess.BusinessObjects.DataContracts;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.DataAccess.BusinessObjects.Games;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.DataAccess.BusinessObjects.Common
{
    [Table("Comments")]
    public class Comment : IRemovable
    {
        [Key] public Guid CommentId { get; set; }

        public Guid EntityId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }

        public string Text { get; set; }

        public bool IsRemoved { get; set; }

        [ForeignKey(nameof(EntityId))] public virtual ForumTopic Topic { get; set; }

        [ForeignKey(nameof(EntityId))] public virtual Game Game { get; set; }

        [ForeignKey(nameof(UserId))] public virtual User Author { get; set; }

        [InverseProperty(nameof(Like.Comment))]
        public virtual ICollection<Like> Likes { get; set; }

        [InverseProperty(nameof(Warning.Comment))]
        public virtual ICollection<Warning> Warnings { get; set; }
    }
}