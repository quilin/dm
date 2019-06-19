using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Web.Classic.Views.Account;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.ViewComponents.Header
{
    public class UserActions : ViewComponent
    {
        private readonly IIdentity identity;
        private readonly IUserActionsViewModelBuilder userActionsViewModelBuilder;

        public UserActions(
            IIdentityProvider identityProvider,
            IUserActionsViewModelBuilder userActionsViewModelBuilder)
        {
            identity = identityProvider.Current;
            this.userActionsViewModelBuilder = userActionsViewModelBuilder;
        }
        
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.Run(() =>
            {
                var user = identity.User;
                if (!user.IsAuthenticated)
                {
                    return View("~/Views/Account/GuestActions.cshtml");
                }

                var userActionsViewModel = userActionsViewModelBuilder.Build(user.Login);
                return View("~/Views/Account/UserActions.cshtml", userActionsViewModel);
            });
        }
    }
}