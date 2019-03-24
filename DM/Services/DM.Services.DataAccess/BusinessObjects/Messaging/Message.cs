using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.DataAccess.BusinessObjects.DataContracts;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.DataAccess.BusinessObjects.Messaging
{
    [Table("Messages")]
    public class Message : IRemovable
    {
        [Key] public Guid MessageId { get; set; }

        public Guid UserId { get; set; }
        public Guid ConversationId { get; set; }

        public DateTime CreateDate { get; set; }
        public string Text { get; set; }

        public bool IsRemoved { get; set; }

        [ForeignKey(nameof(UserId))] public virtual User Author { get; set; }

        [ForeignKey(nameof(ConversationId))] public virtual Conversation Conversation { get; set; }
    }
}