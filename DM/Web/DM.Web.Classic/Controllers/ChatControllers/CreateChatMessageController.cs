using DM.Web.Classic.Views.Chats.CreateMessage;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.ChatControllers
{
    public class CreateChatMessageController : DmControllerBase
    {
        private readonly IChatService chatService;

        public CreateChatMessageController(
            IChatService chatService
            )
        {
            this.chatService = chatService;
        }

        [HttpPost, ValidationRequired]
        public void Create(CreateChatMessageForm form)
        {
            chatService.Create(form.Text);
        }
    }
}