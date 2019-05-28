using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.DataAccess.BusinessObjects.Games;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.DataAccess.BusinessObjects.Common
{
    /// <summary>
    /// DAL model for like
    /// </summary>
    [Table("Likes")]
    public class Like
    {
        /// <summary>
        /// Like identifier
        /// </summary>
        [Key]
        public Guid LikeId { get; set; }

        /// <summary>
        /// Parent entity identifier (e.g. commentary, topic, etc.)
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// Author identifier
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Author
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        /// <summary>
        /// Parent forum commentary
        /// </summary>
        [ForeignKey(nameof(EntityId))]
        public virtual ForumComment ForumComment { get; set; }

        /// <summary>
        /// Parent topic
        /// </summary>
        [ForeignKey(nameof(EntityId))]
        public virtual ForumTopic Topic { get; set; }

        /// <summary>
        /// Parent review
        /// </summary>
        [ForeignKey(nameof(EntityId))]
        public virtual Review Review { get; set; }
        
        /// <summary>
        /// Parent game commentary
        /// </summary>
        [ForeignKey(nameof(EntityId))]
        public virtual GameComment GameComment { get; set; }
    }
}