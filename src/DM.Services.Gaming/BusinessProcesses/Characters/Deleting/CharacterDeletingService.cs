using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.BusinessObjects.Games.Characters;
using DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Characters.Updating;
using DM.Services.MessageQueuing.GeneralBus;

namespace DM.Services.Gaming.BusinessProcesses.Characters.Deleting;

/// <inheritdoc />
internal class CharacterDeletingService(
    IIntentionManager intentionManager,
    IUpdateBuilderFactory updateBuilderFactory,
    ICharacterUpdatingRepository repository,
    IUnreadCountersRepository unreadCountersRepository,
    IInvokedEventProducer producer) : ICharacterDeletingService
{
    /// <inheritdoc />
    public async Task Delete(Guid characterId, CancellationToken cancellationToken)
    {
        var character = await repository.Get(characterId, cancellationToken);
        intentionManager.ThrowIfForbidden(CharacterIntention.Delete, character);

        var updateCharacter = updateBuilderFactory.Create<Character>(characterId);
        updateCharacter.Field(c => c.IsRemoved, true);
        var updateAttributes = (await repository.GetAttributeIds(characterId, cancellationToken)).Keys
            .Select(id => updateBuilderFactory.Create<CharacterAttribute>(id).Delete());

        await repository.Update(updateCharacter, updateAttributes, cancellationToken);
        await unreadCountersRepository.Decrement(
            character.GameId, UnreadEntryType.Character, character.CreateDate, cancellationToken);
        await producer.Send(EventType.DeletedCharacter, characterId);
    }
}