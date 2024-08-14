using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Core.Dto;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;

namespace DM.Services.Gaming.BusinessProcesses.Readers.Reading;

/// <inheritdoc />
internal class ReadersReadingService : IReadersReadingService
{
    private readonly IGameReadingService gameReadingService;
    private readonly IReadersReadingRepository repository;
    private readonly IIdentityProvider identityProvider;

    /// <inheritdoc />
    public ReadersReadingService(
        IGameReadingService gameReadingService,
        IReadersReadingRepository repository,
        IIdentityProvider identityProvider)
    {
        this.gameReadingService = gameReadingService;
        this.repository = repository;
        this.identityProvider = identityProvider;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GeneralUser>> Get(Guid gameId)
    {
        await gameReadingService.GetGame(gameId);
        return await repository.Get(gameId, identityProvider.Current.User.UserId);
    }
}