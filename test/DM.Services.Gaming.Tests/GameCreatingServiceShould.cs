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
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Games.AssistantAssignment;
using DM.Services.Gaming.BusinessProcesses.Games.Creating;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;
using DM.Services.Gaming.BusinessProcesses.Games.Shared;
using DM.Services.Gaming.BusinessProcesses.Schemas.Reading;
using DM.Services.Gaming.Dto.Input;
using DM.Services.Gaming.Dto.Output;
using DM.Services.MessageQueuing.GeneralBus;
using DM.Tests.Core;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Moq.Language.Flow;
using Xunit;
using Game = DM.Services.DataAccess.BusinessObjects.Games.Game;
using Room = DM.Services.DataAccess.BusinessObjects.Games.Posts.Room;
using GameTag = DM.Services.DataAccess.BusinessObjects.Games.Links.GameTag;

namespace DM.Services.Gaming.Tests;

public class GameCreatingServiceShould : UnitTestBase
{
    private readonly ISetup<IIdentity, AuthenticatedUser> currentUserSetup;
    private readonly ISetup<IGameFactory, Game> createGameSetup;
    private readonly ISetup<IRoomFactory, Room> createRoomSetup;
    private readonly ISetup<IGameCreatingRepository, Task<GameExtended>> saveGameSetup;
    private readonly Mock<IGameCreatingRepository> gameRepository;
    private readonly Mock<IGameFactory> gameFactory;
    private readonly Mock<IIntentionManager> intentionManager;
    private readonly Mock<IInvokedEventProducer> publisher;
    private readonly GameCreatingService service;
    private readonly Mock<IUnreadCountersRepository> countersRepository;
    private readonly Mock<IUserRepository> userRepository;
    private readonly Mock<IAssignmentService> assignmentService;

