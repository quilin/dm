using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Web.Classic.Views.Conversations.Messages;

namespace DM.Web.Classic.Views.Conversations
{
    public interface IMessagesViewModelBuilder
    {
        Task<MessagesListViewModel> Build(string login, int entityNumber);
        Task<IEnumerable<MessageViewModel>> BuildList(string login, int entityNumber);
    }
}