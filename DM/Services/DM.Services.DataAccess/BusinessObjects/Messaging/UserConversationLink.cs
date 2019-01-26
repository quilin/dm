using System;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.DataAccess.BusinessObjects.DataContracts;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.DataAccess.BusinessObjects.Messaging
{
    [Table("UserConversationLinks")]
    public class UserConversationLink : IRemovable
    {
        public Guid UserConversationLinkId { get; set; }
        public Guid UserId { get; set; }
        public Guid ConversationId { get; set; }

        public bool IsRemoved { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        [ForeignKey(nameof(ConversationId))]
        public virtual Conversation Conversation { get; set; }
    }
}