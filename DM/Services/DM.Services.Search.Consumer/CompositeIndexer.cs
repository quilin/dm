using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.MessageQueuing;
using DM.Services.MessageQueuing.Dto;
using DM.Services.MessageQueuing.Processing;
using DM.Services.Search.Consumer.Indexing;

namespace DM.Services.Search.Consumer
{
    /// <inheritdoc />
    public class CompositeIndexer : IMessageProcessor<InvokedEvent>
    {
        private readonly IEnumerable<IIndexer> indexers;

        /// <inheritdoc />
        public CompositeIndexer(
            IEnumerable<IIndexer> indexers)
        {
            this.indexers = indexers;
        }

        /// <inheritdoc />
        public async Task<ProcessResult> Process(InvokedEvent invokedEvent)
        {
            await Task.WhenAll(indexers
                .Where(i => i.CanIndex(invokedEvent.Type))
                .Select(i => i.Index(invokedEvent)));
            return ProcessResult.Success;
        }
    }
}