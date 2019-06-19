using System;
using System.Collections.Generic;
using DM.Web.Classic.Views.EditProfileCredentials;
using DM.Web.Classic.Views.Profile;
using DM.Web.Classic.Views.Profile.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DM.Web.Classic.Controllers.ProfileControllers
{
    public class EditProfileController : DmControllerBase
    {
        private readonly IUserService userService;
        private readonly IUserSettingsService userSettingsService;
        private readonly IUserProvider userProvider;
        private readonly IBbParserProvider bbParserProvider;
        private readonly IPersonalDataEditService personalDataEditService;
        private readonly IEditCredentialsFormBuilder editCredentialsFormBuilder;

        public EditProfileController(
            IUserService userService,
            IUserSettingsService userSettingsService,
            IUserProvider userProvider,
            IBbParserProvider bbParserProvider,
            IPersonalDataEditService personalDataEditService,
            IEditCredentialsFormBuilder editCredentialsFormBuilder
            )
        {
            this.userService = userService;
            this.userSettingsService = userSettingsService;
            this.userProvider = userProvider;
            this.bbParserProvider = bbParserProvider;
            this.personalDataEditService = personalDataEditService;
            this.editCredentialsFormBuilder = editCredentialsFormBuilder;
        }

        [HttpPost]
        public ActionResult EditInfo(Guid userId, string info)
        {
            userService.UpdateProfileKey(userId, new KeyValuePair<string, string>(UserProfileKeys.Info, info));
            return Content(bbParserProvider.CurrentInfo.Parse(info).ToHtml());
        }

        [HttpPost]
        public ActionResult EditPersonalData(Guid userId, PersonalDataField field, string value)
        {
            personalDataEditService.Edit(userId, field, value);
            return Content(field != PersonalDataField.Timezone
                ? value
                : string.IsNullOrEmpty(value) ? "Выбирать автоматически" : TimeZoneInfo.FindSystemTimeZoneById(value).DisplayName);
        }

        [HttpGet]
        public ActionResult EditEmail()
        {
            var editEmailForm = editCredentialsFormBuilder.BuildEmail();
            return View("~/Views/EditProfileCredentials/EditEmail.cshtml", editEmailForm);
        }

        [HttpPost, ValidationRequired]
        public IActionResult EditEmail(EditEmailForm editEmailForm)
        {
            return userService.TryChangeEmail(editEmailForm.Password, editEmailForm.NewEmail, out var error)
                ? new EmptyResult()
                : AjaxFormError(error);
        }

        [HttpGet]
        public ActionResult EditPassword()
        {
            var editPasswordForm = editCredentialsFormBuilder.BuilPassword();
            return View("~/Views/EditProfileCredentials/EditPassword.cshtml", editPasswordForm);
        }

        [HttpPost, ValidationRequired]
        public IActionResult EditPassword(EditPasswordForm editPasswordForm)
        {
            return userService.TryChangePassword(editPasswordForm.OldPassword, editPasswordForm.NewPassword, editPasswordForm.NewPasswordConfirmation, out var error)
                ? new EmptyResult()
                : AjaxFormError(error);
        }

        [HttpPost, ValidationRequired]
        public ActionResult UpdateSettings(UserSettingsForm userSettingsForm)
        {
            var user = userProvider.Current;
            userSettingsService.Update(userSettingsForm.PostsPerPage, userSettingsForm.CommentsPerPage,
                userSettingsForm.TopicsPerPage, userSettingsForm.MessagesPerPage,
                userSettingsForm.NurseGreetingsMessage, userSettingsForm.ColorScheme);
            personalDataEditService.Edit(user.UserId, PersonalDataField.Timezone,
                string.IsNullOrEmpty(userSettingsForm.Timezone) ? null : userSettingsForm.Timezone);
            userService.UpdateRatingDisabled(userSettingsForm.RatingDisabled);
            return RedirectToAction("Index", "Profile", new RouteValueDictionary {{"login", user.Login}});
        }
    }
}