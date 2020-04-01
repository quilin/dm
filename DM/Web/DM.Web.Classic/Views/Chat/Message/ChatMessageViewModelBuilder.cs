using System;
using DM.Services.Community.BusinessProcesses.Chat.Reading;
using DM.Services.Core.Parsing;
using DM.Web.Classic.Views.Shared.User;

namespace DM.Web.Classic.Views.Chat.Message
{
    public class ChatMessageViewModelBuilder : IChatMessageViewModelBuilder
    {
        private readonly IUserViewModelBuilder userViewModelBuilder;
        private readonly IBbParserProvider bbParserProvider;

        public ChatMessageViewModelBuilder(
            IUserViewModelBuilder userViewModelBuilder,
            IBbParserProvider bbParserProvider)
        {
            this.userViewModelBuilder = userViewModelBuilder;
            this.bbParserProvider = bbParserProvider;
        }

        public ChatMessageViewModel Build(ChatMessage message, ChatMessage[] messages, int index)
        {
            var prevUserId = index == 0 ? (Guid?)null : messages[index - 1].Author.UserId;
            var displayName = message.Author.UserId != prevUserId;

            var displayDate = true;
            if (!displayName)
            {
                var prevMessageDate = messages[index - 1].CreateDate;
                displayDate = (message.CreateDate - prevMessageDate).TotalMinutes > 10;
            }

            return new ChatMessageViewModel
            {
                Author = userViewModelBuilder.Build(message.Author),
                CreateDate = message.CreateDate,
                Text = bbParserProvider.CurrentGeneralChat.Parse(message.Text).ToHtml(),
                
                DisplayName = displayName,
                DisplayDate = displayDate
            };
        }
    }
}