using System;
using System.Threading.Tasks;
using DM.Services.Community.BusinessProcesses.Messaging.Creating;
using DM.Services.Community.BusinessProcesses.Messaging.Reading;
using DM.Services.Core.Dto;
using DM.Web.Classic.Extensions.RequestExtensions;
using DM.Web.Classic.Middleware;
using DM.Web.Classic.Views.Conversations;
using DM.Web.Classic.Views.Conversations.CreateMessage;
using DM.Web.Classic.Views.Conversations.List;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DM.Web.Classic.Controllers.CommunityControllers
{
    public class ConversationsController : Controller
    {
        private readonly IConversationsListViewModelBuilder conversationsListViewModelBuilder;
        private readonly IMessagesViewModelBuilder messagesViewModelBuilder;
        private readonly IConversationReadingService conversationReadingService;
        private readonly IMessageCreatingService messageCreatingService;
        private readonly IMessageReadingService messageReadingService;

        public ConversationsController(
            IConversationsListViewModelBuilder conversationsListViewModelBuilder,
            IMessagesViewModelBuilder messagesViewModelBuilder,
            IConversationReadingService conversationReadingService,
            IMessageCreatingService messageCreatingService,
            IMessageReadingService messageReadingService)
        {
            this.conversationsListViewModelBuilder = conversationsListViewModelBuilder;
            this.messagesViewModelBuilder = messagesViewModelBuilder;
            this.conversationReadingService = conversationReadingService;
            this.messageCreatingService = messageCreatingService;
            this.messageReadingService = messageReadingService;
        }

        public async Task<IActionResult> List(int entityNumber)
        {
            var conversationsListViewModel = await conversationsListViewModelBuilder.Build(entityNumber);
            return !Request.IsAjaxRequest()
                ? View("~/Views/Conversations/List/Conversations.cshtml", conversationsListViewModel)
                : View("~/Views/Conversations/List/ConversationsList.cshtml", conversationsListViewModel.Conversations);
        }

        public async Task<IActionResult> LastUnread(string login)
        {
            var conversation = await conversationReadingService.GetOrCreate(login);
            return RedirectToAction("Index", new RouteValueDictionary
            {
                ["login"] = login,
                ["entityNumber"] = Math.Max(1, conversation.TotalMessagesCount - conversation.UnreadMessagesCount)
            });
        }

        public async Task<IActionResult> Index(string login, int entityNumber)
        {
            if (!Request.IsAjaxRequest())
            {
                var messagesListViewModel = await messagesViewModelBuilder.Build(login, entityNumber);
                return View("~/Views/Conversations/Messages.cshtml", messagesListViewModel);
            }

            var messages = await messagesViewModelBuilder.BuildList(login, entityNumber);
            return View("~/Views/Conversations/MessagesList.cshtml", messages);
        }

        [HttpPost, ValidationRequired]
        public async Task<IActionResult> Create(CreateMessageForm createMessageForm)
        {
            await messageCreatingService.Create(new CreateMessage
            {
                ConversationId = createMessageForm.ConversationId,
                Text = createMessageForm.Text
            });
            await conversationReadingService.MarkAsRead(createMessageForm.ConversationId);
            var (_, paging) = await messageReadingService.Get(createMessageForm.ConversationId, PagingQuery.Empty);
            return Ok(paging.TotalPagesCount);
        }
    }
}