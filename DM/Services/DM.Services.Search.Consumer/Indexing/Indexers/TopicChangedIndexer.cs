using System.Linq;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Parsing;
using DM.Services.DataAccess;
using DM.Services.DataAccess.SearchEngine;
using DM.Services.MessageQueuing.Dto;
using DM.Services.Search.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Search.Consumer.Indexing.Indexers
{
    /// <summary>
    /// Indexer for modified topic
    /// </summary>
    public class TopicChangedIndexer : BaseIndexer
    {
        private readonly DmDbContext dbContext;
        private readonly IBbParserProvider parserProvider;
        private readonly IIndexingRepository repository;

        /// <inheritdoc />
        public TopicChangedIndexer(
            DmDbContext dbContext,
            IBbParserProvider parserProvider,
            IIndexingRepository repository)
        {
            this.dbContext = dbContext;
            this.parserProvider = parserProvider;
            this.repository = repository;
        }
        
        /// <inheritdoc />
        protected override EventType EventType => EventType.ChangedTopic;

        /// <inheritdoc />
        public override async Task Index(InvokedEvent invokedEvent)
        {
            var topic = await dbContext.ForumTopics
                .Where(t => t.ForumTopicId == invokedEvent.EntityId)
                .Select(t => new {t.Forum.ViewPolicy, t.Title, t.Text})
                .FirstAsync();
            await repository.Index(new SearchEntity
            {
                Id = invokedEvent.EntityId,
                EntityType = SearchEntityType.Topic,
                Title = topic.Title,
                Text = parserProvider.CurrentCommon.Parse(topic.Text).ToHtml(),
                AuthorizedRoles = topic.ViewPolicy.GetAuthorizedRoles()
            });
        }
    }
}