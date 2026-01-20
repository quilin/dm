using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Core.Dto;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;

namespace DM.Services.Gaming.BusinessProcesses.Blacklist.Reading;

/// <inheritdoc />
internal class BlacklistReadingService(
    IGameReadingService gameReadingService,
    IIntentionManager intentionManager,
    IBlacklistReadingRepository repository) : IBlacklistReadingService
{
    /// <inheritdoc />
    public async Task<IEnumerable<GeneralUser>> Get(Guid gameId, CancellationToken cancellationToken)
    {
        var game = await gameReadingService.GetGame(gameId, cancellationToken);
        intentionManager.ThrowIfForbidden(GameIntention.Edit, game);
        return await repository.Get(gameId, cancellationToken);
    }
}