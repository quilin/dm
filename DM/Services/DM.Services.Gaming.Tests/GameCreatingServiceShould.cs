using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.BusinessObjects.Games.Links;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Games.Creating;
using DM.Services.Gaming.Dto.Input;
using DM.Services.Gaming.Dto.Output;
using DM.Services.MessageQueuing.Publish;
using DM.Tests.Core;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Moq.Language.Flow;
using Xunit;
using Game = DM.Services.DataAccess.BusinessObjects.Games.Game;
using Room = DM.Services.DataAccess.BusinessObjects.Games.Posts.Room;

namespace DM.Services.Gaming.Tests
{
    public class GameCreatingServiceShould : UnitTestBase
    {
        private readonly ISetup<IIdentity, AuthenticatedUser> currentUserSetup;
        private readonly ISetup<IGameFactory, (Game game, Room room,
            IEnumerable<GameTag> tags)> createGameSetup;
        private readonly ISetup<IGameCreatingRepository, Task<GameExtended>> saveGameSetup;
        private readonly Mock<IGameCreatingRepository> gameRepository;
        private readonly Mock<IGameFactory> factory;
        private readonly Mock<IIntentionManager> intentionManager;
        private readonly Mock<IInvokedEventPublisher> publisher;
        private readonly GameCreatingService service;
        private readonly Mock<IUnreadCountersRepository> countersRepository;

        public GameCreatingServiceShould()
        {
            var validator = Mock<IValidator<CreateGame>>();
            validator
                .Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<CreateGame>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            intentionManager = Mock<IIntentionManager>();
            intentionManager
                .Setup(m => m.ThrowIfForbidden(It.IsAny<GameIntention>()))
                .Returns(Task.CompletedTask);

            var identityProvider = Mock<IIdentityProvider>();
            var identity = Mock<IIdentity>();
            identityProvider.Setup(p => p.Current).Returns(identity.Object);
            currentUserSetup = identity.Setup(i => i.User);

            factory = Mock<IGameFactory>();
            createGameSetup = factory
                .Setup(f => f.Create(It.IsAny<CreateGame>(), It.IsAny<Guid>(), It.IsAny<Guid?>(), It.IsAny<GameStatus>()));

            gameRepository = Mock<IGameCreatingRepository>();
            saveGameSetup = gameRepository
                .Setup(r => r.Create(It.IsAny<Game>(), It.IsAny<Room>(),
                    It.IsAny<IEnumerable<GameTag>>()));

            countersRepository = Mock<IUnreadCountersRepository>();
            countersRepository
                .Setup(r => r.Create(It.IsAny<Guid>(), It.IsAny<UnreadEntryType>()))
                .Returns(Task.CompletedTask);

            publisher = Mock<IInvokedEventPublisher>();
            publisher
                .Setup(p => p.Publish(It.IsAny<EventType>(), It.IsAny<Guid>()))
                .Returns(Task.CompletedTask);

            service = new GameCreatingService(validator.Object,
                intentionManager.Object,
                identityProvider.Object,
                factory.Object,
                gameRepository.Object,
                countersRepository.Object,
                publisher.Object);
        }

        [Fact]
        public async Task CheckAuthorization()
        {
            currentUserSetup.Returns(new AuthenticatedUser());
            createGameSetup.Returns((new Game(), new Room(), new List<GameTag>()));
            saveGameSetup.ReturnsAsync(new GameExtended());

            await service.Create(new CreateGame());

            intentionManager.Verify(m => m.ThrowIfForbidden(GameIntention.Create), Times.Once);
        }

        [Fact]
        public async Task CreateModerationRequiredGameWhenUserHasLowRating()
        {
            var userId = Guid.NewGuid();
            currentUserSetup.Returns(new AuthenticatedUser
            {
                UserId = userId,
                QuantityRating = 99
            });
            createGameSetup.Returns((new Game(), new Room(), new List<GameTag>()));
            saveGameSetup.ReturnsAsync(new GameExtended());

            var createGame = new CreateGame();
            await service.Create(createGame);
            factory.Verify(f => f.Create(createGame, userId, It.IsAny<Guid?>(), GameStatus.RequiresModeration));
        }

        [Theory]
        [InlineData(true, GameStatus.Draft)]
        [InlineData(false, GameStatus.Requirement)]
        public async Task CreateInDesiredStatusWhenUserHasHighRating(bool draft, GameStatus status)
        {
            var userId = Guid.NewGuid();
            currentUserSetup.Returns(new AuthenticatedUser
            {
                UserId = userId,
                QuantityRating = 100
            });
            createGameSetup.Returns((new Game(), new Room(), new List<GameTag>()));
            saveGameSetup.ReturnsAsync(new GameExtended());

            var createGame = new CreateGame
            {
                Draft = draft
            };
            await service.Create(createGame);
            factory.Verify(f => f.Create(createGame, userId, It.IsAny<Guid?>(), status));
        }

        [Fact]
        public async Task SaveCreatedGameAndRoomInStorage()
        {
            currentUserSetup.Returns(new AuthenticatedUser());
            var game = new Game();
            var room = new Room();
            var tags = new List<GameTag>();
            createGameSetup.Returns((game, room, tags));
            saveGameSetup.ReturnsAsync(new GameExtended());

            await service.Create(new CreateGame());

            gameRepository.Verify(r => r.Create(game, room ,tags), Times.Once);
            gameRepository.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(UnreadEntryType.Message)]
        [InlineData(UnreadEntryType.Character)]
        public async Task CreateCountersForGameEntries(UnreadEntryType entryType)
        {
            currentUserSetup.Returns(new AuthenticatedUser());
            var gameId = Guid.NewGuid();
            createGameSetup.Returns((new Game
            {
                GameId = gameId
            }, new Room(), new List<GameTag>()));
            saveGameSetup.ReturnsAsync(new GameExtended());

            await service.Create(new CreateGame());

            countersRepository.Verify(r => r.Create(gameId, entryType));
        }

        [Fact]
        public async Task PublishMessage()
        {
            currentUserSetup.Returns(new AuthenticatedUser());
            var gameId = Guid.NewGuid();
            createGameSetup.Returns((new Game{GameId = gameId}, new Room(), new List<GameTag>()));
            saveGameSetup.ReturnsAsync(new GameExtended());

            await service.Create(new CreateGame());

            publisher.Verify(p => p.Publish(EventType.NewGame, gameId), Times.Once);
            publisher.VerifyNoOtherCalls();
        }
    }
}