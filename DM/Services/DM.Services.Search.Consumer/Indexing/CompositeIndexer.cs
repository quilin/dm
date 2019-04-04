using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.MessageQueuing.Dto;

namespace DM.Services.Search.Consumer.Indexing
{
    /// <inheritdoc />
    public class CompositeIndexer : ICompositeIndexer
    {
        private readonly IEnumerable<IIndexer> indexers;

        /// <inheritdoc />
        public CompositeIndexer(
            IEnumerable<IIndexer> indexers)
        {
            this.indexers = indexers;
        }

        /// <inheritdoc />
        public Task Index(InvokedEvent invokedEvent) => Task.WhenAll(indexers
            .Where(i => i.CanIndex(invokedEvent))
            .Select(i => i.Index(invokedEvent)));
    }
}