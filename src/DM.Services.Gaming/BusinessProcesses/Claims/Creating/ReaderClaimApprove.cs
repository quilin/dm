using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Core.Exceptions;
using DM.Services.Gaming.Dto.Internal;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.BusinessProcesses.Claims.Creating;

/// <inheritdoc />
internal class ReaderClaimApprove(
    IRoomClaimsCreatingRepository creatingRepository) : IReaderClaimApprove
{
    /// <inheritdoc />
    public async Task<Guid> GetParticipantId(string readerLogin, RoomToUpdate room, CancellationToken cancellationToken)
    {
        var readerId = await creatingRepository.FindReaderId(room.Game.Id, readerLogin, cancellationToken);
        if (!readerId.HasValue)
        {
            throw new HttpBadRequestException(new Dictionary<string, string>
            {
                [nameof(RoomClaim.User)] = ValidationError.Invalid
            });
        }

        return readerId.Value;
    }
}