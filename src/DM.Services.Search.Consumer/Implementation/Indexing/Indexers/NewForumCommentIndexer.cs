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
/// Indexer for new forum commentaries
/// </summary>
internal class NewForumCommentIndexer : BaseIndexer
{
    private readonly DmDbContext dbContext;
    private readonly IBbParserProvider parserProvider;
    private readonly IIndexingRepository repository;

    /// <inheritdoc />
    public NewForumCommentIndexer(
        DmDbContext dbContext,
        IBbParserProvider parserProvider,
        IIndexingRepository repository)
    {
        this.dbContext = dbContext;
        this.parserProvider = parserProvider;
        this.repository = repository;
    }

    /// <inheritdoc />
    protected override EventType EventType => EventType.NewForumComment;

    /// <inheritdoc />
    public override async Task Index(InvokedEvent message)
    {
        var comment = await dbContext.Comments
            .Where(c => c.CommentId == message.EntityId)
            .Select(c => new {c.Topic.Forum.ViewPolicy, c.Topic.ForumTopicId, c.Text})
            .FirstAsync();
        await repository.Index(new SearchEntity
        {
            Id = message.EntityId,
            ParentEntityId = comment.ForumTopicId,
            EntityType = SearchEntityType.ForumComment,
            Text = parserProvider.CurrentCommon.Parse(comment.Text).ToHtml(),
            AuthorizedRoles = comment.ViewPolicy.GetAuthorizedRoles()
        });
    }
}