using DM.Services.Common.Authorization;
using DM.Services.Community.BusinessProcesses.Messaging;
using DM.Services.Community.BusinessProcesses.Messaging.Reading;
using DM.Services.Core.Parsing;

namespace DM.Web.Classic.Views.Conversations.CreateMessage
{
    public class CreateMessageFormFactory : ICreateMessageFormFactory
    {
        private readonly IIntentionManager intentionManager;
        private readonly IBbParserProvider bbParserProvider;

        public CreateMessageFormFactory(
            IIntentionManager intentionManager,
            IBbParserProvider bbParserProvider)
        {
            this.intentionManager = intentionManager;
            this.bbParserProvider = bbParserProvider;
        }

        public CreateMessageForm Create(Conversation conversation)
        {
            return new CreateMessageForm
            {
                ConversationId = conversation.Id,
                CanWrite = intentionManager.IsAllowed(ConversationIntention.CreateMessage, conversation),
                Parser = bbParserProvider.CurrentCommon
            };
        }
    }
}