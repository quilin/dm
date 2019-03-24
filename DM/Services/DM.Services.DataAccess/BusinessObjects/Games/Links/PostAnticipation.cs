using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.DataAccess.BusinessObjects.Games.Posts;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.DataAccess.BusinessObjects.Games.Links
{
    [Table("PostWaitNotifications")]
    public class PostAnticipation
    {
        [Key] public Guid PostAnticipationId { get; set; }

        public Guid UserId { get; set; }
        public Guid TargetId { get; set; }
        public Guid RoomId { get; set; }

        public DateTime CreateDate { get; set; }

        [ForeignKey(nameof(UserId))] public virtual User User { get; set; }

        [ForeignKey(nameof(TargetId))] public virtual User Target { get; set; }

        [ForeignKey(nameof(RoomId))] public virtual Room Room { get; set; }
    }
}