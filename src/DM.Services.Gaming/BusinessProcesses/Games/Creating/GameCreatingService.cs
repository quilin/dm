using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Games.AssistantAssignment;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;
using DM.Services.Gaming.BusinessProcesses.Games.Shared;
using DM.Services.Gaming.BusinessProcesses.Schemas.Reading;
using DM.Services.Gaming.Dto.Input;
using DM.Services.Gaming.Dto.Output;
using DM.Services.MessageQueuing.GeneralBus;
using FluentValidation;
using DbTag = DM.Services.DataAccess.BusinessObjects.Games.Links.GameTag;

namespace DM.Services.Gaming.BusinessProcesses.Games.Creating;

/// <inheritdoc />
internal class GameCreatingService(
    IValidator<CreateGame> validator,
    IIntentionManager intentionManager,
    IGameReadingService readingService,
    IAssignmentService assignmentService,
    IIdentityProvider identityProvider,
    IGameFactory gameFactory,
    IRoomFactory roomFactory,
    IGameTagFactory gameTagFactory,
    IGameCreatingRepository repository,
    IUserRepository userRepository,
    ISchemaReadingRepository schemaRepository,
    IUnreadCountersRepository countersRepository,
    IInvokedEventProducer producer) : IGameCreatingService
{
    /// <inheritdoc />
    public async Task<GameExtended> Create(CreateGame createGame, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(createGame, cancellationToken);
        intentionManager.ThrowIfForbidden(GameIntention.Create);

        // resolve game initial status
        var identity = identityProvider.Current;
        var initialStatus = identity.User.QuantityRating < 100
            ? GameStatus.RequiresModeration
            : createGame.Draft
                ? GameStatus.Draft
                : GameStatus.Requirement;

        // create base DAL entities
        var game = gameFactory.Create(createGame, identity.User.UserId, initialStatus);
        var room = roomFactory.Create(game.GameId);

        IEnumerable<DbTag> tags;
        if (createGame.Tags != null && createGame.Tags.Any())
        {
            var availableTags = (await readingService.GetTags(cancellationToken)).Select(t => t.Id).ToHashSet();
            tags = createGame.Tags
                .Where(availableTags.Contains)
                .Select(tagId => gameTagFactory.Create(game.GameId, tagId));
        }
        else
        {
            tags = Enumerable.Empty<DbTag>();
        }

        // initiate assistant assignment
        if (!string.IsNullOrEmpty(createGame.AssistantLogin))
        {
            var (assistantExists, foundAssistantId) = await userRepository.FindUserId(
                createGame.AssistantLogin, cancellationToken);
            if (assistantExists)
            {
                await assignmentService.CreateAssignment(game.GameId, foundAssistantId, cancellationToken);
            }
        }

        if (createGame.AttributeSchemaId.HasValue)
        {
            var schema = await schemaRepository.GetSchema(createGame.AttributeSchemaId.Value, cancellationToken);
            if (intentionManager.IsAllowed(AttributeSchemaIntention.Use, schema))
            {
                game.AttributeSchemaId = createGame.AttributeSchemaId.Value;
            }
        }

        var createdGame = await repository.Create(game, room, tags, cancellationToken);

        await countersRepository.Create(room.RoomId, UnreadEntryType.Message, cancellationToken);
        await countersRepository.Create(game.GameId, UnreadEntryType.Message, cancellationToken);
        await countersRepository.Create(game.GameId, UnreadEntryType.Character, cancellationToken);

        await producer.Send(EventType.NewGame, game.GameId);
        return createdGame;
    }
}