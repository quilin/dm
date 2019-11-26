using System;
using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.BusinessObjects.Games.Characters;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Characters.Updating;
using DM.Services.MessageQueuing.Publish;

namespace DM.Services.Gaming.BusinessProcesses.Characters.Deleting
{
    /// <inheritdoc />
    public class CharacterDeletingService : ICharacterDeletingService
    {
        private readonly IIntentionManager intentionManager;
        private readonly IUpdateBuilderFactory updateBuilderFactory;
        private readonly ICharacterUpdatingRepository repository;
        private readonly IUnreadCountersRepository unreadCountersRepository;
        private readonly IInvokedEventPublisher publisher;

        /// <inheritdoc />
        public CharacterDeletingService(
            IIntentionManager intentionManager,
            IUpdateBuilderFactory updateBuilderFactory,
            ICharacterUpdatingRepository repository,
            IUnreadCountersRepository unreadCountersRepository,
            IInvokedEventPublisher publisher)
        {
            this.intentionManager = intentionManager;
            this.updateBuilderFactory = updateBuilderFactory;
            this.repository = repository;
            this.unreadCountersRepository = unreadCountersRepository;
            this.publisher = publisher;
        }
        
        /// <inheritdoc />
        public async Task Delete(Guid characterId)
        {
            var character = await repository.Get(characterId);
            await intentionManager.ThrowIfForbidden(CharacterIntention.Delete, character);

            var updateCharacter = updateBuilderFactory.Create<Character>(characterId);
            updateCharacter.Field(c => c.IsRemoved, true);

            await repository.Update(updateCharacter);
            await unreadCountersRepository.Decrement(character.GameId, UnreadEntryType.Character, character.CreateDate);
            await publisher.Publish(EventType.DeletedCharacter, characterId);
        }
    }
}