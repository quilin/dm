using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.MessageQueuing.Dto;

namespace DM.Services.Search.Consumer.Indexing.Indexers
{
    /// <inheritdoc />
    public class CommentDeletedIndexer : BaseIndexer
    {
        private readonly IIndexingRepository indexingRepository;

        /// <inheritdoc />
        public CommentDeletedIndexer(
            IIndexingRepository indexingRepository)
        {
            this.indexingRepository = indexingRepository;
        }
    
        /// <inheritdoc />
        protected override EventType EventType => EventType.DeletedForumComment;

        /// <inheritdoc />
        public override Task Index(InvokedEvent message)
        {
            return indexingRepository.Delete(message.EntityId);
        }
    }
}