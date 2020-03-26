using System.Linq;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Community.BusinessProcesses.Messaging.Reading;
using DM.Web.Classic.Views.Conversations.Messages;
using DM.Web.Classic.Views.Shared.User;

namespace DM.Web.Classic.Views.Conversations.List.Conversations
{
    public class ConversationViewModelBuilder : IConversationViewModelBuilder
    {
        private readonly IMessageViewModelBuilder messageViewModelBuilder;
        private readonly IUserViewModelBuilder userViewModelBuilder;
        private readonly IIdentityProvider identityProvider;

        public ConversationViewModelBuilder(
            IMessageViewModelBuilder messageViewModelBuilder,
            IUserViewModelBuilder userViewModelBuilder,
            IIdentityProvider identityProvider)
        {
            this.messageViewModelBuilder = messageViewModelBuilder;
            this.userViewModelBuilder = userViewModelBuilder;
            this.identityProvider = identityProvider;
        }

        public ConversationViewModel Build(Conversation conversation)
        {
            var currentUserId = identityProvider.Current.User.UserId;
            var collocutor = conversation.Participants.First(p => p.UserId != currentUserId);
            var collocutorUserViewModel = userViewModelBuilder.Build(collocutor);

            return new ConversationViewModel
            {
                LastMessage = messageViewModelBuilder.Build(conversation.LastMessage, collocutorUserViewModel),
                UnreadMessagesCount = conversation.UnreadMessagesCount,
                LastMessageIsOwn = conversation.LastMessage.Author.UserId == currentUserId,
                Collocutor = collocutorUserViewModel
            };
        }
    }
}