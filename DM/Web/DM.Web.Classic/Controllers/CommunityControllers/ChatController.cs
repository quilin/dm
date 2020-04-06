using System;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Community.BusinessProcesses.Chat.Creating;
using DM.Web.Classic.Extensions.RequestExtensions;
using DM.Web.Classic.Middleware;
using DM.Web.Classic.Views.Chat;
using DM.Web.Classic.Views.Chat.CreateMessage;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.CommunityControllers
{
    public class ChatController : Controller
    {
        private readonly IChatViewModelBuilder chatViewModelBuilder;
        private readonly IChatCreatingService chatCreatingService;

        public ChatController(
            IChatViewModelBuilder chatViewModelBuilder,
            IChatCreatingService chatCreatingService)
        {
            this.chatViewModelBuilder = chatViewModelBuilder;
            this.chatCreatingService = chatCreatingService;
        }

        public async Task<IActionResult> Index(int skip)
        {
            var chatViewModel = await chatViewModelBuilder.Build(skip);
            return Request.IsAjaxRequest()
                ? View("ChatMessages", chatViewModel)
                : View(chatViewModel);
        }

        [HttpPost, ValidationRequired]
        public async Task<IActionResult> CreateMessage(CreateChatMessageForm form)
        {
            await chatCreatingService.Create(new CreateChatMessage
            {
                Text = form.Text
            });
            return NoContent();
        }

        public async Task<IActionResult> NewestChatEntries(DateTimeOffset fromDate)
        {
            var chatViewModel = await chatViewModelBuilder.Build(fromDate);
            return chatViewModel.Messages.Any()
                ? View("ChatMessages", chatViewModel)
                : (IActionResult) new EmptyResult();
        }
    }
}