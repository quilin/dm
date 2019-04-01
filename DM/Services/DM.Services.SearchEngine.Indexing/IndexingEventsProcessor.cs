using System;
using System.Text;
using System.Threading.Tasks;
using DM.Services.Core.Implementation.CorrelationToken;
using DM.Services.DataAccess.Eventing;
using DM.Services.MessageQueuing;
using DM.Services.MessageQueuing.Processing;

namespace DM.Services.SearchEngine.Indexing
{
    /// <inheritdoc />
    public class IndexingEventsProcessor : IMessageProcessor<InvokedEvent>
    {
        private readonly ICorrelationTokenProvider correlationTokenProvider;

        /// <inheritdoc />
        public IndexingEventsProcessor(
            ICorrelationTokenProvider correlationTokenProvider)
        {
            this.correlationTokenProvider = correlationTokenProvider;
        }

        /// <inheritdoc />
        public Task<ProcessResult> Process(InvokedEvent message)
        {
            Console.WriteLine(new StringBuilder()
                .AppendLine($"Incoming message with correlation token {correlationTokenProvider.Current}")
                .AppendLine($"Invoked event {message.Type} for entity {message.EntityId}").ToString());
            return Task.FromResult(ProcessResult.Success);
        }
    }
}