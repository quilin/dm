using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.MessageQueuing.Dto;

namespace DM.Services.Mail.Consumer.Processes
{
    /// <summary>
    /// Certain processor for emails
    /// </summary>
    public interface IMailProcessor
    {
        /// <summary>
        /// Tells if this event entity can send email
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <returns>Can be sent by this processor</returns>
        bool CanProcess(EventType eventType);

        /// <summary>
        /// Sends email for invoked event
        /// </summary>
        /// <param name="invokedEvent">Event</param>
        /// <returns></returns>
        Task Process(InvokedEvent invokedEvent);
    }
}