using System.Threading.Tasks;
using DM.Web.Classic.Views.Profile;
using DM.Web.Classic.Views.Profile.Settings;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.CommunityControllers
{
    public class ProfileController : DmControllerBase
    {
        private readonly IProfileViewModelBuilder profileViewModelBuilder;
        private readonly IUserSettingsFormBuilder userSettingsFormBuilder;

        public ProfileController(
            IProfileViewModelBuilder profileViewModelBuilder,
            IUserSettingsFormBuilder userSettingsFormBuilder)
        {
            this.profileViewModelBuilder = profileViewModelBuilder;
            this.userSettingsFormBuilder = userSettingsFormBuilder;
        }

        public async Task<IActionResult> Index(string login)
        {
            var profileViewModel = await profileViewModelBuilder.Build(login);
            return View(profileViewModel);
        }

        public IActionResult GetSettings()
        {
            var userSettingsForm = userSettingsFormBuilder.Build();
            return PartialView("Settings/UserSettings", userSettingsForm);
        }
    }
}