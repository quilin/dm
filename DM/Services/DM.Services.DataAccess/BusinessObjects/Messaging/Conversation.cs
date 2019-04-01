using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DM.Services.DataAccess.BusinessObjects.Messaging
{
    /// <summary>
    /// DAL model for users conversation
    /// </summary>
    [Table("Conversations")]
    public class Conversation
    {
        /// <summary>
        /// Conversation identifier
        /// </summary>
        public Guid ConversationId { get; set; }

        /// <summary>
        /// Links with conversation participants
        /// </summary>
        [InverseProperty(nameof(UserConversationLink.Conversation))]
        public virtual ICollection<UserConversationLink> UserLinks { get; set; }

        /// <summary>
        /// Messages
        /// </summary>
        [InverseProperty(nameof(Message.Conversation))]
        public virtual ICollection<Message> Messages { get; set; }
    }
}