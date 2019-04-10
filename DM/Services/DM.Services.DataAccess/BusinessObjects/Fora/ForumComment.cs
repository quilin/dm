using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.DataAccess.BusinessObjects.Administration;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.BusinessObjects.DataContracts;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.DataAccess.BusinessObjects.Fora
{
    /// <summary>
    /// DAL model for commentary
    /// </summary>
    [Table("ForumComments")]
    public class ForumComment : IRemovable
    {
        /// <summary>
        /// Commentary identifier
        /// </summary>
        [Key]
        public Guid ForumCommentId { get; set; }

        /// <summary>
        /// Forum topic identifier
        /// </summary>
        public Guid ForumTopicId { get; set; }

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
        [ForeignKey(nameof(ForumTopicId))]
        public virtual ForumTopic Topic { get; set; }

        /// <summary>
        /// Author
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public virtual User Author { get; set; }

        /// <summary>
        /// Likes
        /// </summary>
        [InverseProperty(nameof(Like.ForumComment))]
        public virtual ICollection<Like> Likes { get; set; }

        /// <summary>
        /// Administrative warnings
        /// </summary>
        [InverseProperty(nameof(Warning.ForumComment))]
        public virtual ICollection<Warning> Warnings { get; set; }
    }
}