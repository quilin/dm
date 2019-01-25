using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.DataAccess.BusinessObjects.Fora;

namespace DM.Services.DataAccess.BusinessObjects.Common
{
    [Table("Likes")]
    public class Like
    {
        [Key]
        public Guid LikeId { get; set; }

        public Guid EntityId { get; set; }
        public Guid UserId { get; set; }

        [ForeignKey(nameof(EntityId))]
        public Comment Comment { get; set; }

        [ForeignKey(nameof(EntityId))]
        public ForumTopic Topic { get; set; }
    }
}