using System;
using System.Collections.Generic;
using DM.Web.Classic.Views.Conversations.CreateMessage;
using DM.Web.Classic.Views.Conversations.Messages;
using DM.Web.Classic.Views.Shared.User;

namespace DM.Web.Classic.Views.Conversations
{
    public class MessagesListViewModel
    {
        public Guid ConversationId { get; set; }

        public int TotalPagesCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int EntityNumber { get; set; }

        public UserViewModel Collocutor { get; set; }
        public IEnumerable<MessageViewModel> Messages { get; set; }
        public CreateMessageForm CreateMessageForm { get; set; }

        public bool CanReport { get; set; }
        public bool Ignored { get; set; }
    }
}