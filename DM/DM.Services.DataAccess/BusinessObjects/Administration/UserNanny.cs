using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.DataAccess.BusinessObjects.Administration
{
    [Table("UserNurseLinks")]
    public class UserNanny
    {
        [Key]
        public Guid UserNurseLinkId { get; set; }

        public Guid UserId { get; set; }
        public Guid NannyId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        [ForeignKey(nameof(NannyId))]
        public User Nanny { get; set; }
    }
}