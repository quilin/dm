using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Exceptions;
using DM.Services.Gaming.Dto.Internal;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.BusinessProcesses.Claims.Creating;

/// <inheritdoc />
internal class ReaderClaimApprove : IReaderClaimApprove
{
    private readonly IRoomClaimsCreatingRepository creatingRepository;

    /// <inheritdoc />
    public ReaderClaimApprove(
        IRoomClaimsCreatingRepository creatingRepository)
    {
        this.creatingRepository = creatingRepository;
    }

    /// <inheritdoc />
    public async Task<Guid> GetParticipantId(string readerLogin, RoomToUpdate room)
    {
        var readerId = await creatingRepository.FindReaderId(room.Game.Id, readerLogin);
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