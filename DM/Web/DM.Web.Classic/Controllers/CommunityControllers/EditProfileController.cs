using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Community.BusinessProcesses.Users.Updating;
using DM.Services.Core.Parsing;
using DM.Web.Classic.Middleware;
using DM.Web.Classic.Views.Profile;
using DM.Web.Classic.Views.Profile.EditorTemplates;
using DM.Web.Classic.Views.Profile.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DM.Web.Classic.Controllers.CommunityControllers
{
    public class EditProfileController : DmControllerBase
    {
        private readonly IUserUpdatingService userUpdatingService;
        private readonly IBbParserProvider bbParserProvider;
        private readonly IIdentityProvider identityProvider;

        public EditProfileController(
            IUserUpdatingService userUpdatingService,
            IBbParserProvider bbParserProvider,
            IIdentityProvider identityProvider)
        {
            this.userUpdatingService = userUpdatingService;
            this.bbParserProvider = bbParserProvider;
            this.identityProvider = identityProvider;
        }

        [HttpPost]
        public async Task<IActionResult> EditInfo(EditInfoForm editInfoForm)
        {
            var updated = await userUpdatingService.Update(new UpdateUser
            {
                Login = editInfoForm.Login,
                Info = editInfoForm.Info,
            });
            return Ok(bbParserProvider.CurrentInfo.Parse(updated.Info).ToHtml());
        }

        private static readonly IDictionary<PersonalDataField, Expression<Func<UpdateUser, string>>> UpdateFields =
            new Dictionary<PersonalDataField, Expression<Func<UpdateUser, string>>>
            {
                [PersonalDataField.Icq] = u => u.Icq,
                [PersonalDataField.Location] = u => u.Location,
                [PersonalDataField.Name] = u => u.Name,
                [PersonalDataField.Status] = u => u.Status,
                [PersonalDataField.Skype] = u => u.Skype
            };

        [HttpPost]
        public async Task<IActionResult> EditPersonalData(string login, PersonalDataField field, string value)
        {
            var updateUser = new UpdateUser {Login = login};
            var updateField = (PropertyInfo) ((MemberExpression) UpdateFields[field].Body).Member;
            updateField.SetValue(updateUser, value);

            await userUpdatingService.Update(updateUser);
            return Ok(value);
        }

        [HttpPost, ValidationRequired]
        public async Task<IActionResult> UpdateSettings(UserSettingsForm userSettingsForm)
        {
            var userLogin = identityProvider.Current.User.Login;
            await userUpdatingService.Update(new UpdateUser
            {
                Login = userLogin,
                Settings = new UserSettings
                {
                    Paging = new PagingSettings
                    {
                        CommentsPerPage = userSettingsForm.CommentsPerPage,
                        EntitiesPerPage = userSettingsForm.EntitiesPerPage,
                        MessagesPerPage = userSettingsForm.MessagesPerPage,
                        TopicsPerPage = userSettingsForm.TopicsPerPage,
                        PostsPerPage = userSettingsForm.PostsPerPage
                    },
                    ColorSchema = userSettingsForm.ColorSchema,
                    NannyGreetingsMessage = userSettingsForm.NannyGreetingsMessage
                },
                RatingDisabled = userSettingsForm.RatingDisabled
            });
            return RedirectToAction("Index", "Profile", new RouteValueDictionary {{"login", userLogin}});
        }
    }
}