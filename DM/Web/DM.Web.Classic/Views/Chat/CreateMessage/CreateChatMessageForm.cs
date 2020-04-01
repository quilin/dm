using System.ComponentModel.DataAnnotations;
using BBCodeParser;

namespace DM.Web.Classic.Views.Chat.CreateMessage
{
    public class CreateChatMessageForm
    {
        [Required(ErrorMessage = "Введите текст сообщения")]
        public string Text { get; set; }

        public IBbParser Parser { get; set; }
    }
}