using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Community.BusinessProcesses.Registration;
using DM.Services.Community.Dto;
using DM.Web.Classic.Middleware;
using DM.Web.Classic.Views.Account;
using DM.Web.Core.Authentication;
using DM.Web.Core.Authentication.Credentials;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers
{
    public class AccountController : DmControllerBase
    {
        private readonly IRegistrationService registrationService;
        private readonly ILoginFormBuilder loginFormBuilder;
        private readonly IWebAuthenticationService webAuthenticationService;
        private readonly IRegistrationFormBuilder registrationFormBuilder;
        private readonly IUserActionsViewModelBuilder userActionsViewModelBuilder;
        private readonly IIdentity identity;

        public AccountController(
            IRegistrationService registrationService,
            ILoginFormBuilder loginFormBuilder,
            IWebAuthenticationService webAuthenticationService,
            IRegistrationFormBuilder registrationFormBuilder,
            IUserActionsViewModelBuilder userActionsViewModelBuilder,
            IIdentityProvider identityProvider)
        {
            this.registrationService = registrationService;
            this.loginFormBuilder = loginFormBuilder;
            this.webAuthenticationService = webAuthenticationService;
            this.registrationFormBuilder = registrationFormBuilder;
            this.userActionsViewModelBuilder = userActionsViewModelBuilder;
            identity = identityProvider.Current;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View("Register", registrationFormBuilder.Build(Request));
        }

        [HttpPost, ValidationRequired]
        public async Task<IActionResult> Register(RegistrationForm registrationForm)
        {
            await registrationService.Register(new UserRegistration
            {
                Email = registrationForm.Email,
                Login = registrationForm.Login,
                Password = registrationForm.Password
            });
            return new EmptyResult();
        }

        [HttpGet]
        public ActionResult LogIn()
        {
            return View("LogIn", loginFormBuilder.Build(Request));
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LoginForm loginForm)
        {
            var loggedIdentity = await webAuthenticationService.Authenticate(new LoginCredentials
            {
                Login = loginForm.Login,
                Password = loginForm.Password,
                RememberMe = !loginForm.DoNotRemember
            }, HttpContext);
            return loggedIdentity.Error != AuthenticationError.NoError
                ? AjaxFormError(loggedIdentity.Error)
                : Content(loginForm.RedirectUrl);
        }

        public ActionResult LogOut()
        {
            webAuthenticationService.Logout(HttpContext).Wait();
            return RedirectToAction("Index", "Home");
        }

        #region ChildActions
        public ActionResult GetUserActions()
        {
            if (!identity.User.IsAuthenticated)
            {
                return PartialView("GuestActions");
            }

            var userActionsViewModel = userActionsViewModelBuilder.Build(identity.User.Login);
            return PartialView("UserActions", userActionsViewModel);
        }
        #endregion
    }
}