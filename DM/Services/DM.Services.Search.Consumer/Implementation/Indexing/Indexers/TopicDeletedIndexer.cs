using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.MessageQueuing.Dto;

namespace DM.Services.Search.Consumer.Implementation.Indexing.Indexers
{
    /// <summary>
    /// Indexer for removed topic
    /// </summary>
    public class TopicDeletedIndexer : BaseIndexer
    {
        private readonly IIndexingRepository repository;

        /// <inheritdoc />
        public TopicDeletedIndexer(
            IIndexingRepository repository)
        {
            this.repository = repository;
        }
        
        /// <inheritdoc />
        protected override EventType EventType => EventType.DeletedTopic;

        /// <inheritdoc />
        public override Task Index(InvokedEvent message)
        {
            return repository.DeleteByParent(message.EntityId);
        }
    }
}