using DM.Web.Classic.Views.Conversations.CreateMessage;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.ConversationControllers
{
    public class CreateMessageController : DmControllerBase
    {
        private readonly IConversationService conversationService;
        private readonly IUnreadCounterService unreadCounterService;
        private readonly IUserProvider userProvider;

        public CreateMessageController(
            IConversationService conversationService,
            IUnreadCounterService unreadCounterService,
            IUserProvider userProvider)
        {
            this.conversationService = conversationService;
            this.unreadCounterService = unreadCounterService;
            this.userProvider = userProvider;
        }

        [HttpPost, ValidationRequired]
        public int Create(CreateMessageForm createMessageForm)
        {
            conversationService.CreateNewMessage(createMessageForm.ConversationId, createMessageForm.Text);
            unreadCounterService.Flush(createMessageForm.ConversationId, EntryType.Message);
            return PagingHelper.GetTotalPages(conversationService.CountMessages(createMessageForm.ConversationId), userProvider.CurrentSettings.MessagesPerPage);
        }
    }
}