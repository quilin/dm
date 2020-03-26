using System.Threading.Tasks;
using DM.Services.Community.BusinessProcesses.Messaging.Reading;

namespace DM.Web.Classic.Views.Account
{
    public class UserActionsViewModelBuilder : IUserActionsViewModelBuilder
    {
        private readonly IConversationReadingService conversationReadingService;

        public UserActionsViewModelBuilder(
            IConversationReadingService conversationReadingService)
        {
            this.conversationReadingService = conversationReadingService;
        }

        public async Task<UserActionsViewModel> Build(string login)
        {
            return new UserActionsViewModel
            {
                UserName = login,
                UnreadDialoguesCount = await conversationReadingService.GetTotalUnreadCount()
            };
        }
    }
}