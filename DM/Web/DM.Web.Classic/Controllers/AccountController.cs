using System;
using DM.Web.Classic.Views.Account;
using DM.Web.Classic.Views.Account.Activation;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers
{
    public class AccountController : DmControllerBase
    {
        private readonly IUserProvider userProvider;
        private readonly IFormAuthenticationService formAuthenticationService;
        private readonly IUserService userService;
        private readonly ILoginFormBuilder loginFormBuilder;
        private readonly IRegistrationFormBuilder registrationFormBuilder;
        private readonly IUpdatePasswordFormBuilder updatePasswordFormBuilder;
        private readonly IUserActionsViewModelBuilder userActionsViewModelBuilder;
        private readonly IIntentionsManager intentionsManager;

        public AccountController(
            IUserProvider userProvider,
            IFormAuthenticationService formAuthenticationService,
            IUserService userService,
            ILoginFormBuilder loginFormBuilder,
            IRegistrationFormBuilder registrationFormBuilder,
            IUpdatePasswordFormBuilder updatePasswordFormBuilder,
            IUserActionsViewModelBuilder userActionsViewModelBuilder,
            IIntentionsManager intentionsManager
            )
        {
            this.userProvider = userProvider;
            this.formAuthenticationService = formAuthenticationService;
            this.userService = userService;
            this.loginFormBuilder = loginFormBuilder;
            this.registrationFormBuilder = registrationFormBuilder;
            this.updatePasswordFormBuilder = updatePasswordFormBuilder;
            this.userActionsViewModelBuilder = userActionsViewModelBuilder;
            this.intentionsManager = intentionsManager;
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View("Register", registrationFormBuilder.Build(Request));
        }

        [HttpPost, ValidationRequired]
        public IActionResult Register(RegistrationForm registrationForm)
        {
            userService.Create(registrationForm.Login, registrationForm.Email, out var errors);
            return errors.Length > 0
                ? AjaxFormError(errors)
                : new EmptyResult();
        }

        [HttpGet]
        public ActionResult UpdatePassword(Guid tokenId)
        {
            var token = userService.FindToken(tokenId);
            if (token == null)
            {
                return View("UserActivationFailed");
            }

            var updatePasswordForm = updatePasswordFormBuilder.Build(token);
            return View("Activation/UpdatePassword", updatePasswordForm);
        }

        [HttpPost]
        public IActionResult UpdatePassword(UpdatePasswordForm updatePasswordForm)
        {
            if (!userService.TryUpdatePassword(updatePasswordForm.TokenId,
                updatePasswordForm.Password, updatePasswordForm.PasswordConfirmation,
                out var user, out var errors))
            {
                return AjaxFormError(errors);
            }
            formAuthenticationService.LogIn(HttpContext, user.UserId, false, out _);
            return new EmptyResult();
        }

        [HttpGet]
        public ActionResult LogIn()
        {
            return View("LogIn", loginFormBuilder.Build(Request));
        }

        [HttpPost]
        public IActionResult LogIn(LoginForm loginForm)
        {
            return userProvider.CurrentAuthenticationState != UserAuthenticationError.NoError
                ? AjaxFormError(userProvider.CurrentAuthenticationState)
                : Content(loginForm.RedirectUrl);
        }

        [HttpGet]
        public ActionResult RestorePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RestorePassword(RestorePasswordForm restorePasswordForm)
        {
            return userService.TryInitiatePasswordRestoration(restorePasswordForm.Email, out var error)
                ? new EmptyResult()
                : AjaxFormError(error);
        }

        public ActionResult LogOut()
        {
            formAuthenticationService.LogOut(HttpContext);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogInAs(Guid userId)
        {
            intentionsManager.ThrowIfForbidden(CommonIntention.LogAsAnyUser);
            formAuthenticationService.LogIn(HttpContext, userId, false, out _);
            return RedirectToAction("Index", "Home");
        }

        #region ChildActions
        public ActionResult GetUserActions()
        {
            var user = userProvider.Current;
            if (user.IsGuest())
            {
                return PartialView("GuestActions");
            }

            var userActionsViewModel = userActionsViewModelBuilder.Build(user.Login);
            return PartialView("UserActions", userActionsViewModel);
        }
        #endregion
    }
}