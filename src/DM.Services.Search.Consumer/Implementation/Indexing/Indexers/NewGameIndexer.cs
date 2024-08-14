using System.Linq;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Parsing;
using DM.Services.DataAccess;
using DM.Services.DataAccess.SearchEngine;
using DM.Services.MessageQueuing.GeneralBus;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Search.Consumer.Implementation.Indexing.Indexers;

/// <summary>
/// Indexer for new games
/// </summary>
internal class NewGameIndexer : BaseIndexer
{
    private readonly DmDbContext dbContext;
    private readonly IBbParserProvider bbParserProvider;
    private readonly IIndexingRepository repository;

    /// <inheritdoc />
    public NewGameIndexer(
        DmDbContext dbContext,
        IBbParserProvider bbParserProvider,
        IIndexingRepository repository)
    {
        this.dbContext = dbContext;
        this.bbParserProvider = bbParserProvider;
        this.repository = repository;
    }

    /// <inheritdoc />
    protected override EventType EventType => EventType.NewGame;

    /// <inheritdoc />
    public override async Task Index(InvokedEvent message)
    {
        var game = await dbContext.Games
            .Where(g => g.GameId == message.EntityId)
            .Select(g => new {g.GameId, g.Title, g.Info, g.Status, g.MasterId})
            .FirstAsync();
        await repository.Index(new SearchEntity
        {
            Id = game.GameId,
            EntityType = SearchEntityType.Game,
            Title = game.Title,
            Text = bbParserProvider.CurrentInfo.Parse(game.Info).ToHtml(),
            AuthorizedUsers = game.Status == GameStatus.Draft || game.Status == GameStatus.RequiresModeration
                ? new[] {game.MasterId}
                : null
        });
    }
}