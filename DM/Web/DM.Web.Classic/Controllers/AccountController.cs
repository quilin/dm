using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Community.BusinessProcesses.Account.PasswordChange;
using DM.Services.Community.BusinessProcesses.Account.PasswordReset;
using DM.Services.Community.BusinessProcesses.Account.Registration;
using DM.Web.Classic.Middleware;
using DM.Web.Classic.Views.Account;
using DM.Web.Classic.Views.Account.Activation;
using DM.Web.Core.Authentication;
using DM.Web.Core.Authentication.Credentials;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRegistrationService registrationService;
        private readonly IPasswordResetService passwordResetService;
        private readonly IPasswordChangeService passwordChangeService;
        private readonly IWebAuthenticationService webAuthenticationService;
        private readonly IRegistrationFormBuilder registrationFormBuilder;
        private readonly ILoginFormBuilder loginFormBuilder;
        private readonly IUpdatePasswordFormBuilder updatePasswordFormBuilder;

        public AccountController(
            IRegistrationService registrationService,
            IPasswordResetService passwordResetService,
            IPasswordChangeService passwordChangeService,
            IWebAuthenticationService webAuthenticationService,
            IRegistrationFormBuilder registrationFormBuilder,
            ILoginFormBuilder loginFormBuilder,
            IUpdatePasswordFormBuilder updatePasswordFormBuilder)
        {
            this.registrationService = registrationService;
            this.passwordResetService = passwordResetService;
            this.passwordChangeService = passwordChangeService;
            this.webAuthenticationService = webAuthenticationService;
            this.registrationFormBuilder = registrationFormBuilder;
            this.loginFormBuilder = loginFormBuilder;
            this.updatePasswordFormBuilder = updatePasswordFormBuilder;
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
            return Ok();
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
            switch (loggedIdentity.Error)
            {
                case AuthenticationError.NoError:
                    return Content(loginForm.RedirectUrl);
                case AuthenticationError.WrongLogin:
                    return BadRequest(new Dictionary<string, object>
                    {
                        [nameof(LoginForm.Login)] = "Такого пользователя нет"
                    });
                case AuthenticationError.WrongPassword:
                    return BadRequest(new Dictionary<string, object>
                    {
                        [nameof(LoginForm.Password)] = "Неправильный пароль"
                    });
                case AuthenticationError.Banned:
                    return BadRequest(new Dictionary<string, object>
                    {
                        [nameof(LoginForm.Login)] = "Учетная запись заблокирована в связи с нарушением правил сайта"
                    });
                case AuthenticationError.Inactive:
                    return BadRequest(new Dictionary<string, object>
                    {
                        [nameof(LoginForm.Login)] = "Учетная запись ещё не активирована. Проверьте свою почту!"
                    });
                case AuthenticationError.Removed:
                    return BadRequest(new Dictionary<string, object>
                    {
                        [nameof(LoginForm.Login)] = "Учетная запись удалена"
                    });
                default:
                    return BadRequest();
            }
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
        public async Task<IActionResult> UpdatePassword(Guid token) =>
            View("Activation/UpdatePassword", await updatePasswordFormBuilder.Build(token));

        [HttpPost]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordForm updatePasswordForm)
        {
            var user = await passwordChangeService.Change(new UserPasswordChange
            {
                Token = updatePasswordForm.Token,
                NewPassword = updatePasswordForm.AlteredPassword
            });
            await webAuthenticationService.Authenticate(new UnconditionalCredentials
            {
                UserId = user.UserId
            }, HttpContext);
            return new EmptyResult();
        }

        public async Task<IActionResult> LogInAs(Guid userId)
        {
            await webAuthenticationService.Authenticate(new UnconditionalCredentials
            {
                UserId = userId
            }, HttpContext);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> LogOut()
        {
            await webAuthenticationService.Logout(HttpContext);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> LogOutElsewhere(string login)
        {
            await webAuthenticationService.LogoutElsewhere(HttpContext);
            return Ok();
        }
    }
}