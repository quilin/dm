using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.DataAccess.BusinessObjects.Games.Posts;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.DataAccess.BusinessObjects.Games.Links
{
    /// <summary>
    /// DAL model for post anticipation (when users await one another in same room)
    /// </summary>
    [Table("PostWaitNotifications")]
    public class PostAnticipation
    {
        /// <summary>
        /// Anticipation identifier
        /// </summary>
        [Key]
        public Guid PostAnticipationId { get; set; }

        /// <summary>
        /// Waiting user identifier
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// User that should write a post identifier
        /// </summary>
        public Guid TargetId { get; set; }

        /// <summary>
        /// Room identifier
        /// </summary>
        public Guid RoomId { get; set; }

        /// <summary>
        /// Creation moment
        /// </summary>
        public DateTimeOffset CreateDate { get; set; }

        /// <summary>
        /// Waiting user
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        /// <summary>
        /// User that should write a post
        /// </summary>
        [ForeignKey(nameof(TargetId))]
        public virtual User Target { get; set; }

        /// <summary>
        /// Room
        /// </summary>
        [ForeignKey(nameof(RoomId))]
        public virtual Room Room { get; set; }
    }
}