using DM.Services.Community.BusinessProcesses.Messaging.Reading;
using DM.Services.Core.Parsing;
using DM.Web.Classic.Views.Shared.User;

namespace DM.Web.Classic.Views.Conversations.Messages
{
    public class MessageViewModelBuilder : IMessageViewModelBuilder
    {
        private readonly IBbParserProvider bbParserProvider;
        private readonly IUserViewModelBuilder userViewModelBuilder;

        public MessageViewModelBuilder(
            IBbParserProvider bbParserProvider,
            IUserViewModelBuilder userViewModelBuilder
        )
        {
            this.bbParserProvider = bbParserProvider;
            this.userViewModelBuilder = userViewModelBuilder;
        }

        public MessageViewModel Build(Message message, Message[] messages, int index)
        {
            var prevMessage = index == 0
                ? null
                : messages[index - 1];

            var displayName = prevMessage == null || message.Author.UserId != prevMessage.Author.UserId;
            var displayDate = displayName || (message.CreateDate - prevMessage.CreateDate).TotalMinutes > 10;

            return new MessageViewModel
            {
                MessageId = message.Id,
                CreateDate = message.CreateDate,
                Text = bbParserProvider.CurrentCommon.Parse(message.Text).ToHtml(),
                Sender = userViewModelBuilder.Build(message.Author),
                // CanReport = intentionManager.IsAllowed(MessageIntention.Report, message),
                DisplayName = displayName,
                DisplayDate = displayDate
            };
        }

        public MessageViewModel Build(Message message, UserViewModel collocutor)
        {
            return new MessageViewModel
            {
                MessageId = message.Id,
                CreateDate = message.CreateDate,
                Text = bbParserProvider.CurrentConversationMessage.Parse(message.Text).ToText(),
                Sender = collocutor,
                DisplayName = true,
                DisplayDate = true
            };
        }
    }
}