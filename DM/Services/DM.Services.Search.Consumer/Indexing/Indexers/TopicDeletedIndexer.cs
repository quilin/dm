using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.MessageQueuing.Dto;
using DM.Services.Search.Repositories;

namespace DM.Services.Search.Consumer.Indexing.Indexers
{
    /// <summary>
    /// Indexer for removed topic
    /// </summary>
    public class TopicDeletedIndexer : BaseIndexer
    {
        private readonly ISearchEngineRepository searchEngineRepository;

        /// <inheritdoc />
        public TopicDeletedIndexer(
            ISearchEngineRepository searchEngineRepository)
        {
            this.searchEngineRepository = searchEngineRepository;
        }
        
        /// <inheritdoc />
        protected override EventType EventType => EventType.DeletedTopic;

        /// <inheritdoc />
        public override Task Index(InvokedEvent invokedEvent)
        {
            return searchEngineRepository.Delete(invokedEvent.EntityId);
        }
    }
}