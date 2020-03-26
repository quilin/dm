using DM.Web.Classic.Views.Conversations.Messages;
using DM.Web.Classic.Views.Shared.User;

namespace DM.Web.Classic.Views.Conversations.List.Conversations
{
    public class ConversationViewModel
    {
        public UserViewModel Collocutor { get; set; }
        public int UnreadMessagesCount { get; set; }
        public bool LastMessageIsOwn { get; set; }
        public MessageViewModel LastMessage { get; set; }
    }
}