using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.BusinessObjects.DataContracts;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.DataAccess.BusinessObjects.Fora
{
    [Table("ForumTopics")]
    public class ForumTopic : IRemovable
    {
        [Key] public Guid ForumTopicId { get; set; }
        public Guid ForumId { get; set; }

        public Guid UserId { get; set; }
        public DateTime CreateDate { get; set; }

        public string Title { get; set; }
        public string Text { get; set; }

        public bool Attached { get; set; }
        public bool Closed { get; set; }

        public Guid? LastCommentId { get; set; }

        public bool IsRemoved { get; set; }

        [ForeignKey(nameof(ForumId))] public virtual Forum Forum { get; set; }

        [ForeignKey(nameof(UserId))] public virtual User Author { get; set; }

        [ForeignKey(nameof(LastCommentId))] public virtual Comment LastComment { get; set; }

        [InverseProperty(nameof(Comment.Topic))]
        public virtual ICollection<Comment> Comments { get; set; }

        [InverseProperty(nameof(Like.Topic))] public virtual ICollection<Like> Likes { get; set; }
    }
}