using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Community.BusinessProcesses.Messaging.Reading;
using DM.Services.Core.Dto;
using DM.Web.Classic.Views.Conversations.CreateMessage;
using DM.Web.Classic.Views.Conversations.Messages;
using DM.Web.Classic.Views.Shared.User;

namespace DM.Web.Classic.Views.Conversations
{
    public class MessagesViewModelBuilder : IMessagesViewModelBuilder
    {
        private readonly ICreateMessageFormFactory createMessageFormFactory;
        private readonly IMessageViewModelBuilder messageViewModelBuilder;
        private readonly IUserViewModelBuilder userViewModelBuilder;
        private readonly IConversationReadingService conversationReadingService;
        private readonly IMessageReadingService messageReadingService;
        private readonly IIdentityProvider identityProvider;

        public MessagesViewModelBuilder(
            ICreateMessageFormFactory createMessageFormFactory,
            IMessageViewModelBuilder messageViewModelBuilder,
            IUserViewModelBuilder userViewModelBuilder,
            IConversationReadingService conversationReadingService,
            IMessageReadingService messageReadingService,
            IIdentityProvider identityProvider)
        {
            this.createMessageFormFactory = createMessageFormFactory;
            this.messageViewModelBuilder = messageViewModelBuilder;
            this.userViewModelBuilder = userViewModelBuilder;
            this.conversationReadingService = conversationReadingService;
            this.messageReadingService = messageReadingService;
            this.identityProvider = identityProvider;
        }

        public async Task<MessagesListViewModel> Build(string login, int entityNumber)
        {
            var currentUserId = identityProvider.Current.User.UserId;
            var conversation = await conversationReadingService.GetOrCreate(login);
            var (messages, paging) = await messageReadingService.Get(conversation.Id,
                new PagingQuery {Number = entityNumber});
            await conversationReadingService.MarkAsRead(conversation.Id);
            var allMessages = messages.ToArray();

            var collocutor = conversation.Participants.First(u => u.UserId != currentUserId);
            var collocutorViewModel = userViewModelBuilder.Build(collocutor);

            return new MessagesListViewModel
            {
                ConversationId = conversation.Id,

                CurrentPage = paging.CurrentPage,
                TotalPagesCount = paging.TotalPagesCount,
                PageSize = paging.PageSize,
                EntityNumber = paging.EntityNumber,

                Collocutor = collocutorViewModel,
                Messages = allMessages.Select((m, i) => messageViewModelBuilder.Build(m, allMessages, i)),
                CreateMessageForm = createMessageFormFactory.Create(conversation),

                // CanReport = intentionsManager.IsAllowed(CommonIntention.Report),
                // Ignored = isIgnoredByCurrentUser
            };
        }

        public async Task<IEnumerable<MessageViewModel>> BuildList(string login, int entityNumber)
        {
            var conversation = await conversationReadingService.GetOrCreate(login);

            var (messages, _) = await messageReadingService.Get(conversation.Id,
                new PagingQuery {Number = entityNumber});
            var allMessages = messages.ToArray();

            return allMessages.Select((m, i) => messageViewModelBuilder.Build(m, allMessages, i));
        }
    }
}