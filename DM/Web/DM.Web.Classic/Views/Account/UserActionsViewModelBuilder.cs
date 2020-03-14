using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.DataAccess.BusinessObjects.Common;

namespace DM.Web.Classic.Views.Account
{
    public class UserActionsViewModelBuilder : IUserActionsViewModelBuilder
    {
        private readonly IUnreadCountersRepository unreadCounterService;
        private readonly IIdentityProvider identityProvider;

        public UserActionsViewModelBuilder(
            IUnreadCountersRepository unreadCounterService,
            IIdentityProvider identityProvider)
        {
            this.unreadCounterService = unreadCounterService;
            this.identityProvider = identityProvider;
        }

        public UserActionsViewModel Build(string login)
        {
            var userId = identityProvider.Current.User.UserId;
            return new UserActionsViewModel
            {
                UserName = login,
                UnreadDialoguesCount = unreadCounterService.SelectByParents(userId, UnreadEntryType.Message, userId)
                    .Result[userId]
            };
        }
    }
}