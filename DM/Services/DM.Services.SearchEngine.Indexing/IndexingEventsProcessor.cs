using System;
using System.Text;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.CorrelationToken;
using DM.Services.DataAccess.Eventing;
using DM.Services.MessageQueuing;
using DM.Services.MessageQueuing.Processing;

namespace DM.Services.SearchEngine.Indexing
{
    public class IndexingEventsProcessor : IMessageProcessor<InvokedEvent>
    {
        private readonly ICorrelationTokenProvider correlationTokenProvider;

        public IndexingEventsProcessor(
            ICorrelationTokenProvider correlationTokenProvider)
        {
            this.correlationTokenProvider = correlationTokenProvider;
        }
        
        public Task<ProcessResult> Process(InvokedEvent message)
        {
            Console.WriteLine(new StringBuilder()
                .AppendLine($"Incoming message with correlation token {correlationTokenProvider.Current}")
                .AppendLine($"Invoked event {message.Type} for entity {message.EntityId}").ToString());
            return Task.FromResult(ProcessResult.Success);
        }
    }
}