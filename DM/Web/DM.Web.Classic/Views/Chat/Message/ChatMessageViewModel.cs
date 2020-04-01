using System;
using DM.Web.Classic.Views.Shared.User;

namespace DM.Web.Classic.Views.Chat.Message
{
    public class ChatMessageViewModel
    {
        public UserViewModel Author { get; set; }
        public DateTimeOffset CreateDate { get; set; }

        public string Text { get; set; }

        public bool DisplayDate { get; set; }
        public bool DisplayName { get; set; }
    }
}