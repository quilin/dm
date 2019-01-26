using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.DataAccess.BusinessObjects.DataContracts;
using DM.Services.DataAccess.BusinessObjects.Games.Characters;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.DataAccess.BusinessObjects.Games.Posts
{
    [Table("Posts")]
    public class Post : IRemovable
    {
        [Key]
        public Guid PostId { get; set; }

        public Guid RoomId { get; set; }
        public Guid CharacterId { get; set; }
        public Guid UserId { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }

        public string Text { get; set; }
        public string Commentary { get; set; }
        public string MasterMessage { get; set; }

        public bool IsRemoved { get; set; }

        [ForeignKey(nameof(RoomId))]
        public Room Room { get; set; }

        [ForeignKey(nameof(CharacterId))]
        public Character Character { get; set; }

        [ForeignKey(nameof(UserId))]
        public User Author { get; set; }
        
        [InverseProperty(nameof(Vote.Post))]
        public ICollection<Vote> Votes { get; set; }
    }
}