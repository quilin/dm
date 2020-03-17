using System.Threading.Tasks;
using DM.Web.Classic.Views.Profile;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.CommunityControllers
{
    public class ProfileController : DmControllerBase
    {
        private readonly IProfileViewModelBuilder profileViewModelBuilder;

        public ProfileController(
            IProfileViewModelBuilder profileViewModelBuilder)
        {
            this.profileViewModelBuilder = profileViewModelBuilder;
        }

        public async Task<IActionResult> Index(string login)
        {
            var profileViewModel = await profileViewModelBuilder.Build(login);
            return View(profileViewModel);
        }
    }
}