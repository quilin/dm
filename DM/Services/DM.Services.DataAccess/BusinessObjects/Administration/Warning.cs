using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.DataAccess.BusinessObjects.Administration
{
    [Table("Warnings")]
    public class Warning : IAdministrated
    {
        [Key] public Guid WarningId { get; set; }

        public Guid UserId { get; set; }
        public Guid ModeratorId { get; set; }
        public Guid EntityId { get; set; }

        public DateTime CreateDate { get; set; }

        public string Text { get; set; }
        public int Points { get; set; }

        public bool IsRemoved { get; set; }

        [ForeignKey(nameof(UserId))] public virtual User User { get; set; }

        [ForeignKey(nameof(ModeratorId))] public virtual User Moderator { get; set; }

        [ForeignKey(nameof(EntityId))] public virtual Comment Comment { get; set; }
    }
}