using System.Linq;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Parsing;
using DM.Services.DataAccess;
using DM.Services.DataAccess.SearchEngine;
using DM.Services.MessageQueuing.GeneralBus;
using DM.Services.Search.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Search.Consumer.Implementation.Indexing.Indexers;

/// <summary>
/// Indexer for modified topic
/// </summary>
internal class TopicChangedIndexer : BaseIndexer
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
    protected override EventType EventType => EventType.ChangedForumTopic;

    /// <inheritdoc />
    public override async Task Index(InvokedEvent message)
    {
        var topic = await dbContext.ForumTopics
            .Where(t => t.ForumTopicId == message.EntityId)
            .Select(t => new {t.Forum.ViewPolicy, t.Title, t.Text})
            .FirstAsync();
        var authorizedRoles = topic.ViewPolicy.GetAuthorizedRoles().ToArray();

        await repository.UpdateByParent(message.EntityId, authorizedRoles);
        await repository.Index(new SearchEntity
        {
            Id = message.EntityId,
            ParentEntityId = message.EntityId,
            EntityType = SearchEntityType.Topic,
            Title = topic.Title,
            Text = parserProvider.CurrentCommon.Parse(topic.Text).ToHtml(),
            AuthorizedRoles = authorizedRoles
        });
    }
}