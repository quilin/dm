using System.Linq;
using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Community.Authorization;
using DM.Services.Community.BusinessProcesses.Chat.Reading;
using DM.Services.Core.Dto;
using DM.Web.Classic.Views.Chat.CreateMessage;
using DM.Web.Classic.Views.Chat.Message;

namespace DM.Web.Classic.Views.Chat
{
    public class ChatViewModelBuilder : IChatViewModelBuilder
    {
        private readonly ICreateChatMessageFormBuilder createChatMessageFormBuilder;
        private readonly IIntentionManager intentionManager;
        private readonly IChatReadingService chatService;
        private readonly IChatMessageViewModelBuilder chatMessageViewModelBuilder;

        public ChatViewModelBuilder(
            ICreateChatMessageFormBuilder createChatMessageFormBuilder,
            IIntentionManager intentionManager,
            IChatReadingService chatService,
            IChatMessageViewModelBuilder chatMessageViewModelBuilder
        )
        {
            this.createChatMessageFormBuilder = createChatMessageFormBuilder;
            this.intentionManager = intentionManager;
            this.chatService = chatService;
            this.chatMessageViewModelBuilder = chatMessageViewModelBuilder;
        }

        public async Task<ChatViewModel> Build(int skip = 0, int size = 50)
        {
            var (messages, paging) = await chatService.GetMessages(new PagingQuery
            {
                Skip = skip,
                Size = size
            });
            var allMessages = messages.ToArray();

            return new ChatViewModel
            {
                Messages = allMessages.Select((m, i) => chatMessageViewModelBuilder.Build(m, allMessages, i)),
                CanChat = intentionManager.IsAllowed(ChatIntention.CreateMessage),
                CreateForm = createChatMessageFormBuilder.Build()
            };
        }
    }
}