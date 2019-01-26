using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.DataAccess.BusinessObjects.DataContracts;
using DM.Services.DataAccess.BusinessObjects.Games;
using DM.Services.DataAccess.BusinessObjects.Games.Characters;
using DM.Services.DataAccess.BusinessObjects.Games.Posts;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.DataAccess.BusinessObjects.Common
{
    [Table("Uploads")]
    public class Upload : IRemovable
    {
        [Key]
        public Guid UploadId { get; set; }

        public DateTime CreateDate { get; set; }
        public Guid? EntityId { get; set; }
        public Guid UserId { get; set; }

        public string VirtualPath { get; set; }
        public string FileName { get; set; }

        public bool IsRemoved { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        [ForeignKey(nameof(EntityId))]
        public virtual User UserProfile { get; set; }

        [ForeignKey(nameof(EntityId))]
        public virtual Game Game { get; set; }

        [ForeignKey(nameof(EntityId))]
        public virtual Character Character { get; set; }

        [ForeignKey(nameof(EntityId))]
        public virtual Post Post { get; set; }
    }
}