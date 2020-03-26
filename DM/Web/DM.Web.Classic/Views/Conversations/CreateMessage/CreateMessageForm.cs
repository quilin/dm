using System;
using System.ComponentModel.DataAnnotations;
using BBCodeParser;

namespace DM.Web.Classic.Views.Conversations.CreateMessage
{
    public class CreateMessageForm
    {
        public Guid ConversationId { get; set; }

        [Required(ErrorMessage = "Введите текст сообщения")]
        public string Text { get; set; }

        public bool CanWrite { get; set; }

        public IBbParser Parser { get; set; }
    }
}