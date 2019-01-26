using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.DataAccess.BusinessObjects.Fora
{
    [Table("ForumModerators")]
    public class ForumModerator
    {
        [Key]
        public Guid ForumModeratorId { get; set; }
        public Guid ForumId { get; set; }
        public Guid UserId { get; set; }

        [ForeignKey(nameof(ForumId))]
        public virtual Forum Forum { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
    }
}