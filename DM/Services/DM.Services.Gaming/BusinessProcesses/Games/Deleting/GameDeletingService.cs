using System;
using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Games;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;
using DM.Services.Gaming.BusinessProcesses.Games.Updating;
using DM.Services.MessageQueuing.GeneralBus;

namespace DM.Services.Gaming.BusinessProcesses.Games.Deleting;

/// <inheritdoc />
internal class GameDeletingService : IGameDeletingService
{
    private readonly IGameReadingService gameReadingService;
    private readonly IIntentionManager intentionManager;
    private readonly IUpdateBuilderFactory updateBuilderFactory;
    private readonly IGameUpdatingRepository repository;
    private readonly IInvokedEventProducer producer;

    /// <inheritdoc />
    public GameDeletingService(
        IGameReadingService gameReadingService,
        IIntentionManager intentionManager,
        IUpdateBuilderFactory updateBuilderFactory,
        IGameUpdatingRepository repository,
        IInvokedEventProducer producer)
    {
        this.gameReadingService = gameReadingService;
        this.intentionManager = intentionManager;
        this.updateBuilderFactory = updateBuilderFactory;
        this.repository = repository;
        this.producer = producer;
    }

    /// <inheritdoc />
    public async Task DeleteGame(Guid gameId)
    {
        var gameToRemove = await gameReadingService.GetGame(gameId);
        intentionManager.ThrowIfForbidden(GameIntention.Delete, gameToRemove);

        var updateBuilder = updateBuilderFactory.Create<Game>(gameId)
            .Field(g => g.IsRemoved, true);
        await repository.Update(updateBuilder);
        await producer.Send(EventType.DeletedGame, gameId);
    }
}