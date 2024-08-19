using System;
using System.Threading.Tasks;
using DM.Web.API.Authentication;
using DM.Web.API.Controllers.v1.Community;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;
using DM.Web.API.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.v1.Account;

/// <inheritdoc />
[ApiController]
[Route("v1/account")]
[ApiExplorerSettings(GroupName = "Account")]
public class AccountController(
    IRegistrationApiService registrationApiService,
    IActivationApiService activationApiService,
    ILoginApiService loginApiService,
    IPasswordResetApiService passwordResetApiService,
    IEmailChangeApiService emailChangeApiService) : ControllerBase
{
    /// <summary>
    /// Register new user
    /// </summary>
    /// <param name="registration">New user credentials information</param>
    /// <response code="201">User has been registered and expects confirmation by e-mail</response>
    /// <response code="400">Some of registration properties were invalid</response>
    [HttpPost(Name = nameof(Register))]
    [ProducesResponseType(201)]
    [ProducesResponseType(typeof(BadRequestError), 400)]
    public async Task<IActionResult> Register([FromBody] Registration registration)
    {
        await registrationApiService.Register(registration, HttpContext.RequestAborted);
        return CreatedAtRoute(nameof(UserController.GetUser), new {login = registration.Login}, null);
    }

    /// <summary>
    /// Activate registered user
    /// </summary>
    /// <param name="token">Activation token</param>
    /// <response code="200">User has been activated and logged in</response>
    /// <response code="400">Token is invalid</response>
    /// <response code="410">Token is expired</response>
    [HttpPut("{token:guid}", Name = nameof(Activate))]
    [ProducesResponseType(typeof(Envelope<User>), 200)]
    [ProducesResponseType(typeof(GeneralError), 400)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> Activate(Guid token) =>
        Ok(await activationApiService.Activate(token, HttpContext.RequestAborted));

    /// <summary>
    /// Get current user
    /// </summary>
    /// <response code="200"></response>
    [HttpGet(Name = nameof(GetCurrent))]
    [AuthenticationRequired]
    [ProducesResponseType(typeof(Envelope<UserDetails>), 200)]
    public async Task<IActionResult> GetCurrent() =>
        Ok(await loginApiService.GetCurrent(HttpContext.RequestAborted));

    /// <summary>
    /// Reset registered user password
    /// </summary>
    /// <param name="resetPassword">Account details</param>
    /// <response code="200">Password has been reset</response>
    /// <response code="400">Some account details were incorrect</response>
    [HttpPost("password", Name = nameof(ResetPassword))]
    [ProducesResponseType(typeof(Envelope<User>), 201)]
    [ProducesResponseType(typeof(BadRequestError), 400)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPassword resetPassword) =>
        Ok(await passwordResetApiService.Reset(resetPassword, HttpContext.RequestAborted));

    /// <summary>
    /// Change registered user password
    /// </summary>
    /// <param name="changePassword"></param>
    /// <response code="200">Password has been changed</response>
    /// <response code="400">Some account details were incorrect</response>
    [HttpPut("password", Name = nameof(ChangePassword))]
    [ProducesResponseType(typeof(Envelope<User>), 200)]
    [ProducesResponseType(typeof(BadRequestError), 400)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePassword changePassword) =>
        Ok(await passwordResetApiService.Change(changePassword, HttpContext.RequestAborted));

    /// <summary>
    /// Change registered user email
    /// </summary>
    /// <param name="changeEmail"></param>
    /// <response code="200">Email has been changed</response>
    /// <response code="400">Some account details were incorrect</response>
    [HttpPut("email", Name = nameof(ChangeEmail))]
    [ProducesResponseType(typeof(Envelope<User>), 200)]
    [ProducesResponseType(typeof(BadRequestError), 400)]
    public async Task<IActionResult> ChangeEmail([FromBody] ChangeEmail changeEmail) =>
        Ok(await emailChangeApiService.Change(changeEmail, HttpContext.RequestAborted));
}