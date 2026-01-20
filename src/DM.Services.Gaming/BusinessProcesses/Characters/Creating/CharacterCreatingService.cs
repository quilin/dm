using System.Threading;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;
using DM.Services.Gaming.Dto;
using DM.Services.Gaming.Dto.Input;
using DM.Services.Gaming.Dto.Output;
using DM.Services.MessageQueuing.GeneralBus;
using FluentValidation;

namespace DM.Services.Gaming.BusinessProcesses.Characters.Creating;

/// <inheritdoc />
internal class CharacterCreatingService(
    IValidator<CreateCharacter> validator,
    IGameReadingService gameReadingService,
    IIntentionManager intentionManager,
    ICharacterFactory factory,
    ICharacterCreatingRepository creatingRepository,
    IUnreadCountersRepository unreadCountersRepository,
    IInvokedEventProducer producer,
    IIdentityProvider identityProvider) : ICharacterCreatingService
{
    /// <inheritdoc />
    public async Task<Character> Create(CreateCharacter createCharacter, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(createCharacter, cancellationToken);
        var game = await gameReadingService.GetGame(createCharacter.GameId, cancellationToken);
        intentionManager.ThrowIfForbidden(GameIntention.CreateCharacter, game);

        var currentUserId = identityProvider.Current.User.UserId;
        var gameParticipation = game.Participation(currentUserId);

        // Master and assistant characters should be created in Active status
        var initialStatus = gameParticipation.HasFlag(GameParticipation.Authority)
            ? CharacterStatus.Active
            : CharacterStatus.Registration;

        // Only master and assistant are allowed to create NPCs
        createCharacter.IsNpc = createCharacter.IsNpc && gameParticipation.HasFlag(GameParticipation.Authority);

        var (character, attributes) = factory.Create(createCharacter, currentUserId, initialStatus);
        var createdCharacter = await creatingRepository.Create(character, attributes, cancellationToken);

        await unreadCountersRepository.Increment(createCharacter.GameId, UnreadEntryType.Character, cancellationToken);
        await producer.Send(EventType.NewCharacter, createdCharacter.Id);

        return createdCharacter;
    }
}