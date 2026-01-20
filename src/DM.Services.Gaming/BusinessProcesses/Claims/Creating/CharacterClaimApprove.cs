using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Core.Exceptions;
using DM.Services.Gaming.Dto.Internal;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.BusinessProcesses.Claims.Creating;

/// <inheritdoc />
internal class CharacterClaimApprove(
    IRoomClaimsCreatingRepository repository) : ICharacterClaimApprove
{
    /// <inheritdoc />
    public async Task<Guid> GetParticipantId(Guid characterId, RoomToUpdate room, CancellationToken cancellationToken)
    {
        var gameId = await repository.FindCharacterGameId(characterId, cancellationToken);
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