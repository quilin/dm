using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Mail.Consumer.Processes;
using DM.Services.MessageQueuing;
using DM.Services.MessageQueuing.Dto;
using DM.Services.MessageQueuing.Processing;

namespace DM.Services.Mail.Consumer
{
    /// <summary>
    /// Processes all events that should result in mail sending
    /// </summary>
    public class CompositeMailProcessor : IMessageProcessor<InvokedEvent>
    {
        private readonly IEnumerable<IMailProcessor> processors;

        /// <inheritdoc />
        public CompositeMailProcessor(
            IEnumerable<IMailProcessor> processors)
        {
            this.processors = processors;
        }

        /// <inheritdoc />
        public async Task<ProcessResult> Process(InvokedEvent message)
        {
            await Task.WhenAll(processors
                .Where(p => p.CanProcess(message.Type))
                .Select(p => p.Process(message)));
            return ProcessResult.Success;
        }
    }
}