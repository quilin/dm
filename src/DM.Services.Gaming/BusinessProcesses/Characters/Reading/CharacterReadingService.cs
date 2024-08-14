using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Core.Exceptions;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.Gaming.BusinessProcesses.Characters.Shared;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.BusinessProcesses.Characters.Reading;

/// <inheritdoc />
internal class CharacterReadingService : ICharacterReadingService
{
    private readonly IGameReadingService gameReadingService;
    private readonly ICharacterReadingRepository readingRepository;
    private readonly ICharacterAttributeValueFiller attributeValueFiller;
    private readonly IUnreadCountersRepository unreadCountersRepository;
    private readonly IIdentityProvider identityProvider;

    /// <inheritdoc />
    public CharacterReadingService(
        IGameReadingService gameReadingService,
        ICharacterReadingRepository readingRepository,
        ICharacterAttributeValueFiller attributeValueFiller,
        IUnreadCountersRepository unreadCountersRepository,
        IIdentityProvider identityProvider)
    {
        this.gameReadingService = gameReadingService;
        this.readingRepository = readingRepository;
        this.attributeValueFiller = attributeValueFiller;
        this.unreadCountersRepository = unreadCountersRepository;
        this.identityProvider = identityProvider;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Character>> GetCharacters(Guid gameId)
    {
        var game = await gameReadingService.GetGame(gameId);
        var characters = (await readingRepository.GetCharacters(gameId)).ToArray();
        await attributeValueFiller.Fill(characters, game.AttributeSchemaId);
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
        await attributeValueFiller.Fill(new[] {character}, game.AttributeSchemaId);
        return character;
    }

    /// <inheritdoc />
    public async Task MarkAsRead(Guid gameId)
    {
        await gameReadingService.GetGame(gameId);
        await unreadCountersRepository.Flush(identityProvider.Current.User.UserId,
            UnreadEntryType.Character, gameId);
    }
}