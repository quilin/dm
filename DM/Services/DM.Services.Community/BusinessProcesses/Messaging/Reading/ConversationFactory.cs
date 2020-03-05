using System;
using System.Collections.Generic;
using DM.Services.Core.Implementation;
using DbConversation = DM.Services.DataAccess.BusinessObjects.Messaging.Conversation;
using DbConversationLink = DM.Services.DataAccess.BusinessObjects.Messaging.UserConversationLink;

namespace DM.Services.Community.BusinessProcesses.Messaging.Reading
{
    /// <inheritdoc />
    public class ConversationFactory : IConversationFactory
    {
        private readonly IGuidFactory guidFactory;

        /// <inheritdoc />
        public ConversationFactory(
            IGuidFactory guidFactory)
        {
            this.guidFactory = guidFactory;
        }
        
        /// <inheritdoc />
        public (DbConversation conversation, IEnumerable<DbConversationLink>) CreateVisavi(Guid userId, Guid visaviId)
        {
            var conversationId = guidFactory.Create();
            var conversation = new DbConversation
            {
                ConversationId = conversationId,
                LastMessageId = null,
                Visavi = true
            };
            var links = new[]
            {
                new DbConversationLink
                {
                    UserConversationLinkId = guidFactory.Create(),
                    ConversationId = conversationId,
                    UserId = userId,
                    IsRemoved = false
                },
                new DbConversationLink
                {
                    UserConversationLinkId = guidFactory.Create(),
                    ConversationId = conversationId,
                    UserId = visaviId,
                    IsRemoved = false
                }
            };

            return (conversation, links);
        }
    }
}