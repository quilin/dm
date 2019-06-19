using System;
using DM.Web.Classic.Views.Profile;
using DM.Web.Classic.Views.Profile.Characters;
using DM.Web.Classic.Views.Profile.Modules;
using DM.Web.Classic.Views.Profile.Settings;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.ProfileControllers
{
    public class ProfileController : DmControllerBase
    {
        private readonly IProfileViewModelBuilder profileViewModelBuilder;
        private readonly IProfileModulesViewModelBuilder profileModulesViewModelBuilder;
        private readonly IProfileCharactersViewModelBuilder profileCharactersViewModelBuilder;
        private readonly IUserSettingsFormBuilder userSettingsFormBuilder;
        private readonly IUserService userService;
        private readonly ISessionProvider sessionProvider;
        private readonly IIntentionsManager intentionsManager;

        public ProfileController(
            IProfileViewModelBuilder profileViewModelBuilder,
            IProfileModulesViewModelBuilder profileModulesViewModelBuilder,
            IProfileCharactersViewModelBuilder profileCharactersViewModelBuilder,
            IUserSettingsFormBuilder userSettingsFormBuilder,
            IUserService userService,
            ISessionProvider sessionProvider,
            IIntentionsManager intentionsManager
            )
        {
            this.profileViewModelBuilder = profileViewModelBuilder;
            this.profileModulesViewModelBuilder = profileModulesViewModelBuilder;
            this.profileCharactersViewModelBuilder = profileCharactersViewModelBuilder;
            this.userSettingsFormBuilder = userSettingsFormBuilder;
            this.userService = userService;
            this.sessionProvider = sessionProvider;
            this.intentionsManager = intentionsManager;
        }

        public IActionResult Index(string login)
        {
            var profileViewModel = profileViewModelBuilder.Build(login);
            return View(profileViewModel);
        }

        public IActionResult GetModules(string login)
        {
            var profileModulesViewModel = profileModulesViewModelBuilder.Build(login);
            return PartialView("Modules/Modules", profileModulesViewModel);
        }

        public IActionResult GetCharacters(string login)
        {
            var profileCharactersViewModel = profileCharactersViewModelBuilder.Build(login);
            return PartialView("Characters/Characters", profileCharactersViewModel);
        }

        public IActionResult GetUserSettings()
        {
            var userSettingsForm = userSettingsFormBuilder.Build();
            return PartialView("Settings/UserSettings", userSettingsForm);
        }

        public IActionResult CloseSessions(Guid userId)
        {
            var user = userService.Read(userId);
            sessionProvider.RemoveAllSessionsFromUserWithoutCurrent(user);
            var canEdit = intentionsManager.IsAllowed(UserIntention.Edit, user);
            return Json(new { canEdit });
        }
    }
}