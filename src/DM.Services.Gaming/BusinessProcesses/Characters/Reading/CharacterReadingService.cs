using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
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
internal class CharacterReadingService(
    IGameReadingService gameReadingService,
    ICharacterReadingRepository readingRepository,
    ICharacterAttributeValueFiller attributeValueFiller,
    IUnreadCountersRepository unreadCountersRepository,
    IIdentityProvider identityProvider) : ICharacterReadingService
{
    /// <inheritdoc />
    public async Task<IEnumerable<Character>> GetCharacters(Guid gameId, CancellationToken cancellationToken)
    {
        var game = await gameReadingService.GetGame(gameId, cancellationToken);
        var characters = (await readingRepository.GetCharacters(gameId, cancellationToken)).ToArray();
        await attributeValueFiller.Fill(characters, game.AttributeSchemaId, cancellationToken);
        return characters;
    }

    /// <inheritdoc />
    public async Task<Character> GetCharacter(Guid characterId, CancellationToken cancellationToken)
    {
        var character = await readingRepository.FindCharacter(characterId, cancellationToken);
        if (character == null)
        {
            throw new HttpException(HttpStatusCode.Gone, "Character not found");
        }

        var game = await gameReadingService.GetGame(character.GameId, cancellationToken);
        await attributeValueFiller.Fill(new[] {character}, game.AttributeSchemaId, cancellationToken);
        return character;
    }

    /// <inheritdoc />
    public async Task MarkAsRead(Guid gameId, CancellationToken cancellationToken)
    {
        await gameReadingService.GetGame(gameId, cancellationToken);
        await unreadCountersRepository.Flush(identityProvider.Current.User.UserId,
            UnreadEntryType.Character, gameId, cancellationToken);
    }
}