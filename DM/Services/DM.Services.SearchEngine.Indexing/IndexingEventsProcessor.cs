using System;
using System.Threading.Tasks;
using DM.Services.DataAccess.Eventing;
using DM.Services.MessageQueuing;

namespace DM.Services.SearchEngine.Indexing
{
    public class IndexingEventsProcessor : IMessageProcessor<InvokedEvent>
    {
        public Task<ProcessResult> Process(InvokedEvent message)
        {
            Console.WriteLine($"Invoked event {message.Type} for entity {message.EntityId}");
            return Task.FromResult(ProcessResult.Success);
        }
    }
}