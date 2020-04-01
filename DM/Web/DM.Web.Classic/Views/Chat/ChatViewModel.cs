using System.Collections.Generic;
using DM.Web.Classic.Views.Chat.CreateMessage;
using DM.Web.Classic.Views.Chat.Message;

namespace DM.Web.Classic.Views.Chat
{
    public class ChatViewModel
    {
        public IEnumerable<ChatMessageViewModel> Messages { get; set; }

        public bool CanChat { get; set; }
        public CreateChatMessageForm CreateForm { get; set; }
    }
}