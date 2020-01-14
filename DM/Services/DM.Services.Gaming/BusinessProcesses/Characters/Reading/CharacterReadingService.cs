using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Core.Exceptions;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;
using DM.Services.Gaming.BusinessProcesses.Schemas.Reading;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.BusinessProcesses.Characters.Reading
{
    /// <inheritdoc />
    public class CharacterReadingService : ICharacterReadingService
    {
        private readonly IGameReadingService gameReadingService;
        private readonly ICharacterReadingRepository readingRepository;
        private readonly ISchemaReadingRepository schemaReadingRepository;
        private readonly IUnreadCountersRepository unreadCountersRepository;
        private readonly IIdentity identity;

        /// <inheritdoc />
        public CharacterReadingService(
            IGameReadingService gameReadingService,
            ICharacterReadingRepository readingRepository,
            ISchemaReadingRepository schemaReadingRepository,
            IUnreadCountersRepository unreadCountersRepository,
            IIdentityProvider identityProvider)
        {
            this.gameReadingService = gameReadingService;
            this.readingRepository = readingRepository;
            this.schemaReadingRepository = schemaReadingRepository;
            this.unreadCountersRepository = unreadCountersRepository;
            identity = identityProvider.Current;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Character>> GetCharacters(Guid gameId)
        {
            var game = await gameReadingService.GetGame(gameId);
            var characters = (await readingRepository.GetCharacters(gameId)).ToArray();
            await schemaReadingRepository.FillAttributeValues(game.AttributeSchemaId, characters);
            return characters;
        }

        /// <inheritdoc />
        public async Task<Character> GetCharacter(Guid characterId)
        {
            var character = await readingRepository.FindCharacter(characterId);
            if (character == null)
            {
                throw new HttpException(HttpStatusCode.Gone, "Character not found");
            }

            var game = await gameReadingService.GetGame(character.GameId);
            await schemaReadingRepository.FillAttributeValues(game.AttributeSchemaId, new[] {character});
            return character;
        }

        /// <inheritdoc />
        public async Task MarkAsRead(Guid gameId)
        {
            await gameReadingService.GetGame(gameId);
            await unreadCountersRepository.Flush(identity.User.UserId, UnreadEntryType.Character, gameId);
        }
    }
}