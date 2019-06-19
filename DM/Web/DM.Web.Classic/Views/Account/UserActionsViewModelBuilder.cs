using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.DataAccess.BusinessObjects.Common;

namespace DM.Web.Classic.Views.Account
{
    public class UserActionsViewModelBuilder : IUserActionsViewModelBuilder
    {
        private readonly IUnreadCountersRepository unreadCounterService;
        private readonly IIdentity identity;

        public UserActionsViewModelBuilder(
            IUnreadCountersRepository unreadCounterService,
            IIdentityProvider identityProvider)
        {
            this.unreadCounterService = unreadCounterService;
            identity = identityProvider.Current;
        }

        public UserActionsViewModel Build(string login)
        {
            var userId = identity.User.UserId;
            return new UserActionsViewModel
            {
                UserName = login,
                UnreadDialoguesCount = unreadCounterService.SelectByParents(userId, UnreadEntryType.Message, userId)
                    .Result[userId]
            };
        }
    }
}