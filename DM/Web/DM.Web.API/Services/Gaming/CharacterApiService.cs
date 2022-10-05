using System;
using System.Linq;
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
internal class CharacterApiService : ICharacterApiService
{
    private readonly ICharacterReadingService readingService;
    private readonly ICharacterCreatingService creatingService;
    private readonly ICharacterUpdatingService updatingService;
    private readonly ICharacterDeletingService deletingService;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public CharacterApiService(
        ICharacterReadingService readingService,
        ICharacterCreatingService creatingService,
        ICharacterUpdatingService updatingService,
        ICharacterDeletingService deletingService,
        IMapper mapper)
    {
        this.readingService = readingService;
        this.creatingService = creatingService;
        this.updatingService = updatingService;
        this.deletingService = deletingService;
        this.mapper = mapper;
    }
        
    /// <inheritdoc />
    public async Task<ListEnvelope<Character>> GetAll(Guid gameId)
    {
        var characters = await readingService.GetCharacters(gameId);
        return new ListEnvelope<Character>(characters.Select(mapper.Map<Character>));
    }

    /// <inheritdoc />
    public async Task<Envelope<Character>> Get(Guid characterId)
    {
        var character = await readingService.GetCharacter(characterId);
        return new Envelope<Character>(mapper.Map<Character>(character));
    }

    /// <inheritdoc />
    public async Task<Envelope<Character>> Create(Guid gameId, Character character)
    {
        var createCharacter = mapper.Map<CreateCharacter>(character);
        createCharacter.GameId = gameId;
        var createdCharacter = await creatingService.Create(createCharacter);
        return new Envelope<Character>(mapper.Map<Character>(createdCharacter));
    }

    /// <inheritdoc />
    public async Task<Envelope<Character>> Update(Guid characterId, Character character)
    {
        var updateCharacter = mapper.Map<UpdateCharacter>(character);
        updateCharacter.CharacterId = characterId;
        var updatedCharacter = await updatingService.Update(updateCharacter);
        return new Envelope<Character>(mapper.Map<Character>(updatedCharacter));
    }

    /// <inheritdoc />
    public Task Delete(Guid characterId) => deletingService.Delete(characterId);

    /// <inheritdoc />
    public Task MarkAsRead(Guid gameId) => readingService.MarkAsRead(gameId);
}