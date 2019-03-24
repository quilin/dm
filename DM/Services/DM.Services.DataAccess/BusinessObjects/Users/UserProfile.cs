using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.DataAccess.BusinessObjects.DataContracts;

namespace DM.Services.DataAccess.BusinessObjects.Users
{
    [Table("UserDatas")]
    public class UserProfile : IRemovable
    {
        [Key] public Guid UserProfileId { get; set; }
        public Guid UserId { get; set; }

        public string Status { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Icq { get; set; }
        public string Skype { get; set; }
        public bool ShowEmail { get; set; }
        public string Info { get; set; }

        public bool IsRemoved { get; set; }

        [ForeignKey(nameof(UserId))] public virtual User User { get; set; }
    }
}