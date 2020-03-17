using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Community.BusinessProcesses.Account.PasswordReset;
using DM.Services.Community.BusinessProcesses.Account.Registration;
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
        private readonly IPasswordResetService passwordResetService;
        private readonly ILoginFormBuilder loginFormBuilder;
        private readonly IWebAuthenticationService webAuthenticationService;
        private readonly IRegistrationFormBuilder registrationFormBuilder;

        public AccountController(
            IRegistrationService registrationService,
            IPasswordResetService passwordResetService,
            ILoginFormBuilder loginFormBuilder,
            IWebAuthenticationService webAuthenticationService,
            IRegistrationFormBuilder registrationFormBuilder)
        {
            this.registrationService = registrationService;
            this.passwordResetService = passwordResetService;
            this.loginFormBuilder = loginFormBuilder;
            this.webAuthenticationService = webAuthenticationService;
            this.registrationFormBuilder = registrationFormBuilder;
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
        public IActionResult LogIn() => View(loginFormBuilder.Build(Request));

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

        [HttpGet]
        public ActionResult RestorePassword() => View();

        [HttpPost]
        public async Task<IActionResult> RestorePassword(RestorePasswordForm restorePasswordForm)
        {
            await passwordResetService.Reset(new UserPasswordReset
            {
                Email = restorePasswordForm.Email,
                Login = restorePasswordForm.Login
            });
            return Ok();
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

        public async Task<IActionResult> LogInAs(string login)
        {
            var loggedIdentity = await webAuthenticationService.Authenticate(new UnconditionalCredentials
            {
                // UserId = userId
            }, HttpContext);
            return loggedIdentity.Error != AuthenticationError.NoError
                ? AjaxFormError(loggedIdentity.Error)
                : new EmptyResult();
        }

        public async Task<IActionResult> LogOut()
        {
            await webAuthenticationService.Logout(HttpContext);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> LogOutElsewhere(string login)
        {
            await webAuthenticationService.LogoutAll(HttpContext);
            return Ok();
        }
    }
}