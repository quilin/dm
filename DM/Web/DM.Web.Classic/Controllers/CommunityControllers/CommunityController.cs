using DM.Web.Classic.Extensions.RequestExtensions;
using DM.Web.Classic.Views.Community;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.CommunityControllers
{
    public class CommunityController : DmControllerBase
    {
        private readonly ICommunityViewModelBuilder communityViewModelBuilder;

        public CommunityController(
            ICommunityViewModelBuilder communityViewModelBuilder
            )
        {
            this.communityViewModelBuilder = communityViewModelBuilder;
        }

        public IActionResult Index(int entityNumber, bool withInactive)
        {
            if (Request.IsAjaxRequest())
            {
                var communityUsers = communityViewModelBuilder.BuildList(entityNumber, withInactive);
                return PartialView("UsersList", communityUsers);
            }
            var communityViewModel = communityViewModelBuilder.Build(entityNumber, withInactive);
            return View(communityViewModel);
        }
    }
}