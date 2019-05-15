using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Mail.Consumer.Processes;
using DM.Services.MessageQueuing;
using DM.Services.MessageQueuing.Dto;
using DM.Services.MessageQueuing.Processing;
using Microsoft.Extensions.Logging;

namespace DM.Services.Mail.Consumer
{
    /// <summary>
    /// Processes all events that should result in mail sending
    /// </summary>
    public class CompositeMailProcessor : IMessageProcessor<InvokedEvent>
    {
        private readonly IEnumerable<IMailProcessor> processors;
        private readonly ILogger<IMessageProcessor<InvokedEvent>> logger;

        /// <inheritdoc />
        public CompositeMailProcessor(
            IEnumerable<IMailProcessor> processors,
            ILogger<IMessageProcessor<InvokedEvent>> logger)
        {
            this.processors = processors;
            this.logger = logger;
        }

        /// <inheritdoc />
        public async Task<ProcessResult> Process(InvokedEvent message)
        {
            await Task.WhenAll(processors
                .Where(p => p.CanProcess(message.Type))
                .Select(p => p.Process(message)));
            logger.LogInformation("DM.Event {eventType} for entity {entityId} is handled",
                message.Type, message.EntityId);
            return ProcessResult.Success;
        }
    }
}