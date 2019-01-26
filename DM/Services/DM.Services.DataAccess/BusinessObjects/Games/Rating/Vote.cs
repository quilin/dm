using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.DataAccess.BusinessObjects.Games.Posts;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.DataAccess.BusinessObjects.Games.Rating
{
    [Table("Votes")]
    public class Vote
    {
        [Key]
        public Guid VoteId { get; set; }

        public Guid PostId { get; set; }
        public Guid GameId { get; set; }
        public Guid UserId { get; set; }
        public Guid TargetUserId { get; set; }

        public DateTime CreateDate { get; set; }
        public VoteType Type { get; set; }
        public short SignValue { get; set; }
        public VoteSign Sign => (VoteSign) SignValue;

        [ForeignKey(nameof(PostId))]
        public virtual Post Post { get; set; }

        [ForeignKey(nameof(GameId))]
        public virtual Game Game { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User VotedUser { get; set; }

        [ForeignKey(nameof(TargetUserId))]
        public virtual User TargetUser { get; set; }
    }
}