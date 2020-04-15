using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.BusinessProcesses.Uploads;
using DM.Services.Community.BusinessProcesses.Account.EmailChange;
using DM.Services.Community.BusinessProcesses.Account.PasswordChange;
using DM.Services.Community.BusinessProcesses.Users.Updating;
using DM.Services.Core.Exceptions;
using DM.Services.Core.Parsing;
using DM.Web.Classic.Middleware;
using DM.Web.Classic.Views.Profile;
using DM.Web.Classic.Views.Profile.EditorTemplates;
using DM.Web.Classic.Views.Profile.EditProfileCredentials;
using DM.Web.Classic.Views.Profile.Settings;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DM.Web.Classic.Controllers.CommunityControllers
{
    public class EditProfileController : Controller
    {
        private readonly IUserUpdatingService userUpdatingService;
        private readonly IPasswordChangeService passwordChangeService;
        private readonly IEmailChangeService emailChangeService;
        private readonly IUploadService uploadService;
        private readonly IBbParserProvider bbParserProvider;
        private readonly IIdentityProvider identityProvider;
        private readonly IEditCredentialsFormBuilder credentialsFormBuilder;

        public EditProfileController(
            IUserUpdatingService userUpdatingService,
            IPasswordChangeService passwordChangeService,
            IEmailChangeService emailChangeService,
            IUploadService uploadService,
            IBbParserProvider bbParserProvider,
            IIdentityProvider identityProvider,
            IEditCredentialsFormBuilder credentialsFormBuilder)
        {
            this.userUpdatingService = userUpdatingService;
            this.passwordChangeService = passwordChangeService;
            this.emailChangeService = emailChangeService;
            this.uploadService = uploadService;
            this.bbParserProvider = bbParserProvider;
            this.identityProvider = identityProvider;
            this.credentialsFormBuilder = credentialsFormBuilder;
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

        [HttpPost]
        public async Task<IActionResult> UploadPicture(IFormFileCollection files, CancellationToken token)
        {
            var file = files.First();
            var upload = await uploadService.Upload(new CreateUpload
            {
                EntityId = identityProvider.Current.User.UserId,
                FileName = file.FileName,
                ContentType = file.ContentType,
                StreamAccessor = file.OpenReadStream
            });

            return Ok(upload.FilePath);
        }

        [HttpGet]
        public ActionResult EditEmail()
        {
            var editEmailForm = credentialsFormBuilder.BuildEmail();
            return View("~/Views/Profile/EditProfileCredentials/EditEmail.cshtml", editEmailForm);
        }

        private static readonly IDictionary<string, (string Field, IDictionary<string, string> Messages)>
            EditEmailErrors = new Dictionary<string, (string, IDictionary<string, string>)>
            {
                [nameof(UserEmailChange.Email)] = (nameof(EditEmailForm.NewEmail), new Dictionary<string, string>
                {
                    [ValidationError.Empty] = "Введите новый e-mail",
                    [ValidationError.Invalid] = "Введите корректный e-mail"
                }),
                [nameof(UserEmailChange.Password)] = (nameof(EditEmailForm.Password), new Dictionary<string, string>
                {
                    [ValidationError.Empty] = "Введите пароль от вашей учетной записи",
                    [ValidationError.Invalid] = "Неправильный пароль"
                })
            };

        [HttpPost, ValidationRequired]
        public async Task<IActionResult> EditEmail(EditEmailForm editEmailForm)
        {
            try
            {
                await emailChangeService.Change(new UserEmailChange
                {
                    Login = identityProvider.Current.User.Login,
                    Email = editEmailForm.NewEmail,
                    Password = editEmailForm.Password
                });
                return Ok();
            }
            catch (ValidationException validationException)
            {
                var clientErrors = validationException.Errors
                    .GroupBy(e => e.PropertyName)
                    .Where(g => EditEmailErrors.ContainsKey(g.Key))
                    .ToDictionary(
                        g => EditEmailErrors[g.Key].Field,
                        g => g
                            .Where(e => EditEmailErrors[g.Key].Messages.ContainsKey(e.ErrorMessage))
                            .Select(e => EditEmailErrors[g.Key].Messages[e.ErrorMessage])
                            .FirstOrDefault())
                    .Where(kvp => !string.IsNullOrEmpty(kvp.Value));
                return BadRequest(clientErrors);
            }
        }

        [HttpGet]
        public ActionResult EditPassword()
        {
            var editPasswordForm = credentialsFormBuilder.BuildPassword();
            return View("~/Views/Profile/EditProfileCredentials/EditPassword.cshtml", editPasswordForm);
        }

        private static readonly IDictionary<string, (string Field, IDictionary<string, string> Messages)>
            EditPasswordErrors = new Dictionary<string, (string, IDictionary<string, string>)>
            {
                [nameof(UserPasswordChange.OldPassword)] = (nameof(EditPasswordForm.OldPassword), new Dictionary<string, string>
                {
                    [ValidationError.Empty] = "Введите пароль от вашей учетной записи",
                    [ValidationError.Invalid] = "Неправильный пароль"
                }),
                [nameof(UserPasswordChange.NewPassword)] = (nameof(EditPasswordForm.NewPassword), new Dictionary<string, string>
                {
                    [ValidationError.Empty] = "Введите новый пароль",
                    [ValidationError.Short] = "В пароле должно быть хотя бы 6 символов"
                })
            };

        [HttpPost, ValidationRequired]
        public async Task<IActionResult> EditPassword(EditPasswordForm editPasswordForm)
        {
            try
            {
                await passwordChangeService.Change(new UserPasswordChange
                {
                    Token = null,
                    OldPassword = editPasswordForm.OldPassword,
                    NewPassword = editPasswordForm.NewPassword
                });
                return Ok();
            }
            catch (ValidationException validationException)
            {
                var clientErrors = validationException.Errors
                    .GroupBy(e => e.PropertyName)
                    .Where(g => EditPasswordErrors.ContainsKey(g.Key))
                    .ToDictionary(
                        g => EditPasswordErrors[g.Key].Field,
                        g => g
                            .Where(e => EditPasswordErrors[g.Key].Messages.ContainsKey(e.ErrorMessage))
                            .Select(e => EditPasswordErrors[g.Key].Messages[e.ErrorMessage])
                            .FirstOrDefault())
                    .Where(kvp => !string.IsNullOrEmpty(kvp.Value));
                return BadRequest(clientErrors);
            }
        }
    }
}