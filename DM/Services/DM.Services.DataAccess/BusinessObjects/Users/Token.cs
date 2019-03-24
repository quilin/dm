using System;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.DataAccess.BusinessObjects.DataContracts;

namespace DM.Services.DataAccess.BusinessObjects.Users
{
    public class Token : IRemovable
    {
        public Guid TokenId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreateDate { get; set; }
        public TokenType Type { get; set; }

        public bool IsRemoved { get; set; }

        [ForeignKey(nameof(UserId))] public virtual User User { get; set; }
    }
}