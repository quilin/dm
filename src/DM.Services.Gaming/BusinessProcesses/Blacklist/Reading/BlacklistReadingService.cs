using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Core.Dto;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;

namespace DM.Services.Gaming.BusinessProcesses.Blacklist.Reading;

/// <inheritdoc />
internal class BlacklistReadingService : IBlacklistReadingService
{
    private readonly IGameReadingService gameReadingService;
    private readonly IIntentionManager intentionManager;
    private readonly IBlacklistReadingRepository repository;

    /// <summary>
    /// 
    /// </summary>
    public BlacklistReadingService(
        IGameReadingService gameReadingService,
        IIntentionManager intentionManager,
        IBlacklistReadingRepository repository)
    {
        this.gameReadingService = gameReadingService;
        this.intentionManager = intentionManager;
        this.repository = repository;
    }
        
    /// <inheritdoc />
    public async Task<IEnumerable<GeneralUser>> Get(Guid gameId)
    {
        var game = await gameReadingService.GetGame(gameId);
        intentionManager.ThrowIfForbidden(GameIntention.Edit, game);
        return await repository.Get(gameId);
    }
}