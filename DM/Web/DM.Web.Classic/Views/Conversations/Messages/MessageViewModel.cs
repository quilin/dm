using System;
using DM.Web.Classic.Views.Shared.User;

namespace DM.Web.Classic.Views.Conversations.Messages
{
    public class MessageViewModel
    {
        public Guid MessageId { get; set; }
        public DateTimeOffset CreateDate { get; set; }

        public UserViewModel Sender { get; set; }
        public string Text { get; set; }

        public bool CanReport { get; set; }

        public bool DisplayName { get; set; }
        public bool DisplayDate { get; set; }
    }
}