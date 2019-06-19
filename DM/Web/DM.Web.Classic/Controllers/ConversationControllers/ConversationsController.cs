using System;
using DM.Web.Classic.Extensions.RequestExtensions;
using DM.Web.Classic.Views.Conversations;
using DM.Web.Classic.Views.Conversations.List;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DM.Web.Classic.Controllers.ConversationControllers
{
    public class ConversationsController : DmControllerBase
    {
        private readonly IConversationsListViewModelBuilder conversationsListViewModelBuilder;
        private readonly IMessagesViewModelBuilder messagesViewModelBuilder;
        private readonly IConversationService conversationService;
        private readonly IUserService userService;
        private readonly IUnreadCounterService unreadCounterService;
        private readonly IUserProvider userProvider;

        public ConversationsController(
            IConversationsListViewModelBuilder conversationsListViewModelBuilder,
            IMessagesViewModelBuilder messagesViewModelBuilder,
            IConversationService conversationService,
            IUserService userService,
            IUnreadCounterService unreadCounterService,
            IUserProvider userProvider
            )
        {
            this.conversationsListViewModelBuilder = conversationsListViewModelBuilder;
            this.messagesViewModelBuilder = messagesViewModelBuilder;
            this.conversationService = conversationService;
            this.userService = userService;
            this.unreadCounterService = unreadCounterService;
            this.userProvider = userProvider;
        }

        public ActionResult List(int entityNumber)
        {
            var conversationsListViewModel = conversationsListViewModelBuilder.Build(entityNumber);
            return !Request.IsAjaxRequest()
                ? View("~/Views/Conversations/List/Conversations.cshtml", conversationsListViewModel)
                : View("~/Views/Conversations/List/ConversationsList.cshtml", conversationsListViewModel.Conversations);
        }

        public ActionResult LastUnread(string login)
        {
            var collocutor = userService.Find(login);
            var conversation = conversationService.Find(collocutor.UserId);

            var entityNumber = 1;
            if (conversation != null)
            {
                var totalCount = conversationService.CountMessages(conversation.ConversationId);
                if (totalCount > 0)
                {
                    var unreadCount = unreadCounterService.GetFirstUnreadData(
                            new[] {conversation.ConversationId}, EntryType.Message)[userProvider.Current.UserId]
                        .Counter;
                    entityNumber = Math.Min(totalCount - unreadCount + 1, totalCount);
                }
            }

            return RedirectToAction("Index", new RouteValueDictionary
            {
                {"login", login},
                {"entityNumber", entityNumber}
            });
        }

        public ActionResult Index(string login, int entityNumber)
        {
            if (!Request.IsAjaxRequest())
            {
                var messagesListViewModel = messagesViewModelBuilder.Build(login, entityNumber);
                return View("~/Views/Conversations/Messages.cshtml", messagesListViewModel);
            }

            var messages = messagesViewModelBuilder.BuildList(login, entityNumber);
            return View("~/Views/Conversations/MessagesList.cshtml", messages);
        }

        [HttpPost]
        public int Remove(Guid[] messageIds)
        {
            var conversation = conversationService.RemoveMessages(messageIds);
            return PagingHelper.GetTotalPages(conversationService.CountMessages(conversation.ConversationId), userProvider.CurrentSettings.MessagesPerPage);
        }

        [HttpPost]
        public ActionResult ToggleIgnore(Guid conversationId)
        {
            var conversation = conversationService.ToggleIgnoreConversation(conversationId);
            var isIgnored = conversation.IsIgnoredByUser1 || conversation.IsIgnoredByUser2;
            return Json(new {isIgnored});
        }

        [HttpPost]
        public ActionResult MarkAllAsRead()
        {
            unreadCounterService.FlushAll(userProvider.Current.UserId, EntryType.Message);
            return RedirectToAction("List", new RouteValueDictionary {{"entityNumber", 1}});
        }
    }
}