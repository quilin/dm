using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.DataAccess.BusinessObjects.DataContracts;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.DataAccess.BusinessObjects.Administration
{
    [Table("Bans")]
    public class Ban : IAdministrated
    {
        [Key]
        public Guid BanId { get; set; }

        public Guid UserId { get; set; }
        public Guid ModeratorId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Comment { get; set; }
        public AccessPolicy AccessRestrictionPolicy { get; set; }
        public bool IsVoluntary { get; set; }

        public bool IsRemoved { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        [ForeignKey(nameof(ModeratorId))]
        public virtual User Moderator { get; set; }
    }
}