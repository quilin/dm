using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Gaming.BusinessProcesses.Characters.Creating;
using DM.Services.Gaming.BusinessProcesses.Characters.Deleting;
using DM.Services.Gaming.BusinessProcesses.Characters.Reading;
using DM.Services.Gaming.BusinessProcesses.Characters.Updating;
using DM.Services.Gaming.Dto.Input;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Games;

namespace DM.Web.API.Services.Gaming;

/// <inheritdoc />
internal class CharacterApiService(
    ICharacterReadingService readingService,
    ICharacterCreatingService creatingService,
    ICharacterUpdatingService updatingService,
    ICharacterDeletingService deletingService,
    IMapper mapper) : ICharacterApiService
{
    /// <inheritdoc />
    public async Task<ListEnvelope<Character>> GetAll(Guid gameId, CancellationToken cancellationToken)
    {
        var characters = await readingService.GetCharacters(gameId, cancellationToken);
        return new ListEnvelope<Character>(characters.Select(mapper.Map<Character>));
    }

    /// <inheritdoc />
    public async Task<Envelope<Character>> Get(Guid characterId, CancellationToken cancellationToken)
    {
        var character = await readingService.GetCharacter(characterId, cancellationToken);
        return new Envelope<Character>(mapper.Map<Character>(character));
    }

    /// <inheritdoc />
    public async Task<Envelope<Character>> Create(
        Guid gameId, Character character, CancellationToken cancellationToken)
    {
        var createCharacter = mapper.Map<CreateCharacter>(character);
        createCharacter.GameId = gameId;
        var createdCharacter = await creatingService.Create(createCharacter, cancellationToken);
        return new Envelope<Character>(mapper.Map<Character>(createdCharacter));
    }

    /// <inheritdoc />
    public async Task<Envelope<Character>> Update(
        Guid characterId, Character character, CancellationToken cancellationToken)
    {
        var updateCharacter = mapper.Map<UpdateCharacter>(character);
        updateCharacter.CharacterId = characterId;
        var updatedCharacter = await updatingService.Update(updateCharacter, cancellationToken);
        return new Envelope<Character>(mapper.Map<Character>(updatedCharacter));
    }

    /// <inheritdoc />
    public Task Delete(Guid characterId, CancellationToken cancellationToken) =>
        deletingService.Delete(characterId, cancellationToken);

    /// <inheritdoc />
    public Task MarkAsRead(Guid gameId, CancellationToken cancellationToken) =>
        readingService.MarkAsRead(gameId, cancellationToken);
}