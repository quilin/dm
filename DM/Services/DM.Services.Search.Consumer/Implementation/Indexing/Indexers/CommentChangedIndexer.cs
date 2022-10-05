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

/// <inheritdoc />
internal class CommentChangedIndexer : BaseIndexer
{
    private readonly DmDbContext dbContext;
    private readonly IBbParserProvider bbParserProvider;
    private readonly IIndexingRepository indexingRepository;

    /// <inheritdoc />
    public CommentChangedIndexer(
        DmDbContext dbContext,
        IBbParserProvider bbParserProvider,
        IIndexingRepository indexingRepository)
    {
        this.dbContext = dbContext;
        this.bbParserProvider = bbParserProvider;
        this.indexingRepository = indexingRepository;
    }
    
    /// <inheritdoc />
    protected override EventType EventType => EventType.ChangedForumComment;

    /// <inheritdoc />
    public override async Task Index(InvokedEvent message)
    {
        var comment = await dbContext.Comments
            .Where(c => c.CommentId == message.EntityId)
            .Select(c => new {c.Text, c.Topic.Forum.ViewPolicy, c.Topic.ForumTopicId})
            .FirstAsync();

        await indexingRepository.Index(new SearchEntity
        {
            Id = message.EntityId,
            ParentEntityId = comment.ForumTopicId,
            EntityType = SearchEntityType.ForumComment,
            Text = bbParserProvider.CurrentCommon.Parse(comment.Text).ToHtml(),
            AuthorizedRoles = comment.ViewPolicy.GetAuthorizedRoles()
        });
    }
}