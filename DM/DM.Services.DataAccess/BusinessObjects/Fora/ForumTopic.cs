using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.DataAccess.BusinessObjects.Fora
{
    [Table("ForumTopics")]
    public class ForumTopic
    {
        [Key]
        public Guid ForumTopicId { get; set; }

        public Guid ForumId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreateDate { get; set; }

        public string Title { get; set; }
        public string Text { get; set; }

        public bool Attached { get; set; }
        public bool Closed { get; set; }

        [ForeignKey(nameof(ForumId))]
        public Forum Forum { get; set; }

        [ForeignKey(nameof(UserId))]
        public User Author { get; set; }

        [InverseProperty(nameof(Comment.Topic))]
        public ICollection<Comment> Comments { get; set; }

        [InverseProperty(nameof(Like.Topic))]
        public ICollection<Like> Likes { get; set; }
    }
}