using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.DataAccess.BusinessObjects.Common
{
    [Table("Likes")]
    public class Like
    {
        [Key]
        public Guid LikeId { get; set; }

        public Guid EntityId { get; set; }
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        [ForeignKey(nameof(EntityId))]
        public virtual Comment Comment { get; set; }

        [ForeignKey(nameof(EntityId))]
        public virtual ForumTopic Topic { get; set; }

        [ForeignKey(nameof(EntityId))]
        public virtual Review Review { get; set; }
    }
}