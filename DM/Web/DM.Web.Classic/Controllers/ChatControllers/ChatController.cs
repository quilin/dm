using DM.Web.Classic.Views.Chats;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.ChatControllers
{
    public class ChatController : DmControllerBase
    {
        private readonly IChatViewModelBuilder chatViewModelBuilder;

        public ChatController(
            IChatViewModelBuilder chatViewModelBuilder
            )
        {
            this.chatViewModelBuilder = chatViewModelBuilder;
        }

        public ActionResult Index()
        {
            var chatViewModel = chatViewModelBuilder.Build();
            return View("~/Views/Chats/Chat.cshtml", chatViewModel);
        }

        public ActionResult OlderChatEntries(int startIndex)
        {
            var chatViewModel = chatViewModelBuilder.Build(startIndex);
            return View("~/Views/Chats/ChatMessages.cshtml", chatViewModel);
        }

        public ActionResult NewestChatEntries(long fromDate = 0)
        {
            var chatViewModel = chatViewModelBuilder.Build(fromDate);
            if (chatViewModel.Messages.Length == 0)
            {
                return new EmptyResult();
            }
            return View("~/Views/Chats/ChatMessages.cshtml", chatViewModel);
        }
    }
}