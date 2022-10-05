using System;
using System.Threading.Tasks;

namespace DM.Services.Community.BusinessProcesses.Account.EmailChange.Confirmation;

/// <summary>
/// Registration confirmation email sender
/// </summary>
internal interface IEmailChangeMailSender
{
    /// <summary>
    /// Sends the email change confirmation letter to newly registered user
    /// </summary>
    /// <param name="email">User email</param>
    /// <param name="login">User login</param>
    /// <param name="token">Confirmation token</param>
    /// <returns></returns>
    Task Send(string email, string login, Guid token);
}