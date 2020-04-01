using System.Threading.Tasks;

namespace DM.Web.Classic.Views.Chat
{
    public interface IChatViewModelBuilder
    {
        Task<ChatViewModel> Build(int skip);
    }
}