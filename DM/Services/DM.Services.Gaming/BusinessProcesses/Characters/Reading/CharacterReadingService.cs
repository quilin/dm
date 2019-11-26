using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Core.Exceptions;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.BusinessProcesses.Characters.Reading
{
    /// <inheritdoc />
    public class CharacterReadingService : ICharacterReadingService
    {
        private readonly IGameReadingService gameReadingService;
        private readonly ICharacterReadingRepository readingRepository;
        private readonly IUnreadCountersRepository unreadCountersRepository;
        private readonly IIdentity identity;

        /// <inheritdoc />
        public CharacterReadingService(
            IGameReadingService gameReadingService,
            ICharacterReadingRepository readingRepository,
            IUnreadCountersRepository unreadCountersRepository,
            IIdentityProvider identityProvider)
        {
            this.gameReadingService = gameReadingService;
            this.readingRepository = readingRepository;
            this.unreadCountersRepository = unreadCountersRepository;
            identity = identityProvider.Current;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Character>> GetCharacters(Guid gameId)
        {
            await gameReadingService.GetGame(gameId);
            return await readingRepository.GetCharacters(gameId);
        }

        /// <inheritdoc />
        public async Task<Character> GetCharacter(Guid characterId)
        {
            var character = await readingRepository.FindCharacter(characterId);
            if (character == null)
            {
                throw new HttpException(HttpStatusCode.Gone, "Character not found");
            }

            await gameReadingService.GetGame(character.GameId);
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