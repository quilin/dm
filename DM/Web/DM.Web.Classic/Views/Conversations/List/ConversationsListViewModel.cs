using DM.Web.Classic.Views.Conversations.List.Conversations;

namespace DM.Web.Classic.Views.Conversations.List
{
    public class ConversationsListViewModel
    {
        public int CurrentPage { get; set; }
        public int TotalPagesCount { get; set; }
        public int PageSize { get; set; }
        public int EntityNumber { get; set; }

        public ConversationViewModel[] Conversations { get; set; }
    }
}