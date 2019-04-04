using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.MessageQueuing.Dto;

namespace DM.Services.Search.Consumer.Indexing
{
    /// <inheritdoc />
    public abstract class BaseIndexer : IIndexer
    {
        /// <summary>
        /// Event type that this indexer can process
        /// </summary>
        protected abstract EventType EventType { get; }

        /// <inheritdoc />
        public bool CanIndex(InvokedEvent invokedEvent) => invokedEvent.Type == EventType;

        /// <inheritdoc />
        public abstract Task Index(InvokedEvent invokedEvent);
    }
}