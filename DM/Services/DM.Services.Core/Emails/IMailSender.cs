using System.Threading.Tasks;

namespace DM.Services.Core.Emails
{
    /// <summary>
    /// SMTP client
    /// </summary>
    public interface IMailSender
    {
        /// <summary>
        /// Send an email
        /// </summary>
        /// <param name="address">Recipient address</param>
        /// <param name="subject">Letter topic</param>
        /// <param name="body">Letter body</param>
        /// <returns></returns>
        Task Send(string address, string subject, string body);
    }
}