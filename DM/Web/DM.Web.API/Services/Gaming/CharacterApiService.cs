using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Gaming.BusinessProcesses.Characters.Creating;
using DM.Services.Gaming.BusinessProcesses.Characters.Reading;
using DM.Services.Gaming.Dto.Input;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Games;

namespace DM.Web.API.Services.Gaming
{
    /// <inheritdoc />
    public class CharacterApiService : ICharacterApiService
    {
        private readonly ICharacterReadingService readingService;
        private readonly ICharacterCreatingService creatingService;
        private readonly IMapper mapper;

        /// <inheritdoc />
        public CharacterApiService(
            ICharacterReadingService readingService,
            ICharacterCreatingService creatingService,
            IMapper mapper)
        {
            this.readingService = readingService;
            this.creatingService = creatingService;
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
        public Task<Envelope<Character>> Update(Guid characterId, Character character)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task Delete(Guid characterId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task MarkAsRead(Guid gameId) => readingService.MarkAsRead(gameId);
    }
}