using System.Threading.Tasks;

namespace DM.Services.Mail.Sender;

/// <summary>
/// Sends the defined email
/// </summary>
public interface IMailSender
{
    /// <summary>
    /// Sends an email
    /// </summary>
    /// <param name="letter">DTO letter</param>
    /// <returns></returns>
    Task Send(MailLetter letter);
}