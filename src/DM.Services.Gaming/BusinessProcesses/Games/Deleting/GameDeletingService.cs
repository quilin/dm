using System;
using System.Threading;
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
internal class GameDeletingService(
    IGameReadingService gameReadingService,
    IIntentionManager intentionManager,
    IUpdateBuilderFactory updateBuilderFactory,
    IGameUpdatingRepository repository,
    IInvokedEventProducer producer) : IGameDeletingService
{
    /// <inheritdoc />
    public async Task DeleteGame(Guid gameId, CancellationToken cancellationToken)
    {
        var gameToRemove = await gameReadingService.GetGame(gameId, cancellationToken);
        intentionManager.ThrowIfForbidden(GameIntention.Delete, gameToRemove);

        var updateBuilder = updateBuilderFactory.Create<Game>(gameId)
            .Field(g => g.IsRemoved, true);
        await repository.Update(updateBuilder, cancellationToken);
        await producer.Send(EventType.DeletedGame, gameId);
    }
}