using System.Threading.Tasks;

namespace DM.Web.Classic.Views.Conversations.List
{
    public interface IConversationsListViewModelBuilder
    {
        Task<ConversationsListViewModel> Build(int entityNumber);
    }
}