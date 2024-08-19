using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Core.Dto;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;

namespace DM.Services.Gaming.BusinessProcesses.Readers.Reading;

/// <inheritdoc />
internal class ReadersReadingService(
    IGameReadingService gameReadingService,
    IReadersReadingRepository repository,
    IIdentityProvider identityProvider) : IReadersReadingService
{
    /// <inheritdoc />
    public async Task<IEnumerable<GeneralUser>> Get(Guid gameId, CancellationToken cancellationToken)
    {
        await gameReadingService.GetGame(gameId, cancellationToken);
        return await repository.Get(gameId, identityProvider.Current.User.UserId, cancellationToken);
    }
}