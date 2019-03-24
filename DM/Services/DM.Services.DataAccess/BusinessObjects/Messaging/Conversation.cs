using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DM.Services.DataAccess.BusinessObjects.Messaging
{
    [Table("Conversations")]
    public class Conversation
    {
        public Guid ConversationId { get; set; }

        [InverseProperty(nameof(UserConversationLink.Conversation))]
        public virtual ICollection<UserConversationLink> UserLinks { get; set; }

        [InverseProperty(nameof(Message.Conversation))]
        public virtual ICollection<Message> Messages { get; set; }
    }
}