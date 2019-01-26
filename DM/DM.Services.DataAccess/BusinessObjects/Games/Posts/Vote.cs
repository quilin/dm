using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.DataAccess.BusinessObjects.Games.Rating;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.DataAccess.BusinessObjects.Games.Posts
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
        public Post Post { get; set; }

        [ForeignKey(nameof(GameId))]
        public Game Game { get; set; }

        [ForeignKey(nameof(UserId))]
        public User VotedUser { get; set; }

        [ForeignKey(nameof(TargetUserId))]
        public User TargetUser { get; set; }
    }
}