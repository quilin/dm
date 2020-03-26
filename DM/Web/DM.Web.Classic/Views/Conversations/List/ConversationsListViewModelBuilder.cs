using System.Linq;
using System.Threading.Tasks;
using DM.Services.Community.BusinessProcesses.Messaging.Reading;
using DM.Services.Core.Dto;
using DM.Web.Classic.Views.Conversations.List.Conversations;

namespace DM.Web.Classic.Views.Conversations.List
{
    public class ConversationsListViewModelBuilder : IConversationsListViewModelBuilder
    {
        private readonly IConversationReadingService conversationService;
        private readonly IConversationViewModelBuilder conversationViewModelBuilder;

        public ConversationsListViewModelBuilder(
            IConversationReadingService conversationService,
            IConversationViewModelBuilder conversationViewModelBuilder
        )
        {
            this.conversationService = conversationService;
            this.conversationViewModelBuilder = conversationViewModelBuilder;
        }

        public async Task<ConversationsListViewModel> Build(int entityNumber)
        {
            var (conversations, paging) = await conversationService.Get(new PagingQuery {Number = entityNumber});

            return new ConversationsListViewModel
            {
                CurrentPage = paging.CurrentPage,
                TotalPagesCount = paging.TotalPagesCount,
                PageSize = paging.PageSize,
                EntityNumber = paging.EntityNumber,

                Conversations = conversations
                    .Select(conversationViewModelBuilder.Build)
                    .ToArray()
            };
        }
    }
}