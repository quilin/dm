using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.MessageQueuing.Dto;

namespace DM.Services.Mail.Consumer.Processes
{
    /// <summary>
    /// Base email processor
    /// </summary>
    public abstract class BaseMailProcessor : IMailProcessor
    {
        /// <inheritdoc />
        public bool CanProcess(EventType eventType) => eventType == EventType;

        /// <summary>
        /// Event type
        /// </summary>
        protected abstract EventType EventType { get; }

        /// <inheritdoc />
        public abstract Task Process(InvokedEvent invokedEvent);
    }
}