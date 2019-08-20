using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;
using DM.Services.Gaming.Dto.Input;
using DM.Services.Gaming.Dto.Output;
using DM.Services.MessageQueuing.Publish;
using FluentValidation;
using DbTag = DM.Services.DataAccess.BusinessObjects.Games.Links.GameTag;

namespace DM.Services.Gaming.BusinessProcesses.Games.Creating
{
    /// <inheritdoc />
    public class GameCreatingService : IGameCreatingService
    {
        private readonly IValidator<CreateGame> validator;
        private readonly IIntentionManager intentionManager;
        private readonly IGameReadingService readingService;
        private readonly IIdentity identity;
        private readonly IGameFactory gameFactory;
        private readonly IRoomFactory roomFactory;
        private readonly IGameTagFactory gameTagFactory;
        private readonly IGameCreatingRepository repository;
        private readonly IUnreadCountersRepository countersRepository;
        private readonly IInvokedEventPublisher publisher;

        /// <inheritdoc />
        public GameCreatingService(
            IValidator<CreateGame> validator,
            IIntentionManager intentionManager,
            IGameReadingService readingService,
            IIdentityProvider identityProvider,
            IGameFactory gameFactory,
            IRoomFactory roomFactory,
            IGameTagFactory gameTagFactory,
            IGameCreatingRepository repository,
            IUnreadCountersRepository countersRepository,
            IInvokedEventPublisher publisher)
        {
            this.validator = validator;
            this.intentionManager = intentionManager;
            this.readingService = readingService;
            identity = identityProvider.Current;
            this.gameFactory = gameFactory;
            this.roomFactory = roomFactory;
            this.gameTagFactory = gameTagFactory;
            this.repository = repository;
            this.countersRepository = countersRepository;
            this.publisher = publisher;
        }

        /// <inheritdoc />
        public async Task<GameExtended> Create(CreateGame createGame)
        {
            await validator.ValidateAndThrowAsync(createGame);
            await intentionManager.ThrowIfForbidden(GameIntention.Create);

            var initialStatus = identity.User.QuantityRating < 100
                ? GameStatus.RequiresModeration
                : createGame.Draft
                    ? GameStatus.Draft
                    : GameStatus.Requirement;

            Guid? assistantId = null;
            if (!string.IsNullOrEmpty(createGame.AssistantLogin))
            {
                var (assistantExists, foundAssistantId) = await repository.FindUserId(createGame.AssistantLogin);
                if (assistantExists)
                {
                    assistantId = foundAssistantId;
                }
            }

            var game = gameFactory.Create(createGame, identity.User.UserId, assistantId, initialStatus);
            var room = roomFactory.Create(game.GameId);
            IEnumerable<DbTag> tags;
            if (createGame.Tags != null && createGame.Tags.Any())
            {
                var availableTags = (await readingService.GetTags()).Select(t => t.Id).ToHashSet();
                tags = createGame.Tags
                    .Where(t => availableTags.Contains(t))
                    .Select(t => gameTagFactory.Create(game.GameId, t));
            }
            else
            {
                tags = Enumerable.Empty<DbTag>();
            }

            var createdGame = await repository.Create(game, room, tags);

            await countersRepository.Create(game.GameId, UnreadEntryType.Message);
            await countersRepository.Create(game.GameId, UnreadEntryType.Character);

            await publisher.Publish(EventType.NewGame, game.GameId);
            return createdGame;
        }
    }
}