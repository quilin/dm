using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Core.Dto.Enums;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;
using DM.Services.Gaming.Dto.Input;
using DM.Services.Gaming.Dto.Output;
using DM.Services.MessageQueuing.Publish;
using FluentValidation;

namespace DM.Services.Gaming.BusinessProcesses.Characters.Creating
{
    /// <inheritdoc />
    public class CharacterCreatingService : ICharacterCreatingService
    {
        private readonly IValidator<CreateCharacter> validator;
        private readonly IGameReadingService gameReadingService;
        private readonly IIntentionManager intentionManager;
        private readonly ICharacterFactory factory;
        private readonly ICharacterCreatingRepository creatingRepository;
        private readonly IInvokedEventPublisher publisher;
        private readonly IIdentity identity;

        /// <inheritdoc />
        public CharacterCreatingService(
            IValidator<CreateCharacter> validator,
            IGameReadingService gameReadingService,
            IIntentionManager intentionManager,
            ICharacterFactory factory,
            ICharacterCreatingRepository creatingRepository,
            IInvokedEventPublisher publisher,
            IIdentityProvider identityProvider)
        {
            this.validator = validator;
            this.gameReadingService = gameReadingService;
            this.intentionManager = intentionManager;
            this.factory = factory;
            this.creatingRepository = creatingRepository;
            this.publisher = publisher;
            identity = identityProvider.Current;
        }
        
        /// <inheritdoc />
        public async Task<Character> Create(CreateCharacter createCharacter)
        {
            await validator.ValidateAndThrowAsync(createCharacter);
            var game = await gameReadingService.GetGame(createCharacter.GameId);
            await intentionManager.ThrowIfForbidden(GameIntention.CreateCharacter, game);

            var currentUserId = identity.User.UserId;
            var initialStatus = game.Master.UserId == currentUserId || game.Assistant?.UserId == currentUserId
                ? CharacterStatus.Active
                : CharacterStatus.Registration;
            var character = factory.Create(createCharacter, currentUserId, initialStatus);
            var createdCharacter = await creatingRepository.Create(character);
            await publisher.Publish(EventType.NewCharacter, createdCharacter.Id);

            return createdCharacter;
        }
    }
}