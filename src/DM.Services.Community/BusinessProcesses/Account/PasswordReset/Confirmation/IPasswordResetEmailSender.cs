using System;
using System.Threading.Tasks;

namespace DM.Services.Community.BusinessProcesses.Account.PasswordReset.Confirmation;

/// <summary>
/// Password resetting email sender
/// </summary>
internal interface IPasswordResetEmailSender
{
    /// <summary>
    /// Sends the password reset confirmation letter to registered user
    /// </summary>
    /// <param name="email">User email</param>
    /// <param name="login">User login</param>
    /// <param name="token">Confirmation token</param>
    /// <returns></returns>
    Task Send(string email, string login, Guid token);
}