    public GameCreatingServiceShould()
    {
        var validator = Mock<IValidator<CreateGame>>();
        validator
            .Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<CreateGame>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var readingService = Mock<IGameReadingService>();
        readingService.Setup(s => s.GetTags());

        assignmentService = Mock<IAssignmentService>();
        assignmentService
            .Setup(s => s.CreateAssignment(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .Returns(Task.CompletedTask);

        intentionManager = Mock<IIntentionManager>();
        intentionManager
            .Setup(m => m.ThrowIfForbidden(It.IsAny<GameIntention>()));

        var identityProvider = Mock<IIdentityProvider>();
        var identity = Mock<IIdentity>();
        identityProvider.Setup(p => p.Current).Returns(identity.Object);
        currentUserSetup = identity.Setup(i => i.User);

        gameFactory = Mock<IGameFactory>();
        createGameSetup = gameFactory
            .Setup(f => f.Create(It.IsAny<CreateGame>(), It.IsAny<Guid>(), It.IsAny<GameStatus>()));

        var roomFactory = Mock<IRoomFactory>();
        createRoomSetup = roomFactory.Setup(r => r.Create(It.IsAny<Guid>()));

        var gameTagFactory = Mock<IGameTagFactory>();

        userRepository = Mock<IUserRepository>();

        gameRepository = Mock<IGameCreatingRepository>();
        saveGameSetup = gameRepository
            .Setup(r => r.Create(It.IsAny<Game>(), It.IsAny<Room>(),
                It.IsAny<IEnumerable<GameTag>>()));

        countersRepository = Mock<IUnreadCountersRepository>();
        countersRepository
            .Setup(r => r.Create(It.IsAny<Guid>(), It.IsAny<UnreadEntryType>()))
            .Returns(Task.CompletedTask);

        var schemaRepository = Mock<ISchemaReadingRepository>();

        publisher = Mock<IInvokedEventProducer>();
        publisher
            .Setup(p => p.Send(It.IsAny<EventType>(), It.IsAny<Guid>()))
            .Returns(Task.CompletedTask);

        service = new GameCreatingService(validator.Object,
            intentionManager.Object,
            readingService.Object,
            assignmentService.Object,
            identityProvider.Object,
            gameFactory.Object,
            roomFactory.Object,
            gameTagFactory.Object,
            gameRepository.Object,
            userRepository.Object,
            schemaRepository.Object,
            countersRepository.Object,
            publisher.Object);
    }

    [Fact]
    public async Task CheckAuthorization()
    {
        currentUserSetup.Returns(new AuthenticatedUser());
        createGameSetup.Returns(new Game());
        createRoomSetup.Returns(new Room());
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
        createGameSetup.Returns(new Game());
        createRoomSetup.Returns(new Room());
        saveGameSetup.ReturnsAsync(new GameExtended());

        var createGame = new CreateGame();
        await service.Create(createGame);
        gameFactory.Verify(f => f.Create(createGame, userId, GameStatus.RequiresModeration));
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
        createGameSetup.Returns(new Game());
        createRoomSetup.Returns(new Room());
        saveGameSetup.ReturnsAsync(new GameExtended());

        var createGame = new CreateGame
        {
            Draft = draft
        };
        await service.Create(createGame);
        gameFactory.Verify(f => f.Create(createGame, userId, status));
    }

    [Fact]
    public async Task SearchForAssistantWhenLoginGiven()
    {
        currentUserSetup.Returns(new AuthenticatedUser());
        var game = new Game();
        var room = new Room();
        createGameSetup.Returns(game);
        createRoomSetup.Returns(room);
        saveGameSetup.ReturnsAsync(new GameExtended());
        userRepository
            .Setup(r => r.FindUserId(It.IsAny<string>()))
            .ReturnsAsync((false, Guid.Empty));

        var createGame = new CreateGame {AssistantLogin = "assistant boi"};
        await service.Create(createGame);

        userRepository.Verify(r => r.FindUserId("assistant boi"));
    }

    [Fact]
    public async Task CreateGameWithAssistantTokenWhenFound()
    {
        currentUserSetup.Returns(new AuthenticatedUser());
        var gameId = Guid.NewGuid();
        var game = new Game {GameId = gameId};
        var room = new Room();
        createGameSetup.Returns(game);
        createRoomSetup.Returns(room);
        saveGameSetup.ReturnsAsync(new GameExtended());
        var assistantId = Guid.NewGuid();
        userRepository
            .Setup(r => r.FindUserId(It.IsAny<string>()))
            .ReturnsAsync((true, assistantId));

        await service.Create(new CreateGame {AssistantLogin = "assistant boi"});

        assignmentService.Verify(s => s.CreateAssignment(gameId, assistantId), Times.Once);
    }

    [Fact]
    public async Task SaveCreatedGameAndRoomInStorage()
    {
        currentUserSetup.Returns(new AuthenticatedUser());
        var game = new Game();
        var room = new Room();
        createGameSetup.Returns(game);
        createRoomSetup.Returns(room);
        saveGameSetup.ReturnsAsync(new GameExtended());

        await service.Create(new CreateGame());

        gameRepository.Verify(r => r.Create(game, room, It.IsAny<IEnumerable<GameTag>>()), Times.Once);
        gameRepository.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(UnreadEntryType.Message)]
    [InlineData(UnreadEntryType.Character)]
    public async Task CreateCountersForGameEntries(UnreadEntryType entryType)
    {
        currentUserSetup.Returns(new AuthenticatedUser());
        var gameId = Guid.NewGuid();
        createGameSetup.Returns(new Game {GameId = gameId});
        createRoomSetup.Returns(new Room());
        saveGameSetup.ReturnsAsync(new GameExtended());

        await service.Create(new CreateGame());

        countersRepository.Verify(r => r.Create(gameId, entryType), Times.Once);
    }

    [Fact]
    public async Task CreateCountersForRoomEntries()
    {
        currentUserSetup.Returns(new AuthenticatedUser());
        var roomId = Guid.NewGuid();
        createGameSetup.Returns(new Game {GameId = Guid.NewGuid()});
        createRoomSetup.Returns(new Room {RoomId = roomId});
        saveGameSetup.ReturnsAsync(new GameExtended());

        await service.Create(new CreateGame());

        countersRepository.Verify(r => r.Create(roomId, UnreadEntryType.Message), Times.Once);
    }

    [Fact]
    public async Task PublishMessage()
    {
        currentUserSetup.Returns(new AuthenticatedUser());
        var gameId = Guid.NewGuid();
        createGameSetup.Returns(new Game {GameId = gameId});
        createRoomSetup.Returns(new Room());
        saveGameSetup.ReturnsAsync(new GameExtended());

        await service.Create(new CreateGame());

        publisher.Verify(p => p.Send(EventType.NewGame, gameId), Times.Once);
        publisher.VerifyNoOtherCalls();
    }
}