using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Exceptions;
using DM.Services.Gaming.Dto.Internal;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.BusinessProcesses.Claims.Creating;

/// <inheritdoc />
internal class CharacterClaimApprove : ICharacterClaimApprove
{
    private readonly IRoomClaimsCreatingRepository repository;

    /// <inheritdoc />
    public CharacterClaimApprove(
        IRoomClaimsCreatingRepository repository)
    {
        this.repository = repository;
    }

    /// <inheritdoc />
    public async Task<Guid> GetParticipantId(Guid characterId, RoomToUpdate room)
    {
        var gameId = await repository.FindCharacterGameId(characterId);
        if (!gameId.HasValue || room.Game.Id != gameId)
        {
            throw new HttpBadRequestException(new Dictionary<string, string>
            {
                [nameof(RoomClaim.Character)] = ValidationError.Invalid
            });
        }

        return characterId;
    }
}