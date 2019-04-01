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
    /// <summary>
    /// DAL model for commentary
    /// </summary>
    [Table("Comments")]
    public class Comment : IRemovable
    {
        /// <summary>
        /// Commentary identifier
        /// </summary>
        [Key]
        public Guid CommentId { get; set; }

        /// <summary>
        /// Parent entity identifier (e.g. forum topic, game, etc.)
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// Author identifier
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Creation moment
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Last update moment
        /// </summary>
        public DateTime? LastUpdateDate { get; set; }

        /// <summary>
        /// Commentary content
        /// </summary>
        public string Text { get; set; }

        /// <inheritdoc />
        public bool IsRemoved { get; set; }

        /// <summary>
        /// Parent topic
        /// </summary>
        [ForeignKey(nameof(EntityId))]
        public virtual ForumTopic Topic { get; set; }

        /// <summary>
        /// Parent game
        /// </summary>
        [ForeignKey(nameof(EntityId))]
        public virtual Game Game { get; set; }

        /// <summary>
        /// Author
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public virtual User Author { get; set; }

        /// <summary>
        /// Likes
        /// </summary>
        [InverseProperty(nameof(Like.Comment))]
        public virtual ICollection<Like> Likes { get; set; }

        /// <summary>
        /// Administrative warnings
        /// </summary>
        [InverseProperty(nameof(Warning.Comment))]
        public virtual ICollection<Warning> Warnings { get; set; }
    }
}