using System;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;
using DM.Services.Gaming.Dto.Output;
using DM.Services.MessageQueuing.Publish;
using Game = DM.Services.DataAccess.BusinessObjects.Games.Game;

namespace DM.Services.Gaming.BusinessProcesses.Games.Updating
{
    /// <inheritdoc />
    public class GameStatusService : IGameStatusService
    {
        private readonly IGameReadingService gameReadingService;
        private readonly IGameIntentionConverter intentionConverter;
        private readonly IIntentionManager intentionManager;
        private readonly IIdentity identity;
        private readonly IUpdateBuilderFactory updateBuilderFactory;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IGameUpdatingRepository repository;
        private readonly IInvokedEventPublisher publisher;

        /// <inheritdoc />
        public GameStatusService(
            IGameReadingService gameReadingService,
            IGameIntentionConverter intentionConverter,
            IIntentionManager intentionManager,
            IIdentityProvider identityProvider,
            IUpdateBuilderFactory updateBuilderFactory,
            IDateTimeProvider dateTimeProvider,
            IGameUpdatingRepository repository,
            IInvokedEventPublisher publisher)
        {
            this.gameReadingService = gameReadingService;
            this.intentionConverter = intentionConverter;
            this.intentionManager = intentionManager;
            identity = identityProvider.Current;
            this.updateBuilderFactory = updateBuilderFactory;
            this.dateTimeProvider = dateTimeProvider;
            this.repository = repository;
            this.publisher = publisher;
        }

        /// <inheritdoc />
        public async Task<GameExtended> UpdateStatus(Guid gameId, GameStatus gameStatus)
        {
            var game = await gameReadingService.GetGame(gameId);
            if (game.Status == gameStatus)
            {
                return game;
            }

            var (intention, eventType) = intentionConverter.Convert(gameStatus);
            await intentionManager.ThrowIfForbidden(intention, game);

            var changes = updateBuilderFactory.Create<Game>(gameId).Field(g => g.Status, gameStatus);

            if (gameStatus == GameStatus.Moderation) // when we go to moderation the actor becomes nanny
            {
                changes = changes.Field(g => g.NannyId, identity.User.UserId);
            }
            else if (game.Status == GameStatus.Moderation) // when we go from moderation the nanny is no more
            {
                changes = changes.Field(g => g.NannyId, null);
            }

            if (!game.ReleaseDate.HasValue && gameStatus == GameStatus.Requirement)
            {
                changes = changes.Field(g => g.ReleaseDate, dateTimeProvider.Now);
            }

            var result = await repository.Update(changes);
            await publisher.Publish(eventType, gameId);
            return result;
        }
    }
}