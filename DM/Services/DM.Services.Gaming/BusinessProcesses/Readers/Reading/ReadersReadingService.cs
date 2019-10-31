using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Core.Dto;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;

namespace DM.Services.Gaming.BusinessProcesses.Readers.Reading
{
    /// <inheritdoc />
    public class ReadersReadingService : IReadersReadingService
    {
        private readonly IGameReadingService gameReadingService;
        private readonly IReadersReadingRepository repository;
        private readonly IIdentity identity;

        /// <inheritdoc />
        public ReadersReadingService(
            IGameReadingService gameReadingService,
            IReadersReadingRepository repository,
            IIdentityProvider identityProvider)
        {
            this.gameReadingService = gameReadingService;
            this.repository = repository;
            identity = identityProvider.Current;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<GeneralUser>> Get(Guid gameId)
        {
            await gameReadingService.GetGame(gameId);
            return await repository.Get(gameId, identity.User.UserId);
        }
    }
}