using System;
using System.Threading.Tasks;
using DM.Services.DataAccess.Eventing;
using DM.Services.MessageQueuing;
using DM.Services.MessageQueuing.Processing;
using DM.Services.Search.Implementation;

namespace DM.Services.Search.IndexingConsumer
{
    /// <inheritdoc />
    public class IndexingEventsProcessor : IMessageProcessor<InvokedEvent>
    {
        private readonly ICompositeIndexer indexer;

        /// <inheritdoc />
        public IndexingEventsProcessor(
            ICompositeIndexer indexer)
        {
            this.indexer = indexer;
        }

        /// <inheritdoc />
        public async Task<ProcessResult> Process(InvokedEvent message)
        {
            Console.WriteLine($"Indexing {message.Type} event...");
            await indexer.Index(message);
            Console.WriteLine($"{message.Type} has been indexed!");
            return ProcessResult.Success;
        }
    }
}