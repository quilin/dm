using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.MessageQueuing;
using DM.Services.MessageQueuing.Dto;
using DM.Services.MessageQueuing.Processing;
using DM.Services.Search.Consumer.Implementation.Indexing;
using Microsoft.Extensions.Logging;

namespace DM.Services.Search.Consumer.Implementation
{
    /// <inheritdoc />
    internal class CompositeIndexer : IMessageProcessor<InvokedEvent>
    {
        private readonly IEnumerable<IIndexer> indexers;
        private readonly ILogger<IMessageProcessor<InvokedEvent>> logger;

        /// <inheritdoc />
        public CompositeIndexer(
            IEnumerable<IIndexer> indexers,
            ILogger<CompositeIndexer> logger)
        {
            this.indexers = indexers;
            this.logger = logger;
        }

        /// <inheritdoc />
        public async Task<ProcessResult> Process(InvokedEvent message)
        {
            await Task.WhenAll(indexers
                .Where(i => i.CanIndex(message.Type))
                .Select(i => i.Index(message)));
            logger.LogInformation("DM.Event {EventType} for entity {EntityId} is indexed",
                message.Type, message.EntityId);
            return ProcessResult.Success;
        }
    }
}