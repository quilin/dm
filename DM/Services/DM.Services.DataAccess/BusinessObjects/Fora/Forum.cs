using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.DataAccess.BusinessObjects.Fora
{
    [Table("Fora")]
    public class Forum
    {
        [Key]
        public Guid ForumId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public int Order { get; set; }

        public ForumAccessPolicy ViewPolicy { get; set; }
        public ForumAccessPolicy CreateTopicPolicy { get; set; }

        [InverseProperty(nameof(ForumModerator.Forum))]
        public virtual ICollection<ForumModerator> Moderators { get; set; }

        [InverseProperty(nameof(ForumTopic.Forum))]
        public virtual ICollection<ForumTopic> Topics { get; set; }
    }
}