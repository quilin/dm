using System.Linq;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess;
using DM.Services.DataAccess.SearchEngine;
using DM.Services.MessageQueuing.GeneralBus;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Search.Consumer.Implementation.Indexing.Indexers;

/// <inheritdoc />
internal class NewUserIndexer : BaseIndexer
{
    private readonly DmDbContext dbContext;
    private readonly IIndexingRepository repository;

    /// <inheritdoc />
    public NewUserIndexer(
        DmDbContext dbContext,
        IIndexingRepository repository)
    {
        this.dbContext = dbContext;
        this.repository = repository;
    }
        
    /// <inheritdoc />
    protected override EventType EventType => EventType.ActivatedUser;

    /// <inheritdoc />
    public override async Task Index(InvokedEvent message)
    {
        var userInfo = await dbContext.Users
            .Where(u => u.UserId == message.EntityId)
            .Select(u => new {u.Login, u.Name})
            .FirstAsync();

        await repository.Index(new SearchEntity
        {
            Id = message.EntityId,
            Title = userInfo.Login,
            Text = userInfo.Name,
            EntityType = SearchEntityType.User
        });
    }
}